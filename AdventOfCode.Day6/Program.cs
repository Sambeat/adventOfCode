// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;
using MoreLinq.Extensions;

Console.WriteLine("Hello, World!");

var today = await Calendar.OpenPuzzleAsync(2022, 6);

var lines = today.RawInput;

Console.WriteLine(lines);
Console.WriteLine(lines.Length);

var start = 0;

while (start <= lines.Length - 14)
{
    var lettersCount = lines.Substring(start, 14).Distinct().Count();

    if (lettersCount == 14)
    {
        Console.WriteLine(lines.Substring(start, 14));
        break;
    }

    start++;
}

Console.WriteLine(start + 14);


//Bonus one liner for fun since the guy must be smoking something to put this as the day 6 puzzle 
var quick = lines.IndexOf(
    new string(
        lines.Window(14).First(
            s => 
                s
                .Distinct()
                .Count() == 14)
            .ToArray()
        ),
    StringComparison.Ordinal) + 14;
Console.WriteLine(quick);