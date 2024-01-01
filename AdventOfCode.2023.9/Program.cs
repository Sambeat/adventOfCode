// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2023, 9);

var lines = today.InputLinesTrimmed;

var sum = 0;
var backsum = 0;
for (int i = 0; i < lines.Length; i++)
{
    var line = lines[i];

    var numbers = line.Split(" ").Select(int.Parse).ToList();

    var nextNumber = Differentiate(numbers);

    sum += nextNumber;

    backsum += DifferentiateBack(numbers);
}

Console.WriteLine(sum);

Console.WriteLine(backsum);

int Differentiate(List<int> series)
{
    if (series.All(i => i == 0))
    {
        return 0;
    }

    var nextSeries = new List<int>();

    for (int i = 1; i < series.Count; i++)
    {
        nextSeries.Add(series[i] - series[i-1]);
    }

    var slope = Differentiate(nextSeries);

    return series.Last() + slope;
}

int DifferentiateBack(List<int> series)
{
    if (series.All(i => i == 0))
    {
        return 0;
    }

    var nextSeries = new List<int>();

    for (int i = 1; i < series.Count; i++)
    {
        nextSeries.Add(series[i] - series[i-1]);
    }

    var slope = DifferentiateBack(nextSeries);

    return series.First() - slope;
}
