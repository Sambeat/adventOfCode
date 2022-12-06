// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;

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