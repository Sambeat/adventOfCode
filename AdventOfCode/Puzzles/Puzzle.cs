namespace AdventOfCode.Puzzles;

public class Puzzle
{
    private string Input { get; }

    internal Puzzle(string input)
    {
        Input = input;
    }
    
    public string RawInput => Input;
    public string[] InputLines => Input.Split(Environment.NewLine);
}