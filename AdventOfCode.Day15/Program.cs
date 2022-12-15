// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 15);

today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var grid = new HashSet<(int x, int y, char element)>();

var sensorBeaconPairs = new Dictionary<(int x, int y), (int x, int y)>();

foreach (var line in lines)
{
    var parts = line.Split(' ');
    var sensorX = int.Parse(new String(parts[2].Skip(2).SkipLast(1).ToArray()));
    var sensorY = int.Parse(new String(parts[3].Skip(2).SkipLast(1).ToArray()));

    grid.Add((sensorX, sensorY, 'S'));

    var beaconX = int.Parse(new String(parts[8].Skip(2).SkipLast(1).ToArray()));
    var beaconY = int.Parse(new String(parts[9].Skip(2).ToArray()));

    grid.Add((beaconX, beaconY, 'B'));

    sensorBeaconPairs.Add((sensorX, sensorY), (beaconX, beaconY));
}

var minX = int.MaxValue;
var maxX = int.MinValue;

var targetY = 2000000;

foreach (var pair in sensorBeaconPairs)
{
    var distanceX = pair.Value.x - pair.Key.x;
    var distanceY = pair.Value.y - pair.Key.y;

    var manhattanDistance = Math.Abs(distanceX) + Math.Abs(distanceY);

    var yList = new List<int> { pair.Key.y - manhattanDistance, pair.Key.y + manhattanDistance };

    if (yList[0] <= targetY && targetY <= yList[1])
    {
        minX = Math.Min(minX, pair.Key.x - (manhattanDistance - Math.Abs(pair.Key.y - targetY)));
        maxX = Math.Max(maxX, pair.Key.x + (manhattanDistance - Math.Abs(pair.Key.y - targetY)));

        Console.WriteLine(
            $"Scanner: {pair.Key.x}, {pair.Key.y} Beacon: {pair.Value.x}, {pair.Value.y}  : minx: {minX}, maxx: {maxX}");
    }
}

var minXs = new Dictionary<int, int>();
var maxXs = new Dictionary<int, int>();
var ranges = new Dictionary<int, List<(int, int)>>();
var part2MaxY = 4000000;
foreach (var pair in sensorBeaconPairs)
{
    var distanceX = pair.Value.x - pair.Key.x;
    var distanceY = pair.Value.y - pair.Key.y;

    var manhattanDistance = Math.Abs(distanceX) + Math.Abs(distanceY);

    for (var y = 0; y < part2MaxY; y++)
    {
        if (Math.Abs(pair.Key.y - y) > manhattanDistance)
        {
            continue;
        }
        
        minXs.TryGetValue(y, out var yMinX);
        yMinX = Math.Min(yMinX, pair.Key.x - (manhattanDistance - Math.Abs(pair.Key.y - y)));
        minXs.Remove(y);
        minXs.Add(y, yMinX);
        
        maxXs.TryGetValue(y, out var yMaxX);
        yMaxX = Math.Max(yMaxX, pair.Key.x + (manhattanDistance - Math.Abs(pair.Key.y - y)));
        maxXs.Remove(y);
        maxXs.Add(y, yMaxX);
        
        ranges.TryGetValue(y, out var yRange);

        var rangeMinX = Math.Min(int.MaxValue, pair.Key.x - (manhattanDistance - Math.Abs(pair.Key.y - y)));
        var rangeMaxX = Math.Max(int.MinValue, pair.Key.x + (manhattanDistance - Math.Abs(pair.Key.y - y)));

        if (yRange == null)
        {
            yRange = new List<(int, int)>();
            ranges.Add(y, yRange);
        }

        yRange.Add((rangeMinX, rangeMaxX));
    }

    Console.WriteLine(
        $"Scanner: {pair.Key.x}, {pair.Key.y} Beacon: {pair.Value.x}, {pair.Value.y}  : minx: {minX}, maxx: {maxX}");
}


Console.WriteLine(grid.Count(g => g.y == targetY && g.element != 'B'));

var beaconsOnTargetCount = grid.Count(g => g.element == 'B' && g.y == targetY);
Console.WriteLine($"b on Target: {beaconsOnTargetCount}");
Console.WriteLine(maxX - minX + 1 - beaconsOnTargetCount);

foreach (var keyValuePair in ranges)
{
    ranges[keyValuePair.Key] = keyValuePair.Value.OrderBy( a => a.Item1).ThenBy(a => a.Item2).ToList();
}
for (var y = 0; y < part2MaxY; y++)
{
    ranges.TryGetValue(y, out var yRanges);

    if (yRanges == null)
    {
        continue;
    }

    var maxXForThisY = yRanges[0].Item2;
    for (var r = 0; r < yRanges.Count; r++)
    {
        var range = yRanges[r];

        var newMinX = range.Item1;
        if (newMinX > maxXForThisY + 1)
        {
            Console.WriteLine($"Found a gap at {maxXForThisY + 1}, {y}");
            Console.WriteLine($"Tuning Freq: {(maxXForThisY + 1L) * 4000000L + y}");
        }
        
        maxXForThisY = Math.Max(maxXForThisY, range.Item2);
    }
    
}