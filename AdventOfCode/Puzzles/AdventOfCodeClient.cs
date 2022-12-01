using System.Diagnostics;
using AdventOfCode.Configs;

namespace AdventOfCode.Puzzles;

public class AdventOfCodeClient
{
    private HttpClient HttpClient { get; }

    internal static AdventOfCodeClient DefaultInstance => new AdventOfCodeClient(ConfigManager.AdventOfCodeConfig);

    private AdventOfCodeClient(AdventOfCodeConfig adventOfCodeConfig)
    {
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(adventOfCodeConfig.BaseUrl ?? "https://adventofcode.com")
        };
        HttpClient.DefaultRequestHeaders.Add("Cookie", adventOfCodeConfig.SessionConfig?.Cookie);
    }

    internal async Task<string> GetInputAsync(int year, int day)
    {
        var response = await HttpClient.GetAsync($"/{year}/day/{day}/input");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}