using AdventOfCode.Events;

Console.WriteLine("Hello, World!");

var today = await Calendar.OpenPuzzleAsync(2023, 1);

var caloriesLines = today.InputLinesTrimmed;


var sum = 0;
foreach (var line in caloriesLines)
{
    var calibrationValue = string.Empty;
    var digitsFoundCount = 0;
    var lastDigitFound = string.Empty;
    foreach (var c in line)
    {
        if (char.IsDigit(c))
        {
            if (digitsFoundCount == 0)
            {
                calibrationValue += c;
            }

            lastDigitFound = c.ToString();
            digitsFoundCount++;
        } else {}
    }

    calibrationValue += lastDigitFound;

    sum += int.Parse(calibrationValue);
}

Console.WriteLine(sum);



var secondSum = 0;
foreach (var line in caloriesLines)
{
    
    var firstDigit = string.Empty;
    var firstDigitIndex = 100;
    var lastDigit = string.Empty;
    var lastDigitIndex = -1;
    
    var numerals = new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    var digits = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    foreach (var num in numerals)
    {
        var index = line.IndexOf(num, StringComparison.Ordinal);
        if (index != -1 && index < firstDigitIndex)
        {
            firstDigit = digits[numerals.IndexOf(num)];
            firstDigitIndex = index;
        }
        
        var lastIndex = line.LastIndexOf(num, StringComparison.Ordinal);
        if (lastIndex != -1 && lastIndex > lastDigitIndex)
        {
            lastDigit = digits[numerals.IndexOf(num)];
            lastDigitIndex = lastIndex;
        }
    }

    foreach (var digit in digits)
    {
        var index = line.IndexOf(digit, StringComparison.Ordinal);
        if (index != -1 && index < firstDigitIndex)
        {
            firstDigit = digit;
            firstDigitIndex = index;
        }
        
        var lastIndex = line.LastIndexOf(digit, StringComparison.Ordinal);
        if (lastIndex != -1 && lastIndex > lastDigitIndex)
        {
            lastDigit = digit;
            lastDigitIndex = lastIndex;
        }
    }
    
    var calibValue = firstDigit + lastDigit;
    secondSum += int.Parse(calibValue);
}


Console.WriteLine("Second Sum: " + secondSum);