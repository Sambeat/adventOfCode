namespace AdventOfCode.Events.Puzzles;

public class Puzzle
{
    private string Input { get; }

    internal Puzzle(string input)
    {
        Input = input;
    }
    
    public string RawInput => Input;
    public string[] InputLines => Input.Split('\n');
    
    public string[] InputLinesTrimmed => InputLines.Where(s => string.Empty != s.Trim()).ToArray();

    public char[][] InputMatrix => InputMatrixFunc((c) => c);
    public int[][] InputMatrixAsInts => InputMatrixFunc((c) => int.Parse(c.ToString()));
    
    private T[][] InputMatrixFunc<T>(Func<char, T> parserFunc)
    {
        var matrix = new T[InputLinesTrimmed.Length][];
        for (var i = 0; i < InputLinesTrimmed.Length; i++)
        {
            if (matrix[i] == null)
            {
                matrix[i] = new T[InputLinesTrimmed[i].Length];
            }
            for (var j = 0; j < InputLinesTrimmed[i].Length; j++)
            {
                matrix[i][j] = parserFunc(InputLinesTrimmed[i][j]);
            }
        }

        return matrix;
    }
    
    public void PrintMatrix()
    {
        foreach (var line in InputMatrix)
        {
            Console.WriteLine(string.Join("", line));
        }
    }
    
    public void PrintLines()
    {
        foreach (var line in InputLines)
        {
            Console.WriteLine(line);
        }
    }
    
}