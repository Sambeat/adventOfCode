using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AdventOfCode.Events.Leaderboards;

public class Day
{
    [JsonProperty("1")]
    public Star Part1 { get; set; }

    [JsonProperty("2")]
    public Star Part2 { get; set; }
}