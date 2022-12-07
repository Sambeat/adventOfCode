// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq;

var leaderboard = await Calendar.GetMercantileLeaderboard(2022);

Console.WriteLine(leaderboard);


for (var i = DateTimeOffset.Now.Day; i > 0; i--)
{
    Dictionary<string, (DateTime, DateTime)> stars = new Dictionary<string, (DateTime, DateTime)>();

    leaderboard.Members.ForEach(m =>
    {
        var currentMember = m.Value;

        currentMember.CompletionDayLevel.TryGetValue(i.ToString(), out var day);

        if (day != null)
        {
            stars.Add(currentMember.Name,
                (UnixTimeStampToDateTime(day.Part1.GetStarTimestamp),
                    UnixTimeStampToDateTime(day.Part2.GetStarTimestamp)));
        }
    });

    var longestName = stars.Keys.Max(k => k.Length);
    var sortedMembers = stars.OrderBy(s => s.Value.Item2 ).ToList();
    Console.WriteLine($"{$"Day {i}",-30}{"Part 1",-25}{"Part 2",-25}");
    sortedMembers.ForEach(m =>
    {
        Console.WriteLine($"{$"{m.Key}:",-30}{$"{m.Value.Item1}",-25}{$"{m.Value.Item2}",-25}");
    });
}

Console.WriteLine("");


static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
{
    // Unix timestamp is seconds past epoch
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
    return dateTime;
}