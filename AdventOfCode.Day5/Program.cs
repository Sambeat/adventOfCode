// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;

Console.WriteLine("Hello, World!");


var day3 = await Calendar.OpenPuzzleAsync(2022, 5);

var lines = day3.InputLines.ToList();

// lines.ForEach(Console.WriteLine);

var matrix = new Stack<char>[9];
var i = 0;
foreach (var line in lines)
{
    if (line == string.Empty)
        break;

    for (var j = 0; j < line.Length; j++)
    {
        var c = line[j];
        if (c != ' ' && c != '[' && c != ']' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != '7' && c != '8' && c != '9' && c != '0')
        {
            Console.WriteLine($"{c}:{j}");
            var position = j / 4;

            if (matrix[position] == null)
            {
                matrix[position] = new Stack<char>();
            }

            matrix[position].Push(c);
        }
    }

    i++;
}

for (var s = 0; s < matrix.Length; s++)
{
    //Pop the column number which is wrongly inserted
    // matrix[s].Dequeue();
    
    matrix[s] = new Stack<char>(matrix[s]) ;
}

i++;

var commands = new List<Tuple<int, int, int>>();
for (; i < lines.Count; i++)
{
    if (lines[i] == String.Empty)
        break;
    
    Console.WriteLine(lines[i]);
    var parts = lines[i].Split(' ');
    
    commands.Add(new Tuple<int, int, int>(int.Parse(parts[1]), int.Parse(parts[3]) - 1 , int.Parse(parts[5]) - 1));
}

// Part 1
// var buffer = new Queue<char>();
// Part 2
var buffer = new Stack<char>();

foreach (var command in commands)
{
    Console.WriteLine($"{command.Item1} {command.Item2} {command.Item3}");
    for (var qty = command.Item1; qty > 0; qty--)
    {
        buffer.Push(matrix[command.Item2].Pop());
    }

    for (var bufferedQty = buffer.Count; bufferedQty > 0; bufferedQty--)
    {
        matrix[command.Item3].Push(buffer.Pop());
    }
}

Console.WriteLine("ANSWER");
foreach (var stack in matrix)
{
    Console.Write(stack.Pop());
}