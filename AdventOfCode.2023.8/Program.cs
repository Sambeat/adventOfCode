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

    var destinations = (parts[1].Substring(2, 3), parts[1].Substring(7, 3));
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

var nodes = new Dictionary<string, Node>(mappings.Count);

foreach (var key in mappings.Keys)
{
    nodes.Add(key, new Node() { Label = key });
}

foreach (var node in nodes.Values)
{
    node.Nodes.Add('L', nodes[mappings[node.Label].Item1]);
    node.Left = nodes[mappings[node.Label].Item1];
    node.Nodes.Add('R', nodes[mappings[node.Label].Item2]);
    node.Right = nodes[mappings[node.Label].Item2];

}

var currentLocations = lines.Where(line => line.Take(3).EndsWith(new[] { 'A' }))
    .Select(line => new string(line.Take(3).ToArray())).ToList();
var currentNodes = currentLocations.Select(cl => nodes[cl]).ToArray();
var megaStepCount = 0L;

var stepsCounts = new int[currentLocations.Count];
Array.Fill(stepsCounts, 0);

var cycleCounts = new long[currentLocations.Count];
Array.Fill(cycleCounts, 0L);

var founds = 0;
while (founds != currentNodes.Length)
{
    // Parallel.For(0, currentLocations.Count, (i) =>
    // {
    //     currentLocations[i] = instructions[stepCount % instructions.Length] switch
    //     {
    //         'L' => mappings[currentLocations[i]].Item1,
    //         'R' => mappings[currentLocations[i]].Item2,
    //         _ => throw new ArgumentOutOfRangeException()
    //     };
    //
    //     stepsCounts[i]++;
    //     
    //     if (currentLocations[i] == "FPZ" || currentLocations[i] == "SKZ" || currentLocations[i] == "ZZZ" || currentLocations[i] == "STZ" || currentLocations[i] == "MKZ" || currentLocations[i] == "CVZ")
    //     {
    //         Console.WriteLine($"Step {stepsCounts[i]} - {currentLocations[i]}");
    //     }
    //     
    //     if (currentLocations[i].EndsWith(new[] { 'Z' }) && cycleCounts[i] == 0)
    //     {
    //         cycleCounts[i] = stepsCounts[i];
    //     }
    //
    // });


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
    //
    // megaStepCount++;
    //
    // if (megaStepCount % 100000 == 0)
    // {
    //     Console.WriteLine(megaStepCount);
    //     
    // }


    for (var i = 0; i < currentNodes.Length; i++)
    {
        // currentNodes[i] = currentNodes[i].Nodes[instructions[stepCount % instructions.Length]];
        
        currentNodes[i] = instructions[(int) megaStepCount % instructions.Length] switch
        {
            'L' => currentNodes[i].Left,
            'R' => currentNodes[i].Right,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (currentNodes[i].Label[2] == 'Z' && cycleCounts[i] == 0)
        {
            cycleCounts[i] = megaStepCount + 1;
            Console.WriteLine($"Found {i} = {megaStepCount + 1}");
            founds++;
        }
    }

    megaStepCount++;

    if (megaStepCount % 100000000 == 0)
    {
        Console.WriteLine(megaStepCount);
    }
}

// Parallel.For(0, currentLocations.Count, (i) =>
// {
//     while (cycleCounts[i] == 0)
//     {
//         currentNodes[i] = currentNodes[i].Nodes[instructions[stepCount % instructions.Length]];
//         stepsCounts[i]++;
//         if (currentNodes[i].Label[2] == 'Z' && cycleCounts[i] == 0)
//         {
//             cycleCounts[i] = stepsCounts[i];
//             Console.WriteLine($"Found {i} = {stepsCounts[i]}");
//         }
//         
//         if (stepsCounts[i] % 10000000 == 0)
//      {
//          Console.WriteLine($"{i}: {stepsCounts[i]}");
//      }
//     }
// });

Console.WriteLine(MathNet.Numerics.Euclid.LeastCommonMultiple(cycleCounts));

class Node
{
    public string Label { get; set; }

    public Dictionary<char, Node> Nodes { get; set; }

    public Node Left { get; set; }
    public Node Right { get; set; }

    public Node()
    {
        Nodes = new Dictionary<char, Node>(2);
    }
}