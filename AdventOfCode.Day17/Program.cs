// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq.Extensions;

var today = await Calendar.OpenPuzzleAsync(2022, 17);

today.PrintLines();

// var lines = today.RawInput;
var lines = File.ReadAllText("test.txt");

var rocks = new List<List<(int x, int y)>>
{
    new List<(int x, int y)> { (0, 0), (1, 0), (2, 0), (3, 0) },
    new List<(int x, int y)> { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) },
    new List<(int x, int y)> { (2, 0), (2, 1), (0, 2), (1, 2), (2, 2) },
    new List<(int x, int y)> { (0, 0), (0, 1), (0, 2), (0, 3) },
    new List<(int x, int y)> { (0, 0), (1, 0), (0, 1), (1, 1) }
};

var jetIdx = 0;
var fallenRocks = 0;
var nextRockIdx = 0;
var highestRock = 1;
List<(int x, int y)>? fallingRock = null;

var map = new Dictionary<(int, int), bool>
{
    { (0, 1), true }, { (1, 1), true }, { (2, 1), true }, { (3, 1), true }, { (4, 1), true }, { (5, 1), true },
    { (6, 1), true }
};

while (fallenRocks <= 2022)
{
    if (fallingRock == null)
    {
        fallingRock = SpawnRock(nextRockIdx);
        nextRockIdx++;
    }

    while (!fallingRock.Any(r => map.ContainsKey(r)))
    {
        Console.WriteLine("GRAVITY");
        Console.WriteLine(string.Join("-", fallingRock.Select(r => $"({r.x},{r.y})")));
        fallingRock = ApplyJet(jetIdx, fallingRock);
        Console.WriteLine("JET");
        Console.WriteLine(string.Join("-", fallingRock.Select(r => $"({r.x},{r.y})")));

        jetIdx++;
        fallingRock = ApplyGravity(fallingRock);
    }
    fallingRock = ApplyGravity(fallingRock, true);

    fallingRock.ForEach(face => map.Add(face, true));

    highestRock = Math.Min(highestRock, fallingRock.Max(r => r.y));
    fallingRock = null;
    fallenRocks++;
}

Console.WriteLine(highestRock);

List<(int x, int y)> SpawnRock(int nextRockIdx)
{
    var nextRock = rocks[nextRockIdx % rocks.Count];

    var bottomMostSide = nextRock.Max(r => r.Item2);

    var offset = (2, highestRock - 3 - bottomMostSide);

    return nextRock.Select(r => (r.x + offset.Item1, r.y + offset.Item2)).ToList();
}

List<(int x, int y)> ApplyJet(int jetIdx, List<(int x, int y)> rock)
{
    List<(int x, int y)> newRock = lines[jetIdx % lines.Length] switch
    {
        '<' => rock.Select(r => (Math.Max(0, r.x - 1), r.y)).ToList(),
        '>' => rock.Select(r => (Math.Min(6, r.x + 1), r.y)).ToList(),
        _ => throw new Exception()
    };
    
    //Do nothing if jet pushes against a rock or a wall
    if (newRock.Any(r => map.ContainsKey(r)) || newRock.Select(r => r.x).Distinct().Count() != newRock.Count)
    {
        return rock;
    }

    return newRock;
}

List<(int x, int y)> ApplyGravity(List<(int x, int y)> rock, bool reverse = false)
{
    rock = rock.Select(r => (r.x, r.y + (reverse ? -1 : 1))).ToList();

    return rock;
}