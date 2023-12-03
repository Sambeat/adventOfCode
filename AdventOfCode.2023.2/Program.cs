// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq.Extensions;

Console.WriteLine("Hello, World!");

var today = await Calendar.OpenPuzzleAsync(2023, 2);

var lines = today.InputLinesTrimmed;

var sum = 0;
var powerSum = 0;
foreach(var line in lines)
{
    
    var gameId = line.Split(" ")[1].TrimEnd(':');

    var highestBlue = 0;
    var highestGreen = 0;
    var highestRed = 0;

    var sets = line.Split(":")[1].Split(";");

    foreach (var set in sets)
    {
        var cubes = set.Split(",");

        foreach (var cube in cubes)
        {
            var count = cube.Trim().Split(" ")[0];
            var color = cube.Trim().Split(" ")[1];

            switch (color)
            {
                case "blue":
                    if (int.Parse(count) > highestBlue)
                    {
                        highestBlue = int.Parse(count);
                    }
                    break;
                case "green":
                    if (int.Parse(count) > highestGreen)
                    {
                        highestGreen = int.Parse(count);
                    }

                    break;
                case "red":
                    if (int.Parse(count) > highestRed)
                    {
                        highestRed = int.Parse(count);
                    }

                    break;
            }
        }
    }

    if (highestBlue <= 14 && highestGreen <= 13 && highestRed <= 12)
    {
        sum += int.Parse(gameId);
    }
    
    powerSum += highestBlue * highestGreen * highestRed;
}

//5050 too high
Console.WriteLine(sum);
Console.WriteLine(powerSum);