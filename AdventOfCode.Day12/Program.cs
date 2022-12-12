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

var startTiles = new List<Tile>();

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
            if (line[j] == 'a')
            {
                startTiles.Add(tiles[i][j]);
            }
        }
    }
}

var paths = new List<List<Tile>>();

var steps = forward(startTile);
Console.WriteLine(steps);

var stepCounts = startTiles.Select(forward);

Console.WriteLine(stepCounts.Min());

int forward(Tile tile)
{
    var visited = new HashSet<Tile>();
    var queueDistinctTracker = new HashSet<Tile>();
    var toVisit = new Queue<(int s, Tile tile)>();
    toVisit.Enqueue((0, tile));
    queueDistinctTracker.Add(tile);
    
    while (toVisit.Any())
    {
        var nextToVisit = toVisit.Dequeue();
        queueDistinctTracker.Remove(nextToVisit.tile);

        if (Equals(nextToVisit.tile, endTile))
        {
            return nextToVisit.s;
        }
        
        visited.Add(nextToVisit.tile);
        
        var neighbors = Neighbors(nextToVisit.tile);

        foreach (var neighbor in neighbors)
        {
            if (!visited.Contains(neighbor) && !queueDistinctTracker.Contains(neighbor))
            {
                toVisit.Enqueue((nextToVisit.s + 1, neighbor));
                queueDistinctTracker.Add(neighbor);
            }
        }
    }

    return int.MaxValue;
}

IEnumerable<Tile> Neighbors(Tile tile)
{
    var neighbors = new List<Tile>();
    var currentTile = tiles[tile.I][tile.J];

    if (tile.I > 0 && (tiles[tile.I - 1][tile.J].Height - currentTile.Height <= 1))
    {
        neighbors.Add(tiles[tile.I - 1][tile.J]);
    }

    if (tile.I < tiles.Length - 1 && (tiles[tile.I + 1][tile.J].Height - currentTile.Height <= 1))
    {
        neighbors.Add(tiles[tile.I + 1][tile.J]);
    }

    if (tile.J > 0 && (tiles[tile.I][tile.J - 1].Height - currentTile.Height <= 1))
    {
        neighbors.Add(tiles[tile.I][tile.J - 1]);
    }

    if (tile.J < tiles[tile.I].Length - 1 && (tiles[tile.I][tile.J + 1].Height - currentTile.Height <= 1))
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