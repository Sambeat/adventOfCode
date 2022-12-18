// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using AdventOfCdoe.Day16;
using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 16);

today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();


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
var openedValves = new List<Valve>();

var pairWiseDistances = new Dictionary<string, Dictionary<string, int>>();

var currentLocation = valves.Single(v => v.Name == "AA");
ComputeShortestPathLinear(currentLocation);
foreach (var valve in unopenedValvesWithFlow)
{
    ComputeShortestPathLinear(valve);
}

Console.WriteLine("computed shortest paths");

var currentMinute = 1;
var maxPressureReleasing = 0;
var maxCurrentMinute = 0;

// Part 1, set max to 30 minutes
 var maxPressureReleased = Move(currentLocation, 0, currentMinute, 0, unopenedValvesWithFlow.Select(v => v.Name).ToList());

 Console.WriteLine(maxPressureReleased);
 Console.WriteLine(maxPressureReleasing);
 Console.WriteLine(maxCurrentMinute);

//Part 2. set max to 26 minutes


int Move(Valve currentLocation, int pressureReleasedTotal, int currentMinute, int pressureReleasing, List<string> unvisitedValves)
{
    maxPressureReleasing = Math.Max(maxPressureReleasing, pressureReleasing);
    if (currentMinute >= 30)
    {
        pressureReleasedTotal = pressureReleasedTotal - (currentMinute - 30 - 1) * pressureReleasing;
        
        maxCurrentMinute = Math.Max(currentMinute, maxCurrentMinute);
        return pressureReleasedTotal;
    }

    unvisitedValves.Remove(currentLocation.Name);
    var availablePaths = pairWiseDistances[currentLocation.Name].Where(kvp => unvisitedValves.Contains(kvp.Key));

    var pressures = availablePaths
        .Select(p => Move(valves.Single(v => v.Name == p.Key),
            pressureReleasedTotal + pressureReleasing * (p.Value + 1),
            currentMinute + p.Value + 1, 
            pressureReleasing + valves.Single(v => v.Name == p.Key).Flow, new List<string>(unvisitedValves)));


    if (pressures.Any())
    {
        return pressures.Max();
    }
    else
    {
        return pressureReleasedTotal + (30 - currentMinute + 1) * pressureReleasing;
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