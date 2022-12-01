using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Configs;

public static class ConfigManager
{
    private static readonly IConfigurationRoot Config = InitializeConfiguration();

    public static AdventOfCodeConfig AdventOfCodeConfig
    {
        get
        {
            AdventOfCodeConfig adventOfCodeConfig = new();
            Config.GetSection(nameof(AdventOfCodeConfig))
                .Bind(adventOfCodeConfig);

            return adventOfCodeConfig;
        }
    }

    static IConfigurationRoot InitializeConfiguration()
    {
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configurationRoot = configBuilder.Build();

        return configurationRoot;
    }
}