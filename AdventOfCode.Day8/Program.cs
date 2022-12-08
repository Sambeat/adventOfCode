// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 8);

today.PrintLines();

var matrix = today.InputMatrixAsInts;
//
// var lines = File.ReadLines("text.txt").Where(s => string.Empty != s).ToArray();
//
// var matrix = new int[lines.Length][];
// for (var i = 0; i < lines.Length; i++)
// {
//     if (matrix[i] == null)
//     {
//         matrix[i] = new int[lines[i].Length];
//     }
//     for (var j = 0; j < lines[i].Length; j++)
//     {
//         matrix[i][j] = int.Parse(lines[i][j].ToString());
//     }
// }

var visiblesQty = 0;
var maxScore = 0L;
for (var i = 0; i < matrix.Length; i++){

    for (var j = 0; j < matrix[i].Length; j++)
    {

        if (isVisible(i, j))
        {
            Console.WriteLine($"{i},{j}: {matrix[i][j]}");
            visiblesQty++;
        }

        maxScore = Math.Max(maxScore, scenicScore(i, j));
    }
}

Console.WriteLine(visiblesQty);
Console.WriteLine(maxScore);

bool isVisible(int i, int j)
{
    if (i == 0 || j == 0 || i == matrix.Length || j == matrix[i].Length)
    {
        return true;
    }

    var treeHeight = matrix[i][j];
    for (var i2 = i + 1; i2 <= matrix.Length; i2++)
    {
        if (i2 == matrix.Length)
        {
            return true;
        }
        else if (treeHeight <= matrix[i2][j])
        {
            break;
        }
    }    
    
    for (var i2 = i - 1; i2 >= -1; i2--)
    {
        if (i2 < 0)
        {
            return true;
        }
        else if (treeHeight <= matrix[i2][j])
        {
            break;
        }
    }
    
    for (var j2 = j + 1; j2 <= matrix[i].Length; j2++)
    {
        if (j2 == matrix[i].Length)
        {
            return true;
        }
        else if (treeHeight <= matrix[i][j2])
        {
            break;
        }
    }
    
    for (var j2 = j - 1; j2 >= -1; j2--)
    {
        if (j2 < 0)
        {
            return true;
        }
        else if (treeHeight <= matrix[i][j2])
        {
            break;
        }
    }

    return false;
}

long scenicScore(int i, int j)
{
    var treeHeight = matrix[i][j];
    var score = 0;
    var currentLineScore = 0;
    for (var i2 = i + 1; i2 <= matrix.Length; i2++)
    {
        if (i2 == matrix.Length)
        {
            break;
        }
        else if (treeHeight <= matrix[i2][j])
        {
            currentLineScore++;
            break;
        }

        currentLineScore++;
    }

    score += currentLineScore;

    currentLineScore = 0;
    for (var i2 = i - 1; i2 >= -1; i2--)
    {
        if (i2 < 0)
        {
            break;
        }
        else if (treeHeight <= matrix[i2][j])
        {
            currentLineScore++;
            break;
        }

        currentLineScore++;
    }
    score *= currentLineScore;

    currentLineScore = 0;
    for (var j2 = j + 1; j2 <= matrix[i].Length; j2++)
    {
        if (j2 == matrix[i].Length)
        {
            break;
        }
        else if (treeHeight <= matrix[i][j2])
        {
            currentLineScore++;
            break;
        }

        currentLineScore++;
    }
    score *= currentLineScore;

    currentLineScore = 0;
    for (var j2 = j - 1; j2 >= -1; j2--)
    {
        if (j2 < 0)
        {
            break;
        }
        else if (treeHeight <= matrix[i][j2])
        {
            currentLineScore++;
            break;
        }

        currentLineScore++;
    }
    score *= currentLineScore;

    return score;
}