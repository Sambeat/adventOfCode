using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AdventOfCode.Events.Leaderboards;

public class Star
{
    [JsonProperty("star_index")]
    public int StarIndex { get; set; }
    [JsonProperty("get_star_ts")]
    public long GetStarTimestamp { get; set; }
}