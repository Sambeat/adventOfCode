// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
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

var maxPressureReleased = Move(currentLocation, 0, currentMinute, 0);

Console.WriteLine(maxPressureReleased);

int Move(Valve currentLocation, int pressureReleasedTotal, int currentMinute, int pressureReleasing, List<string> unvisitedValves)
{
    
    if (currentMinute >= 30)
    {
        return pressureReleasedTotal;
    }

    var availablePaths = pairWiseDistances[currentLocation.Name];

    var pressures = availablePaths
        .Select(p => Move(valves.Single(v => v.Name == p.Key),
            pressureReleasedTotal + pressureReleasing * (currentMinute + p.Value + 1),
            currentMinute + p.Value + 1, 
            pressureReleasing + valves.Single(v => v.Name == p.Key).Flow), availableDistances.);

    return pressures.Max();
}


// var currentMinute = 1;
// var pressureReleased = 0;
// Console.WriteLine($"Minute: {currentMinute}, Current Release: {openedValves.Sum(v => v.Flow)}, Total Pressure Released: {pressureReleased}");
//
// var currentLocation = valves.Single(v => v.Name == "AA");
// while (currentMinute < 30)
// {
//     var hasToReevaluate = false;
//     var pathV = DetermineTarget(currentLocation, currentMinute);
//
//     if (pathV == null)
//     {
//         pressureReleased += openedValves.Sum(v => v.Flow);
//         currentMinute++;
//     }
//     else
//     {
//
//         //Moving
//         for (var p = 0; p < pathV?.path.Count; p++)
//         {
//             Console.WriteLine($"Minute: {currentMinute}, Current Release: {openedValves.Sum(v => v.Flow)}, Total Pressure Released: {pressureReleased}, Opened valves: {string.Join(',',openedValves.Select(v => v.Name))}");
//
//             var pathValve = pathV?.path[p];
//
//             if (p + 1 == pathV?.path.Count)
//             {
//                 Console.WriteLine($"MOVING TO {pathV?.v.Name}");
//             }
//             else
//             {
//                 Console.WriteLine($"MOVING TO {pathV?.path[p+1]}");
//
//             }
//
//             pressureReleased += openedValves.Sum(v => v.Flow);
//             currentMinute++;
//             
//             var unopenedValveOnPath = unopenedValvesWithFlow.SingleOrDefault(v => v.Name == pathValve);
//             
//             Console.WriteLine($"Minute: {currentMinute}, Current Release: {openedValves.Sum(v => v.Flow)}, Total Pressure Released: {pressureReleased}, Opened valves: {string.Join(',',openedValves.Select(v => v.Name))}");
//             
//             if (unopenedValveOnPath != null && unopenedValveOnPath.Flow * (pathV?.path.Count - p) > pathV?.v.Flow)
//             {
//                 openedValves.Add(unopenedValveOnPath);
//                 unopenedValvesWithFlow.Remove(unopenedValveOnPath);
//                 currentLocation = unopenedValveOnPath;
//                 hasToReevaluate = true;
//                 Console.WriteLine($"Special! Minute: {currentMinute}, Current Release: {openedValves.Sum(v => v.Flow)}, Total Pressure Released: {pressureReleased}, Opened valves: {string.Join(',',openedValves.Select(v => v.Name))}");
//
//                 break;
//                 
//             }
//         }
//
//         if (hasToReevaluate)
//         {
//             continue;
//         }
//
//
//         //Opening Valve
//         pressureReleased += openedValves.Sum(v => v.Flow);
//         currentMinute++;
//         openedValves.Add(pathV?.v!);
//         unopenedValvesWithFlow.Remove(pathV?.v!);
//         currentLocation = pathV?.v;            
//      
//         Console.WriteLine($"Minute: {currentMinute}, Current Release: {openedValves.Sum(v => v.Flow)}, Total Pressure Released: {pressureReleased}, Opened valves: {string.Join(',',openedValves.Select(v => v.Name))}");
//     }
//     
//     Console.WriteLine($"Minute: {currentMinute}, Current Release: {openedValves.Sum(v => v.Flow)}, Total Pressure Released: {pressureReleased}, Opened valves: {string.Join(',',openedValves.Select(v => v.Name))}");
//
// }
//
// // var pressureReleased = Release(valves.Single(v => v.Name == "AA"), 0, 0, ImmutableHashSet<Valve>.Empty);
// Console.WriteLine(pressureReleased);


//
// (Valve v, List<string> path)? DetermineTarget(Valve currentLocation, int currentMinute)
// {
//     var paths = unopenedValvesWithFlow.Where(v => v.Name != currentLocation.Name).Select(v => (v, ComputeShortestPath(currentLocation, v))).ToList();
//
//     var pressurePotentials = paths.Select(vp => (vp.v, (30 - currentMinute - (vp.Item2.Count)) * vp.v.Flow));
//
//     if (!pressurePotentials.Any())
//     {
//         return null;
//     }
//     
//     
//     
//     var targetValve = Enumerable.MaxBy(pressurePotentials, (vpotential) => vpotential.Item2).v;
//
//     return paths.Single(pathv => pathv.v.Name == targetValve.Name);
//
//     // return paths.MinBy(pathv => pathv.Item2.Count);
// }
//
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
//
// List<string> ComputeShortestPath(Valve currentLocation, Valve target)
// {
//     return ComputeShortestPathRecursive(currentLocation, target, ImmutableHashSet<string>.Empty);
// }
//
// List<string> ComputeShortestPathRecursive(Valve currentLocation, Valve target, ImmutableHashSet<string> moves)
// {
//     if (currentLocation == target)
//     {
//         return moves.ToList();
//     }
//     
//     moves = moves.Add(currentLocation.Name);
//
//     var allPossibleMoves = new List<List<string>>();
//
//     foreach (var tunnel in currentLocation.Tunnels)
//     {
//         if (!moves.Contains(tunnel))
//         {
//             allPossibleMoves.Add(ComputeShortestPathRecursive(valves.Single(v => v.Name == tunnel), target, moves));
//         }
//     }
//
//     return Enumerable.MinBy(allPossibleMoves, m => m.Count) ?? moves.ToList();
// }
//


// int Release(Valve currentLocation, int pressureReleased, int minutesElapsed, ImmutableHashSet<Valve> openedValves)
// {
//     if (minutesElapsed == 30)
//     {
//         return pressureReleased;
//     }
//
//     var pressures = new List<int>();
//
//     if (currentLocation.Flow == 0 || openedValves.Any(v => v.Name == currentLocation.Name))
//     {
//         Parallel.ForEach(currentLocation.Tunnels, (t) =>
//         {
//             var valve = valves.Single(v => v.Name == t);
//
//             pressures.Add(
//                 Release(valve, pressureReleased + openedValves.Sum(v => v.Flow), minutesElapsed + 1, openedValves));
//         });
//     }  
//
//     // foreach (var currentLocationTunnel in currentLocation.Tunnels)
//     // {
//     //     var valve = valves.Single(v => v.Name == currentLocationTunnel);
//     //
//     //     pressures.Add(Release(valve, pressureReleased + openedValves.Sum(v => v.Flow), minutesElapsed + 1, openedValves));
//     // }
//
//      else 
//     {
//         pressures.Add(Release(currentLocation, pressureReleased + openedValves.Sum(v => v.Flow), minutesElapsed + 1,
//             openedValves.Add(currentLocation)));
//     }
//
//     return pressures.Max();
// }

// int Move(Valve currentLocation, int pressureReleased, int minutesElapsed, ImmutableHashSet<Valve> openedValves)
// {
//     if (minutesElapsed == 30)
//     {
//         return pressureReleased;
//     }
//     
//     var pressures = new List<int>();
//     foreach (var currentLocationTunnel in currentLocation.Tunnels)
//     {
//         var valve = valves.Single(v => v.Name == currentLocationTunnel);
//
//         pressures.Add(Move(valve, pressureReleased + openedValves.Sum(v => v.Flow), minutesElapsed + 1, openedValves));
//     }
//
//     pressures.Add(Release(currentLocation, pressureReleased + openedValves.Sum(v => v.Flow), minutesElapsed + 1, openedValves.Add(currentLocation)));
//
//     return pressures.Max();
// }

