// See https://aka.ms/new-console-template for morinte information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 18);

today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var sides = new Dictionary<(double, double, double), int>();
var exposedSides = new Dictionary<(int, int, int), int>();
var cubes = new List<(int x, int y, int z)>();

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

var boundedBoxMax = (cubes.Max(c => c.x), cubes.Max(c => c.y), cubes.Max(c => c.z));
var boundedBoxMin = (cubes.Min(c => c.x), cubes.Min(c => c.y), cubes.Min(c => c.z));

var boundedBox = new List<(int x, int y, int z)>();
for (var x = boundedBoxMin.Item1 - 1; x <= boundedBoxMax.Item1 + 1; x++)
{
    for (var y = boundedBoxMin.Item2 - 1; y <= boundedBoxMax.Item2 + 1; y++)
    {
        for (var z = boundedBoxMin.Item3 - 1; z <= boundedBoxMax.Item3 + 1; z++)
        {
            var pos = (x, y, z);
            boundedBox.Add(pos);
        }
    }
}

var airCubes = boundedBox.Except(cubes).ToHashSet();
var startCube = airCubes.MinBy(c => c.x + c.y + c.z);

var outsideAir = new HashSet<(int x, int y, int z)>();
var faces = 0;

buildByBFS(startCube);

void buildByBFS((int x, int y, int z) sc)
{
    var visited = new HashSet<(int x, int y, int z)>();
    var toVisit = new Queue<(int x, int y, int z)>();
    toVisit.Enqueue(sc);
    
    while (toVisit.Any())
    {
        var nextToVisit = toVisit.Dequeue();
    
        if (cubes.Contains(nextToVisit) || visited.Contains(nextToVisit))
        {
            if (cubes.Contains(nextToVisit))
                faces++;
            continue;
        }
    
        outsideAir.Add(nextToVisit);
    
        visited.Add(nextToVisit);
    
    
        var neighbors = Neighbors(nextToVisit);
    
        foreach (var neighbor in neighbors)
        {
            if (!visited.Contains(neighbor))
            {
                
                toVisit.Enqueue(neighbor);
            }
        }
    }
}

Console.WriteLine($"True faces count: {faces}");

IEnumerable<(int x, int y, int z)> Neighbors((int x, int y, int z) cube)
{
    var neighbors = new List<(int x, int y, int z)>();

    // neighbors.Add((cube.x + 1, cube.y, cube.z));
    // neighbors.Add((cube.x - 1, cube.y, cube.z));
    // neighbors.Add((cube.x, cube.y + 1, cube.z));
    // neighbors.Add((cube.x, cube.y - 1, cube.z));
    // neighbors.Add((cube.x, cube.y, cube.z + 1));
    // neighbors.Add((cube.x, cube.y, cube.z - 1));

    for (var x = cube.x - 1; x <= cube.x + 1; x++)
    {
        for (var y = cube.y - 1; y <= cube.y + 1; y++)
        {
            for (var z = cube.z - 1; z <= cube.z + 1; z++)
            {
                if (x == cube.x && y == cube.y && z == cube.z)
                    continue;

                if (Math.Abs(x - cube.x) + Math.Abs(y - cube.y) + Math.Abs(z - cube.z) == 1 &&
                    (airCubes.Contains((x, y, z)) || cubes.Contains((x, y, z))))
                    neighbors.Add((x, y, z));
            }
        }
    }

    return neighbors;
}