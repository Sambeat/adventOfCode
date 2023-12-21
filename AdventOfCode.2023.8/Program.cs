// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2023, 7);

var lines = today.InputLinesTrimmed;

var instructions = lines[0];

var mappings = new Dictionary<string, (string, string)>();
for (var l = 1; l < lines.Length; l++)
{
    var parts = lines[l].Split("=");

    var destinations = (parts[1].Substring(2, 3),parts[1].Substring(7,3));
    mappings.Add(parts[0].Trim(), destinations);
}