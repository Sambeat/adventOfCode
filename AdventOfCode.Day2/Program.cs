// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;

Console.WriteLine("Hello, World!");

var day2 = await Calendar.OpenPuzzleAsync(2022, 2);

var lines = day2.InputLines.Where(l => l != String.Empty).ToList();

var score = 0;
foreach (var line in lines)
{
    var parts = line.Split(' ');

    switch (parts[1].ToLower())
    {
        case "x":
            score += 1;
            switch (parts[0].ToLower())
            {
                case "a":
                    score += 3;
                    break;
                case "c":
                    score += 6;
                    break;
            }
            break;
        case "y":
            score += 2;
            switch (parts[0].ToLower())
            {
                case "a":
                    score += 6;
                    break;
                case "b":
                    score += 3;
                    break;
            }
            
            break;
        case "z":
            score += 3;
            switch (parts[0].ToLower())
            {
                case "b":
                    score += 6;
                    break;
                case "c":
                    score += 3;
                    break;
            }
            break;
    }
    
    
}

//Part 1 answer
Console.WriteLine(score);

score = 0;
foreach (var line in lines)
{
    var parts = line.Split(' ');

    switch (parts[1].ToLower())
    {
        case "x":
            score += 0;
            switch (parts[0].ToLower())
            {
                case "a":
                    score += 3;
                    break;
                case "b":
                    score += 1;
                    break;
                case "c":
                    score += 2;
                    break;
            }
            break;
        case "y":
            score += 3;
            switch (parts[0].ToLower())
            {
                case "a":
                    score += 1;
                    break;
                case "b":
                    score += 2;
                    break;
                case "c":
                    score += 3;
                    break;
            }
            
            break;
        case "z":
            score += 6;
            switch (parts[0].ToLower())
            {
                case "a":
                    score += 2;
                    break;
                case "b":
                    score += 3;
                    break;
                case "c":
                    score += 1;
                    break;
            }
            break;
    }
    
    
}

//Part 2 answer
Console.WriteLine(score);