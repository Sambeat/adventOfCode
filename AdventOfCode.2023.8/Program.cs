// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq;

var today = await Calendar.OpenPuzzleAsync(2023, 8);

var lines = today.InputLinesTrimmed;

var instructions = lines[0];

var mappings = new Dictionary<string, (string, string)>();
for (var l = 1; l < lines.Length; l++)
{
    var parts = lines[l].Split("=");

    var destinations = (parts[1].Substring(2, 3),parts[1].Substring(7,3));
    mappings.Add(parts[0].Trim(), destinations);
}

var currentLocation = "AAA";
var stepCount = 0;
while (currentLocation != "ZZZ")
{
    currentLocation = instructions[stepCount % instructions.Length] switch
    {
        'L' => mappings[currentLocation].Item1,
        'R' => mappings[currentLocation].Item2,
        _ => throw new ArgumentOutOfRangeException()
    };

    stepCount++;
}

Console.WriteLine(stepCount);


var currentLocations = lines.Where(line => line.Take(3).EndsWith(new []{'A'})).Select(line => new string(line.Take(3).ToArray())).ToList();
var megaStepCount = 0;

var stepsCounts = new int[currentLocations.Count];
Array.Fill(stepsCounts, 0);

var cycleCounts = new int[currentLocations.Count];
Array.Fill(cycleCounts, 0);
while (cycleCounts.Any(cl => cl == 0))
{
    Parallel.For(0, currentLocations.Count, (i) =>
    {
        currentLocations[i] = instructions[stepCount % instructions.Length] switch
        {
            'L' => mappings[currentLocations[i]].Item1,
            'R' => mappings[currentLocations[i]].Item2,
            _ => throw new ArgumentOutOfRangeException()
        };

        stepsCounts[i]++;
        
        if (currentLocations[i] == "FPZ" || currentLocations[i] == "SKZ" || currentLocations[i] == "ZZZ" || currentLocations[i] == "STZ" || currentLocations[i] == "MKZ" || currentLocations[i] == "CVZ")
        {
            Console.WriteLine($"Step {stepsCounts[i]} - {currentLocations[i]}");
        }
        
        if (currentLocations[i].EndsWith(new[] { 'Z' }) && cycleCounts[i] == 0)
        {
            cycleCounts[i] = stepsCounts[i];
        }

    });
    // for (var i = 0; i < currentLocations.Count; i++)
    // {
    //     currentLocations[i] = instructions[stepCount % instructions.Length] switch
    //     {
    //         'L' => mappings[currentLocations[i]].Item1,
    //         'R' => mappings[currentLocations[i]].Item2,
    //         _ => throw new ArgumentOutOfRangeException()
    //     };
    //     
    //     if (currentLocations[i].EndsWith(new []{'Z'}) && cycleCounts[i] == 0)
    //     {
    //         cycleCounts[i] = megaStepCount;
    //     }
    // }
    
    // megaStepCount++;
}

var lcm = 1;
foreach (var cycleCount in cycleCounts)
{
    lcm = lcm * cycleCount;
}

Console.WriteLine(lcm);