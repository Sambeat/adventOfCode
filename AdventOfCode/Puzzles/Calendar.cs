namespace AdventOfCode.Puzzles;

public static class Calendar
{
    private static AdventOfCodeClient Client => AdventOfCodeClient.DefaultInstance;
    
    public static async Task<Puzzle> OpenPuzzleAsync(int year, int day)
    {
        var input = await Client.GetInputAsync(year, day);

        return new Puzzle(input);
    }
}