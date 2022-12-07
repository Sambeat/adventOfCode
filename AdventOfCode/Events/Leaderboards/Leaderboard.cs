namespace AdventOfCode.Events.Leaderboards;

public class Leaderboard
{
    public string Event { get; set; }
    public Dictionary<string, Member> Members { get; set; }
}