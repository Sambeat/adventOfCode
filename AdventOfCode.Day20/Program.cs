using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 20);

// today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();


var initialEncryptedFile = lines.Select((l, i) => ((long)i, long.Parse(l) * 811589153)).ToList();
var fileCopy = initialEncryptedFile.ToList();

for (var mix = 0; mix < 10; mix++)
{
    for (var i = 0; i < initialEncryptedFile.Count; i++)
    {
        var currentValue = initialEncryptedFile[i];
        var currentIndex = fileCopy.IndexOf(currentValue);

        fileCopy.Remove(currentValue);

        var insertionIndex =
            (int)((currentIndex + currentValue.Item2 +
                   fileCopy.Count * (Math.Abs(currentValue.Item2 / fileCopy.Count) + 1)) % fileCopy.Count);
        fileCopy.Insert(insertionIndex, currentValue);
    }
}

var indexOf0 = fileCopy.IndexOf(initialEncryptedFile.Single(l => l.Item2 == 0));
var indexOfFirst = (indexOf0 + 1000) % fileCopy.Count;
var indexOfSecond = (indexOf0 + 2000) % fileCopy.Count;
var indexOfThird = (indexOf0 + 3000) % fileCopy.Count;

Console.WriteLine($"{fileCopy[indexOfFirst]}, {fileCopy[indexOfSecond]}, {fileCopy[indexOfThird]}");

var result = fileCopy[indexOfFirst].Item2 + fileCopy[indexOfSecond].Item2 + fileCopy[indexOfThird].Item2;

Console.WriteLine(result);