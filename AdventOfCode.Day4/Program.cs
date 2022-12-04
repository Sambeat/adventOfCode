// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;

var day3 = await Calendar.OpenPuzzleAsync(2022, 4);

var lines = day3.InputLines.Where(s => s != string.Empty).ToList();

// lines.ForEach(Console.WriteLine);

var sum = 0;
foreach (var line in lines)
{
    var assignments = line.Split(',');

    var ranges1 = assignments[0].Split('-');

    var range1 = Enumerable.Range(int.Parse(ranges1[0]), int.Parse(ranges1[1]) - int.Parse(ranges1[0]) + 1).ToList();
    
    var ranges2 = assignments[1].Split('-');
    var range2 = Enumerable.Range(int.Parse(ranges2[0]), int.Parse(ranges2[1]) - int.Parse(ranges2[0]) + 1).ToList();

    Console.WriteLine(range1.Count());
    Console.WriteLine(range2.Count());
    Console.WriteLine(range1.Intersect(range2).Count());
    Console.WriteLine(range2.Intersect(range1).Count());
    
    
    // Part 1 answer
    if (!range1.Except(range2).Any() || !range2.Except(range1).Any())
    //Part 2 answer    
    //if (range1.Intersect(range2).Count() >= 1)
    {
        sum++;
    }
}

Console.WriteLine(sum);