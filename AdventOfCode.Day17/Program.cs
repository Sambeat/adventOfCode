// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq.Extensions;

var today = await Calendar.OpenPuzzleAsync(2022, 17);

today.PrintLines();

var lines = today.RawInput.TrimEnd();
// var lines = File.ReadAllText("test.txt");

var rocks = new List<List<(int x, long y)>>
{
    new() { (0, 0), (1, 0), (2, 0), (3, 0) },
    new() { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) },
    new() { (2, 0), (2, 1), (0, 2), (1, 2), (2, 2) },
    new() { (0, 0), (0, 1), (0, 2), (0, 3) },
    new() { (0, 0), (1, 0), (0, 1), (1, 1) }
};

var jetIdx = 0L;
var fallenRocks = 0L;
var nextRockIdx = 0L;
var highestRock = 1L;
List<(int x, long y)>? fallingRock = null;

var map = new Dictionary<(int, long), bool>
{
    { (0, 1), true }, { (1, 1), true }, { (2, 1), true }, { (3, 1), true }, { (4, 1), true }, { (5, 1), true },
    { (6, 1), true }
};

var memo = new Dictionary<(int, int), int>();

while (fallenRocks < 2022)
{
    var initialRockIdx = nextRockIdx;
    var initialJetIdx = jetIdx;

    if (fallingRock == null)
    {
        fallingRock = SpawnRock(nextRockIdx);
        nextRockIdx++;
    }

    while (!fallingRock.Any(r => map.ContainsKey(r)))
    {
        // Task.Delay(200).Wait();
        // Console.Clear();
        // for (var y = -30; y <= 0; y++)
        // {
        //     for (var x = 0; x < 7; x++)
        //     {
        //         Console.Write(map.ContainsKey((x, y)) || fallingRock.Any(r => r.x == x && r.y == y) ? "#" : ".");
        //     }
        //     Console.WriteLine();
        // }
        // Console.WriteLine("GRAVITY");
        // Console.WriteLine(string.Join("-", fallingRock.Select(r => $"({r.x},{r.y})")));
        fallingRock = ApplyJet(jetIdx, fallingRock);
        // Console.WriteLine("JET");
        // Console.WriteLine(string.Join("-", fallingRock.Select(r => $"({r.x},{r.y})")));

        jetIdx++;
        fallingRock = ApplyGravity(fallingRock);
    }
    fallingRock = ApplyGravity(fallingRock, true);

    fallingRock.ForEach(face => map.Add(face, true));

    var initialHighestRock = highestRock;
    highestRock = Math.Min(highestRock, fallingRock.Min(r => r.y));
    
    fallingRock = null;
    fallenRocks++;
    
    // Task.Delay(200).Wait();
    // Console.Clear();
    // for (var y = -30; y <= 0; y++)
    // {
    //     for (var x = 0; x < 7; x++)
    //     {
    //         Console.Write(map.ContainsKey((x, y)) ? "#" : ".");
    //     }
    //     Console.WriteLine();
    // }
}

var earlyPoint = Math.Abs(highestRock) + 1;
Console.WriteLine(earlyPoint);

// var midPoint = (Math.Abs(highestRock) + 1) * (1000000000000L / 50455);
// Console.WriteLine(midPoint);

// fallenRocks = 999999986655L;
//
// while (fallenRocks < 1000000000000L)
// {
//
//     if (fallingRock == null)
//     {
//         fallingRock = SpawnRock(nextRockIdx);
//         nextRockIdx++;
//     }
//
//     while (!fallingRock.Any(r => map.ContainsKey(r)))
//     {
//         // Task.Delay(200).Wait();
//         // Console.Clear();
//         // for (var y = -30; y <= 0; y++)
//         // {
//         //     for (var x = 0; x < 7; x++)
//         //     {
//         //         Console.Write(map.ContainsKey((x, y)) || fallingRock.Any(r => r.x == x && r.y == y) ? "#" : ".");
//         //     }
//         //     Console.WriteLine();
//         // }
//         // Console.WriteLine("GRAVITY");
//         // Console.WriteLine(string.Join("-", fallingRock.Select(r => $"({r.x},{r.y})")));
//         fallingRock = ApplyJet(jetIdx, fallingRock);
//         // Console.WriteLine("JET");
//         // Console.WriteLine(string.Join("-", fallingRock.Select(r => $"({r.x},{r.y})")));
//
//         jetIdx++;
//         fallingRock = ApplyGravity(fallingRock);
//     }
//     fallingRock = ApplyGravity(fallingRock, true);
//
//     fallingRock.ForEach(face => map.Add(face, true));
//
//     highestRock = Math.Min(highestRock, fallingRock.Min(r => r.y));
//     fallingRock = null;
//     fallenRocks++;
//     
//     // Task.Delay(200).Wait();
//     // Console.Clear();
//     // for (var y = -30; y <= 0; y++)
//     // {
//     //     for (var x = 0; x < 7; x++)
//     //     {
//     //         Console.Write(map.ContainsKey((x, y)) ? "#" : ".");
//     //     }
//     //     Console.WriteLine();
//     // }
// }
//
// var latePoint = Math.Abs(highestRock) + 1;
// Console.WriteLine(latePoint);
// Console.WriteLine(midPoint + (latePoint - earlyPoint));


List<(int x, long y)> SpawnRock(long nextRockIdx)
{
    var nextRock = rocks[(int)(nextRockIdx % rocks.Count)];

    var bottomMostSide = nextRock.Max(r => r.Item2);

    var offset = (2, highestRock - 4 - bottomMostSide);

    return nextRock.Select(r => (r.x + offset.Item1, r.y + offset.Item2)).ToList();
}

List<(int x, long y)> ApplyJet(long jetIdx, List<(int x, long y)> rock)
{
    List<(int x, long y)> newRock = lines[(int)(jetIdx % (lines.Length))] switch
    {
        '<' => rock.Select(r => (Math.Max(0, r.x - 1), r.y)).ToList(),
        '>' => rock.Select(r => (Math.Min(6, r.x + 1), r.y)).ToList(),
        _ => throw new Exception()
    };
    
    //Do nothing if jet pushes against a rock or a wall
    if (newRock.Any(r => map.ContainsKey(r)) || newRock.Distinct().Count() != newRock.Count)
    {
        return rock;
    }

    return newRock;
}

List<(int x, long y)> ApplyGravity(List<(int x, long y)> rock, bool reverse = false)
{
    rock = rock.Select(r => (r.x, r.y + (reverse ? -1 : 1))).ToList();

    return rock;
}