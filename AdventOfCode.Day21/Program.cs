using AdventOfCode.Day21;
using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 21);

// today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

Monkey root = null;

var orphanMonkey = new Dictionary<string, Monkey>();
for (var l = 0; l < lines.Length; l++)
{
    var line = lines[l];

    var parts = line.Split(" ");

    Monkey lineMonkey = null;
    if (parts.Length == 4)
    {
        lineMonkey = new Monkey
        {
            Name = new String(parts[0].SkipLast(1).ToArray()),
            LeftName = parts[1],
            Operator = parts[2],
            RightName = parts[3],
        };
    }
    else
    {
        lineMonkey = new Monkey
        {
            Name = new String(parts[0].SkipLast(1).ToArray()),
            Value = long.Parse(parts[1])
        };
    }

    if (root == null && lineMonkey.Name == "root")
    {
        lineMonkey.Operator = "=";
        root = lineMonkey;
    }
    else
    {
        if (lineMonkey.Name == "humn")
        {
            lineMonkey.Value = 1;
            lineMonkey.Operator = "t";
        }
        orphanMonkey.Add(lineMonkey.Name, lineMonkey);
    }
}

foreach (var monkey in orphanMonkey)
{
    if (monkey.Value.Value == null)
    {
        monkey.Value.Left = orphanMonkey[monkey.Value.LeftName];
        monkey.Value.Right = orphanMonkey[monkey.Value.RightName];
    }
}

root.Left = orphanMonkey[root.LeftName];
root.Right = orphanMonkey[root.RightName];

// var result = root.Compute();
//
// Console.WriteLine(result);

// Console.WriteLine(root.Print());

root.ResolveFor("t");

var result = root.Compute();

Console.WriteLine(result);