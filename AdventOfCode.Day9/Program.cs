// See https://aka.ms/new-console-template for more information

using System.Drawing;
using AdventOfCode.Day9;
using AdventOfCode.Events;

Console.WriteLine("Hello, World!");

var today = await Calendar.OpenPuzzleAsync(2022, 9);

today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var visited = new HashSet<(int x, int y)>();
var k10Visited = new HashSet<(int x, int y)>();

var knots = new List<PP>(){new(0, 0),new(0, 0),new(0, 0),new(0, 0),new(0, 0),new(0, 0),new(0, 0),new(0, 0),new(0, 0),new()};

var head = (0, 0);
var tail = (0, 0);

visited.Add((0, 0));
k10Visited.Add((0, 0));

foreach (var line in lines)
{
    var command = line.Split(" ");
    
    move(command);
}

Console.WriteLine(visited.Count);
Console.WriteLine(k10Visited.Count);

void move(string[] command)
{
    var direction = command[0];
    var steps = int.Parse(command[1]);

    for (var i = 0; i < steps; i++)
    {
        switch (direction)
        {
            case "U":
                head.Item2 += 1;
                knots[0].y += 1;
                break;
            case "D":
                head.Item2 -= 1;
                knots[0].y -= 1;

                break;
            case "R":
                head.Item1 += 1;
                knots[0].x += 1;

                break;
            case "L":
                head.Item1 -= 1;
                knots[0].x -= 1;

                break;
        }
        
        for (var k = 1; k < knots.Count; k++)
        {
            // for (int j = -6; j < 6; j++)
            // {
            //     for (var l = -6; l < 6; l++)
            //     {
            //         var match = knots.Where(kn => kn.x == j && kn.y == l).MinBy(kn => kn.index);
            //         if (match != null)
            //         {
            //             Console.Write($"{match.index}");
            //         }
            //         else
            //         {
            //             Console.Write(".");
            //         }
            //     }
            //     Console.WriteLine();
            // }
            //
            // Console.WriteLine("------");
            
            var parentDistance = (knots[k-1].x - knots[k].x, knots[k-1].y - knots[k].y);

            if (Math.Abs(parentDistance.Item1) <= 1 && Math.Abs(parentDistance.Item2) <= 1)
            {
                continue;
            }
            else if (Math.Abs(parentDistance.Item1) == 1 && Math.Abs(parentDistance.Item2) != 2)
            {
                knots[k].y += (knots[k-1].y - knots[k].y);
            } else if (Math.Abs(parentDistance.Item1) != 2 && Math.Abs(parentDistance.Item2) == 1)
            {
                knots[k].x += (knots[k-1].x - knots[k].x);
            }
            else
            {
                knots[k].y += 1 * Math.Sign(knots[k-1].y - knots[k].y);
                knots[k].x += 1 * Math.Sign(knots[k-1].x - knots[k].x);
            }

            
        }
        // Console.WriteLine($"{knots[9].x},{knots[9].y}");
        k10Visited.Add((knots[9].x, knots[9].y));

        var distance = (head.Item1 - tail.Item1, head.Item2 - tail.Item2);
        
        if (Math.Abs(distance.Item1) <= 1 && Math.Abs(distance.Item2) <= 1)
        {
            continue;
        }
        else if (Math.Abs(distance.Item1) == 1 && Math.Abs(distance.Item2) != 2)
        {
            tail.Item2 += (head.Item2 - tail.Item2);
        } else if (Math.Abs(distance.Item1) != 2 && Math.Abs(distance.Item2) == 1)
        {
            tail.Item1 += (head.Item1 - tail.Item1);
        }
        else
        {
            tail.Item2 += 1 * Math.Sign(head.Item2 - tail.Item2);
            tail.Item1 += 1 * Math.Sign(head.Item1 - tail.Item1);
        }
        
        visited.Add((tail.Item1, tail.Item2));
    }
}