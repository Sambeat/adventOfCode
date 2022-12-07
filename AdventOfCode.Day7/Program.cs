// See https://aka.ms/new-console-template for more information

using AdventOfCode.Day7;
using AdventOfCode.Puzzles;

Console.WriteLine("Hello, World!");

var today = await Calendar.OpenPuzzleAsync(2022, 7);

today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var root = new AoCFile("/", 0, null);
var currentDirectory = root;
for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    if (line == "$ cd /")
    {
        continue;
    }
    
    if (line.Substring(0, 4) == "$ cd")
    {
        if (line.Split(" ")[2] == "..")
        {
            currentDirectory = currentDirectory.Parent;
        }
        else
        {
            currentDirectory = currentDirectory.SubFiles.First(x => x.Name == line.Split(" ")[2]);
        }
    }

    if (line.Substring(0, 4) == "$ ls")
    {
        i++;

        while(i < lines.Length && !lines[i].Contains("$"))
        {

            var fileParts = lines[i].Split(" ");

            if (fileParts[0] == "dir")
            {
                currentDirectory.SubFiles.Add(new AoCFile(fileParts[1], 0, currentDirectory));
            }
            else
            {
                currentDirectory.SubFiles.Add(new AoCFile(fileParts[1], long.Parse(fileParts[0]), currentDirectory));
            }

            i++;
        }

        //Rewind by one to let the outer foor loop start from where it expects
        i--;
    }
}

Console.WriteLine(root.TotalSizeUnder1000000());

var availableSpace = 70000000 - root.DirectorySize();
Console.WriteLine($"Available space: {availableSpace}");

var spaceNeeded = 30000000 - availableSpace;
Console.WriteLine($"Space Needed: {spaceNeeded}");

Console.WriteLine($"Found: {root.FindSmallest(spaceNeeded)}");