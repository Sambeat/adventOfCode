// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2023, 6);

var lines = today.InputLinesTrimmed;

var times = lines[0].Split(':')[1].Trim().Split(' ').Where(s => string.Empty != s).Select(int.Parse).ToList();
var distances = lines[1].Split(':')[1].Trim().Split(' ').Where(s => string.Empty != s).Select(int.Parse).ToList();

var mult = 1;
for (var t = 0; t < times.Count; t++)
{
  var sum = 0;
  var time = times[t];
  for (int i = 0; i < time; i++)
  {
    if (i * (time - i) > distances[t])
    {
      sum ++;
    }
  }
  
  mult *= sum;
}

Console.WriteLine(mult);


var bigTime = long.Parse(times.Select(t => t.ToString()).Aggregate((a,b) => a + b));
var bigDistance = long.Parse(distances.Select(d => d.ToString()).Aggregate((a,b) => a + b));

var bigsum = 0l;
for (long i = 0; i < bigTime; i++)
{
  if (i * (bigTime - i) > bigDistance)
  {
    bigsum ++;
  }
}


Console.WriteLine(bigsum);