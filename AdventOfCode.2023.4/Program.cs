// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2023, 4);

var lines = today.InputLinesTrimmed;

var pointSum = 0;
var cards = new int[lines.Length];
Array.Fill(cards, 1);
for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    var split = line.Split(":");
    var numSplit = split[1].Split("|");
    
    var myNums = numSplit[0].Trim().Split(" ").Where(n => string.Empty != n).Select(int.Parse).ToArray();
    var otherNums = numSplit[1].Trim().Split(" ").Where(n => string.Empty != n).Select(int.Parse).ToArray();
    
    var matches = myNums.Count(n => otherNums.Contains(n));
    
    if (matches > 0)
    {
        pointSum += (int)Math.Pow(2, matches - 1);
    }

    for (int c = 1; c <= matches; c++)
    {
        if (c+i < cards.Length)
        {
            cards[c+i] += cards[i];
        }
    }
}

Console.WriteLine(pointSum);

Console.WriteLine(cards.Sum());