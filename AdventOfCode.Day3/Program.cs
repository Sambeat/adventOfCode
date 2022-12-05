// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;

Console.WriteLine("Hello, World!");

var today = await Calendar.OpenPuzzleAsync(2022, 3);

var lines = today.InputLines.Where(s => s != string.Empty).ToList();

var sum = 0;
foreach (var line in lines)
{
    var firstHalf = line.Substring(0, line.Length / 2);
    var secondHalf = line.Substring(line.Length / 2);

    var firstHalfLetters = firstHalf.Distinct();
    var secondHalfLetters = secondHalf.Distinct();
    
    var both = firstHalfLetters.Intersect(secondHalfLetters).Single();
    
    // Console.Write(both);

    if (both >= 97)
    {
        sum += both - 96;
        // Console.WriteLine(((int)both - 96));
    }
    else
    {
        sum += both - 38;
        // Console.WriteLine(((int)both - 38));
    }
}

//Part 1 solution
Console.WriteLine(sum);

sum = 0;
for (var i = 0; i < lines.Count; i += 3)
{
    var group = lines.Skip(i).Take(3).ToList();
    
    var commonChar = group[0].Intersect(group[1]).Intersect(group[2]).Single();
    
    if (commonChar >= 97)
    {
        sum += commonChar - 96;
        // Console.WriteLine(((int)commonChar - 96));
    }
    else
    {
        sum += commonChar - 38;
        // Console.WriteLine(((int)commonChar - 38));
    }
}

//Part 2 solution
Console.WriteLine(sum);