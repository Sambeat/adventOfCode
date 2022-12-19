// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Diagnostics;
using AdventOfCdoe.Day16;
using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 16);

today.PrintLines();

// var lines = today.InputLinesTrimmed;
var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();


var valves = new List<Valve>();

foreach (var line in lines)
{
    var parts = line.Split(" ");

    var valveName = parts[1];
    var flow = int.Parse(new String(Enumerable.SkipLast(parts[4].Skip(5), 1).ToArray()));

    var tunnels = parts.Where((p, i) => i > 8);
    
    valves.Add(new Valve
    {
        Name = valveName,
        Flow = flow,
        Tunnels = tunnels.Select(t => new string(t.Take(2).ToArray())).ToList()
    });
}

var unopenedValvesWithFlow = valves.Where(v => v.Flow > 0).ToList();

var pairWiseDistances = new Dictionary<string, Dictionary<string, int>>();

var startingLocation = valves.Single(v => v.Name == "AA");
ComputeShortestPathLinear(startingLocation);
foreach (var valve in unopenedValvesWithFlow)
{
    ComputeShortestPathLinear(valve);
}

Console.WriteLine("computed shortest paths");

// var currentMinute = 1;
var maxMinutes = 26;
var maxPressureReleasing = 0;
var maxCurrentMinute = 0;

//Part 1, set max to 30 minutes
// var maxPressureReleased = Move(startingLocation, 0, 1, 0, unopenedValvesWithFlow.Select(v => v.Name).ToList());
//
// Console.WriteLine(maxPressureReleased);
// Console.WriteLine(maxPressureReleasing);
// Console.WriteLine(maxCurrentMinute);

//Part 2. set max to 26 minutes
var subSets = ComputeAllSubsets(unopenedValvesWithFlow).ToList();
Console.WriteLine("pop");

var maxPressuresReleased = new ConcurrentBag<(int, List<string>, List<string>)>();

var watch = Stopwatch.StartNew();

Parallel.ForEach(subSets, (set) =>
{
    var humanPressure = Move(startingLocation, 0, 1, 0, set.Item1.Select(v => v.Name).ToList(), new List<string>());
    var elephantPressure = Move(startingLocation, 0, 1, 0, set.Item2.Select(v => v.Name).ToList(), new List<string>());
    
    maxPressuresReleased.Add((humanPressure.Item1 + elephantPressure.Item1, humanPressure.Item2, elephantPressure.Item2));

    if (maxPressuresReleased.Count % 1000 == 0)
    {
        Console.WriteLine($"Progress: {maxPressuresReleased.Count}/{(double)subSets.Count}");
    }
});

// foreach (var set in subSets)
// {
//     var humanPressure = Move(startingLocation, 0, 1, 0, set.Item1.Select(v => v.Name).ToList());
//
//     var elephantPressure = Move(startingLocation, 0, 1, 0, set.Item2.Select(v => v.Name).ToList());
//
//     maxPressuresReleased.Add(humanPressure + elephantPressure);
//
//     if (maxPressuresReleased.Count % 1000 == 0)
//     {
//         Console.WriteLine($"Progress: {maxPressuresReleased.Count}/{(double)subSets.Count}");
//     }
// }

Console.WriteLine($"time: {watch.Elapsed}");

Console.WriteLine(maxPressureReleasing);
Console.WriteLine(maxCurrentMinute);

var bestPressureAndPath = maxPressuresReleased.MaxBy(mpr => mpr.Item1);
Console.WriteLine($"Human Path: {string.Join(',', bestPressureAndPath.Item2)}");
Console.WriteLine($"Elephant Path: {string.Join(',', bestPressureAndPath.Item3)}");

Console.WriteLine(bestPressureAndPath.Item1);

(int, List<string>) Move(Valve currentLocation, int pressureReleasedTotal, int currentMinute, int pressureReleasing, List<string> unvisitedValves, List<string> path)
{
    path.Add(currentLocation.Name);

    maxPressureReleasing = Math.Max(maxPressureReleasing, pressureReleasing);
    if (currentMinute >= maxMinutes)
    {
        pressureReleasedTotal -= (currentMinute - maxMinutes - 1) * pressureReleasing;

        maxCurrentMinute = Math.Max(currentMinute, maxCurrentMinute);
        return (pressureReleasedTotal, path);
    }

    unvisitedValves.Remove(currentLocation.Name);
    var availablePaths = pairWiseDistances[currentLocation.Name].Where(kvp => unvisitedValves.Contains(kvp.Key));

    var pressures = availablePaths
        .Select(p => Move(valves.Single(v => v.Name == p.Key),
            pressureReleasedTotal + pressureReleasing * (p.Value + 1),
            currentMinute + p.Value + 1, 
            pressureReleasing + valves.Single(v => v.Name == p.Key).Flow, new List<string>(unvisitedValves), new List<string>(path)));


    if (pressures.Any())
    {
        return pressures.MaxBy(p => p.Item1);
    }
    else
    {
        maxCurrentMinute = Math.Max(currentMinute, maxCurrentMinute);
        return (pressureReleasedTotal + (maxMinutes - currentMinute + 1) * pressureReleasing, path);
    }
}

void ComputeShortestPathLinear(Valve currentLocation)
{
    pairWiseDistances.Add(currentLocation.Name, new Dictionary<string, int>());
    
    var visited = new HashSet<string>();
    var queueDistinctTracker = new HashSet<string>();
    var toVisit = new Queue<(int s, Valve valve)>();
    toVisit.Enqueue((0, currentLocation));
    queueDistinctTracker.Add(currentLocation.Name);
    
    while (toVisit.Any())
    {
        
        
        var nextToVisit = toVisit.Dequeue();
        queueDistinctTracker.Remove(nextToVisit.valve.Name);

        if (nextToVisit.valve.Flow != 0 && nextToVisit.valve.Name != currentLocation.Name)
        {
            pairWiseDistances[currentLocation.Name].Add(nextToVisit.valve.Name, nextToVisit.s);
        }
        
        visited.Add(nextToVisit.valve.Name);

        var neighbors = nextToVisit.valve.Tunnels;

        foreach (var neighbor in neighbors)
        {
            if (!visited.Contains(neighbor) && !queueDistinctTracker.Contains(neighbor))
            {
                toVisit.Enqueue((nextToVisit.s + 1, valves.Single(v => v.Name == neighbor)));
                queueDistinctTracker.Add(neighbor);
            }
        }
    }
}

IEnumerable<(List<Valve>, List<Valve>)>
    ComputeAllSubsets(List<Valve> initialSet)
{
    var subsetQty = (int)Math.Pow(2, initialSet.Count);
    var subsets = new List<(List<Valve>, List<Valve>)>(subsetQty);
    var uniqueKeys = new HashSet<string>();

    for (var subSet = 0; subSet < subsetQty; subSet++)
    {
        var firstSubSet = new List<Valve>();
        var secondSubSet = new List<Valve>();
        for(var bitMask = 0; bitMask < initialSet.Count; bitMask++)
        {
            if ((subSet & (1 << bitMask)) != 0)
            {
                firstSubSet.Add(initialSet.ElementAt(bitMask));
            }
            else
            {
                secondSubSet.Add(initialSet.ElementAt(bitMask));
            }
        }
        
        var firstSubSetKeys = string.Join("", firstSubSet.Select(v => v.Name).OrderBy(s => s));
        
        var subSetAcceptableRange = firstSubSet.Count >= (initialSet.Count / 2 - 0) && firstSubSet.Count <= (initialSet.Count / 2 + 1);
        if (subSetAcceptableRange && !uniqueKeys.Contains(firstSubSetKeys))
        {
            uniqueKeys.Add(firstSubSetKeys);
            subsets.Add((firstSubSet, secondSubSet));
        }
    }

    return subsets;
}