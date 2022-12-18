// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 18);

today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var sides = new Dictionary<(double, double, double), int>();
var exposedSides = new Dictionary<(int, int, int), int>();
var cubes = new List<(int, int, int)>();

var offsets = new (double, double, double)[]
    { (0, 0, 0.5), (0, 0.5, 0), (0.5, 0, 0), (0, 0, -0.5), (0, -0.5, 0), (-0.5, 0, 0) };
for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    var parts = line.Split(",").Select(int.Parse).ToArray();

    foreach (var (dx, dy, dz) in offsets)
    {
        var pos = (dx + parts[0], dy + parts[1], dz + parts[2]);

        if (!sides.ContainsKey(pos))
            sides.Add(pos, 0);
        else
        {
            var originalValue = sides[pos];

            sides.Remove(pos);
            sides.Add(pos, originalValue + 1);
        }
    }


    exposedSides.Remove((parts[0], parts[1], parts[2]));

    for (var x = parts[0] - 1; x <= parts[0] + 1; x++)
    {
        if (x == parts[0] || cubes.Contains((x, parts[1], parts[2])))
            continue;

        if (!exposedSides.TryAdd((x, parts[1], parts[2]), 1))
        {
            exposedSides[(x, parts[1], parts[2])]++;
        }
    }

    for (var y = parts[1] - 1; y <= parts[1] + 1; y++)
    {
        if (y == parts[1] || cubes.Contains((parts[0], y, parts[2])))
            continue;

        if (!exposedSides.TryAdd((parts[0], y, parts[2]), 1))
        {
            exposedSides[(parts[0], y, parts[2])]++;
        }
    }

    for (var z = parts[2] - 1; z <= parts[2] + 1; z++)
    {
        if (z == parts[2] || cubes.Contains((parts[0], parts[1], z)))
            continue;

        if (!exposedSides.TryAdd((parts[0], parts[1], z), 1))
        {
            exposedSides[(parts[0], parts[1], z)]++;
        }
    }

    cubes.Add((parts[0], parts[1], parts[2]));
}

Console.WriteLine($"exposed sides: {sides.Select(s => s.Value).Count(v => v == 0)}");
Console.WriteLine($"exposed sides tech 2: {exposedSides.Sum(s => s.Value)}");

var pockets = 0;
for (var x = 0; x < 20; x++)
{
    for (var y = 0; y < 20; y++)
    {
        for (var z = 0; z < 20; z++)
        {
            if (!cubes.Contains((x, y, z)))
            {
                var cubeArounds = 0;
                foreach (var (dx, dy, dz) in offsets)
                {
                    if (sides.ContainsKey((x + dx, y + dy, z + dz)))
                        cubeArounds++;
                }

                if (cubeArounds == 6)
                {
                    pockets++;
                }
            }
        }
    }
}

Console.WriteLine($"pockets: {pockets}");
Console.WriteLine($"truly exposed sides: {sides.Select(s => s.Value).Count(v => v == 0) - (pockets * 6)}");