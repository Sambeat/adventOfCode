// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2023, 3);

var matrix = today.InputMatrix;

today.PrintLines();

var currentNumber = "";
var isCurrentNumberValid = false;
var sum = 0;
var currentSymbol = "";
var currentSymbolRow = 0;
var currentSymbolCol = 0;

var gearValues = new Dictionary<string, List<string>>();

for(var x = 0; x < matrix.Length; x++)
{
    for(var y = 0; y < matrix[x].Length; y++)
    {
        var value = matrix[x][y];
        if (char.IsDigit(value))
        {
            currentNumber += value;

            if (!isCurrentNumberValid)
            {
                (isCurrentNumberValid, var matchingValue, var matchingRow, var matchingCol) = HasAdjacent(new List<char> {'!','#','^','*','/','@','=','$','&','%','+','-'}.Contains, x, y, matrix);
                if (isCurrentNumberValid)
                {
                    currentSymbol = matchingValue;
                    currentSymbolRow = matchingRow!.Value;
                    currentSymbolCol = matchingCol!.Value;
                }
            }
        }
        else
        {
            if (isCurrentNumberValid)
            {
                sum += int.Parse(currentNumber);
                
                if (currentSymbol == "*")
                {
                    var gearValue = currentNumber;
                    if (gearValues.ContainsKey($"{currentSymbolRow},{currentSymbolCol}"))
                    {
                        gearValues[$"{currentSymbolRow},{currentSymbolCol}"].Add(gearValue);
                    }
                    else
                    {
                        gearValues.Add($"{currentSymbolRow},{currentSymbolCol}", new List<string> { gearValue });
                    }
                }
            }
            currentNumber = "";
            currentSymbol = "";
            currentSymbolRow = 0;
            currentSymbolCol = 0;
            isCurrentNumberValid = false;
        }
    }
}
Console.WriteLine(sum);

var gearRatioSum = 0;
foreach (var gear in gearValues)
{
    if(gear.Value.Count == 2)
    {
        gearRatioSum += int.Parse(gear.Value[0]) * int.Parse(gear.Value[1]);
    }
}
Console.WriteLine(gearRatioSum);

(bool isMatching, string? matchingValue, int? matchingRow, int? matchingCol) HasAdjacent(Func<char, bool> adjacentValueEvaluator, int row, int col, char[][] matrix)
{
    var hasAdjacent = false;

    for (var x = row - 1; x <= row + 1; x++)
    {
        for (var y = col - 1; y <= col + 1; y++)
        {
            if (x >= 0 && y >= 0 && x < matrix.Length && y < matrix[x].Length && !(x == row && y == col))
            {
                var adjacentValue = matrix[x][y];
                
                if (adjacentValueEvaluator.Invoke(adjacentValue))
                {
                    hasAdjacent = true;
                    var matchingValue = adjacentValue.ToString();
                    
                    return (hasAdjacent, matchingValue, x, y);
                }
            }
        }
    }
    
    return (hasAdjacent, null, null, null);
}