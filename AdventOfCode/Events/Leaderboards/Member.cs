using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AdventOfCode.Events.Leaderboards;

public class Member : IComparable<Member>
{
    [JsonProperty("stars")]
    public int Stars { get; set; }
    [JsonProperty("GlobalScore")]
    public int GlobalScore { get; set; }

    [JsonProperty("completion_day_level")] public Dictionary<string, Day> CompletionDayLevel { get; set; }
    
    [JsonProperty("last_star_ts")]
    public long LastStarTs { get; set; }
    
    [JsonProperty("id")]
    public int id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("local_score")]
    public int LocalScore { get; set; }

    public int CompareTo(Member? other)
    {
        return LocalScore.CompareTo(other?.LocalScore);
    }
}