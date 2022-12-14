// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq;

var today = await Calendar.OpenPuzzleAsync(2022, 14);

// today.PrintLines();

var lines = today.InputLines;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var rockPaths = new char[1000,1000];
var lowestRock = 0;
for (var line = 0; line < lines.Length; line++)
{
    var pairs = lines[line].Split("->");

    for (var pair = 1; pair < pairs.Length; pair++)
    {
        var currentCoordinates = pairs[pair].Split(",").Select(int.Parse).ToArray();
        var previousCoordinates = pairs[pair - 1].Split(",").Select(int.Parse).ToArray();
        
        var xDistance = currentCoordinates[0] - previousCoordinates[0];
        var yDistance = currentCoordinates[1] - previousCoordinates[1];

        //Draw line
        for (var x = 0; x <= Math.Abs(xDistance); x++)
        {
            rockPaths[previousCoordinates[0] + Math.Sign(xDistance) * x,previousCoordinates[1]] = '#';
        }
            
        for (var y = 0; y <= Math.Abs(yDistance); y++)
        {
            rockPaths[previousCoordinates[0],previousCoordinates[1] + Math.Sign(yDistance) * y] = '#';

            if (previousCoordinates[1] + Math.Sign(yDistance) * y > lowestRock)
            {
                lowestRock = previousCoordinates[1] + Math.Sign(yDistance) * y;
            }
        }
    }
}

var rockCount = 0;
foreach (var rock in rockPaths)
{
    if (rock == '#')
    {
        rockCount++;
    }
}
Console.WriteLine($"rockcount: {rockCount}");
Console.WriteLine($"lowestRock: {rockCount}");

var origin = (x: 500, y: 0);
var sandsCount = 0;
while (true)
{
    var sand = origin;

    if (!IsFree(500, 0))
    {
        Console.WriteLine(sandsCount);
        break;
    }
    
    try
    {
        while (IsFree(sand.x, sand.y + 1) || IsFree(sand.x - 1, sand.y + 1) || IsFree(sand.x + 1, sand.y + 1))
        {
            while (IsFree(sand.x, sand.y + 1))
            {
                sand.y++;
            }

            if (IsFree(sand.x - 1, sand.y + 1))
            {
                sand.x--;
                sand.y++;
            }
            else if (IsFree(sand.x + 1, sand.y + 1))
            {
                sand.x++;
                sand.y++;
            }
        }
    } catch (Exception)
    {
        Console.WriteLine(sandsCount);
        break;
    }

    rockPaths[sand.x, sand.y] = 'O';
    
    // Console.WriteLine($"{sand.x}:{sand.y}");
    sandsCount++;
}

bool IsFree(int x, int y)
{
    return rockPaths[x,y] != '#' && rockPaths[x,y] != 'O' && y != lowestRock + 2;
}