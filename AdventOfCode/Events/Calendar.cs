using AdventOfCode.Events.Leaderboards;
using AdventOfCode.Events.Puzzles;

namespace AdventOfCode.Events;

public static class Calendar
{
    private static AdventOfCodeClient Client => AdventOfCodeClient.DefaultInstance;
    
    public static async Task<Puzzle> OpenPuzzleAsync(int year, int day)
    {
        var input = await Client.GetInputAsync(year, day);

        return new Puzzle(input);
    }

    public static async Task<Leaderboard?> GetMercantileLeaderboard(int year)
    {
        var leaderboard = await Client.GetLeaderboardAsync(year);

        return leaderboard;
    }
}