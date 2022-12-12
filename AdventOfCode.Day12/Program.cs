using AdventOfCode.Day12;
using AdventOfCode.Events;
using MoreLinq;

var today = await Calendar.OpenPuzzleAsync(2022, 12);

// today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();


var tiles = new Tile[lines.Length][];
Tile? startTile = null;
Tile? endTile = null;
for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];

    for (var j = 0; j < line.Length; j++)
    {
        tiles[i] ??= new Tile[line.Length];

        if (line[j] == 'S')
        {
            tiles[i][j] = new Tile { I = i, J = j, Height = (int)'a' };
            startTile = tiles[i][j];
        }
        else if (line[j] == 'E')
        {
            tiles[i][j] = new Tile { I = i, J = j, Height = (int)'z' };
            endTile = tiles[i][j];
        }
        else
        {
            tiles[i][j] = new Tile { I = i, J = j, Height = (int)line[j] };
        }
    }
}

var paths = new List<List<Tile>>();

var steps = forward(startTile);
Console.WriteLine(steps);

int forward(Tile tile)
{
    var visited = new HashSet<Tile>();
    var toVisit = new Queue<(int s, Tile tile)>();
    toVisit.Enqueue((0, tile));
    
    while (toVisit.Any())
    {
        var nextToVisit = toVisit.Dequeue();
        if (Equals(nextToVisit.tile, endTile))
        {
            return nextToVisit.s;
        }
        visited.Add(nextToVisit.tile);
        Neighbors(nextToVisit.tile).Where(n => !visited.Contains(n)).ForEach(n => toVisit.Enqueue((nextToVisit.s + 1 , n)));
    }

    return 0;
}

// var path = AStar(startTile, endTile);
// Console.WriteLine(path.Count());

IEnumerable<Tile> Neighbors(Tile tile)
{
    var neighbors = new List<Tile>();
    var currentTile = tiles[tile.I][tile.J];

    if (tile.I > 0 && (tiles[tile.I - 1][tile.J].Height - currentTile.Height == 1 ||
                       tiles[tile.I - 1][tile.J].Height - currentTile.Height == 0))
    {
        neighbors.Add(tiles[tile.I - 1][tile.J]);
    }

    if (tile.I < tiles.Length - 1 && (tiles[tile.I + 1][tile.J].Height - currentTile.Height == 1 ||
                                      tiles[tile.I + 1][tile.J].Height - currentTile.Height == 0))
    {
        neighbors.Add(tiles[tile.I + 1][tile.J]);
    }

    if (tile.J > 0 && (tiles[tile.I][tile.J - 1].Height - currentTile.Height == 1 ||
                       tiles[tile.I][tile.J - 1].Height - currentTile.Height == 0))
    {
        neighbors.Add(tiles[tile.I][tile.J - 1]);
    }

    if (tile.J < tiles[tile.I].Length - 1 && (tiles[tile.I][tile.J + 1].Height - currentTile.Height == 1 ||
                                              tiles[tile.I][tile.J + 1].Height - currentTile.Height == 0))
    {
        neighbors.Add(tiles[tile.I][tile.J + 1]);
    }

    foreach (var neighbor in neighbors)
    {
        neighbor.Cost = currentTile.Cost + 1;
        neighbor.Parent = currentTile;
    }

    return neighbors;
}

IEnumerable<Tile> ConstructPath(Tile tile)
{
    var path = new HashSet<Tile>();
    while (!Equals(tile.Parent, startTile))
    {
        Console.WriteLine($"{(char)tile.Height}: {tile.I}, {tile.J}");
        //Do I want the initial node here?
        tile = tile.Parent;
        path.Add(tile);
    }

    return path;
}

int Heuristic(Tile tile)
{
    return Math.Abs(tile.I - endTile.I) + Math.Abs(tile.J - endTile.J);
}


IEnumerable<Tile> AStar(Tile start, Tile goal)
{
    var accessible = new HashSet<Tile>() { start };
    var visited = new HashSet<Tile>();
    start.Cost = 0;
    start.FullCost = start.Cost + Heuristic(start);
    while (accessible.Any())
    {
        var current = Enumerable.MinBy(accessible, t => t.FullCost);
        if (Equals(current, goal))
        {
            return ConstructPath(current);
        }

        accessible.Remove(current);
        visited.Add(current);

        foreach (var neighbor in Neighbors(current))
        {
            if (visited.Contains(neighbor))
            {
                continue;
            }

            neighbor.FullCost = neighbor.Cost + Heuristic(neighbor);
            if (!accessible.Contains(neighbor))
            {
                accessible.Add(neighbor);
            }
            else
            {
                var accessibleNeighbor = accessible.First(t => t.Equals(neighbor));
                if (neighbor.Cost < accessibleNeighbor.Cost)
                {
                    accessibleNeighbor.Cost = neighbor.Cost;
                    accessibleNeighbor.Parent = neighbor.Parent;
                }
            }
        }
    }

    return new List<Tile>();
}