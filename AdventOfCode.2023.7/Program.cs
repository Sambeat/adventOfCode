// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2023, 7);

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").ToArray();

var pairs = lines.Select(l =>
{ 
    var parts = l.Split(' ');
    
    return (parts[0], parts[1]);
});

var types =  new Dictionary<string, StrengthSortedList>();

types.Add("5", new StrengthSortedList());
types.Add("4", new StrengthSortedList());
types.Add("fh", new StrengthSortedList());
types.Add("3", new StrengthSortedList());
types.Add("2p", new StrengthSortedList());
types.Add("1p", new StrengthSortedList());
types.Add("0",new StrengthSortedList());



var strengths = new List<string>{"A","K","Q","J","T","9","8","7","6","5","4","3","2"};
foreach (var pair in pairs)
{
    var counts = new int[strengths.Count];

    foreach (var c in pair.Item1)
    {
        if (c == 'J')
        {
            for (int j = 0; j < counts.Length; j++)
            {
                counts[j]++;
            }
        }
        else
        {
            counts[strengths.IndexOf(c.ToString())]++;
        }
    }

    switch (counts.Max())
    {
        case 5:
            types["5"].Add(pair.Item1, pair.Item2);
            break;
        case 4:
            types["4"].Add(pair.Item1, pair.Item2);
            break;
        case 3:
            if (counts.Count(c => c == 2) == 1 || counts.Count(c => c == 3) == 2)
            {
                types["fh"].Add(pair.Item1, pair.Item2);
            }
            else
            {
                types["3"].Add(pair.Item1, pair.Item2);
            }
            break;
        case 2:
            if (counts.Count(c => c == 2) == 2)
            {
                types["2p"].Add(pair.Item1, pair.Item2);
            }
            else
            {
                types["1p"].Add(pair.Item1, pair.Item2);
            }
            break;
        case 1:
            types["0"].Add(pair.Item1, pair.Item2);
            break;
    }
    
}

var totalHands = lines.Length;
var totalWinnings = 0l;

foreach(var type in types)
{
    foreach (var item in type.Value)
    {
        totalWinnings += long.Parse(item.Value) * totalHands--;
    }
}

Console.WriteLine(totalWinnings);

class StrengthSortedList : SortedList<string,string>
{
    public StrengthSortedList() : base(new StrengthComparer())
    {
    }
}

class StrengthComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        var strengths = new List<char>{'A','K','Q','T','9','8','7','6','5','4','3','2', 'J'};

        for (int i = 0; i < x.Length; i++)
        {
            var comparison = strengths.IndexOf(x[i]).CompareTo(strengths.IndexOf(y[i]));

             if (comparison != 0)
             {
                 return comparison;
             }
             
             if (i == x.Length - 1)
             {
                 return 1;
             }
        }
        
        return 1;
    }
}