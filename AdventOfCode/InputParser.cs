using Microsoft.Extensions.Configuration;

namespace AdventOfCode;

public class InputParser
{
  public HttpClient HttpClient { get; set; }
  public InputParser()
  {
    var configBuilder = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    
    IConfigurationRoot configurationRoot = configBuilder.Build();

    SessionConfig sessionConfig = new();
    configurationRoot.GetSection(nameof(SessionConfig))
      .Bind(sessionConfig);
    
    HttpClient = new HttpClient();
    HttpClient.DefaultRequestHeaders.Add("Cookie", $"session={sessionConfig.Cookie}");
  }
  
  public async Task<string[]> ReadLinesAsync(int year, int day)
  {
    var response = await HttpClient.GetAsync($"https://adventofcode.com/{year}/day/{day}/input");
    var inputString = await response.Content.ReadAsStringAsync();
    return inputString.Split(Environment.NewLine);
  }
}