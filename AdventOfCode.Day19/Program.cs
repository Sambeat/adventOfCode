using AdventOfCode.Day19;
using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 19);

today.PrintLines();

// var lines = today.InputLinesTrimmed;
var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();

var blueprints = new List<Blueprint>(lines.Length);
for (var l = 0; l < lines.Length; l++)
{
    var line = lines[l];

    var parts = line.Split(" ");
    
    var blueprint = new Blueprint
    {
        Id = int.Parse(new string(parts[1].SkipLast(1).ToArray())),
        OreRobotCost = int.Parse(parts[6]),
        ClayRobotCost = int.Parse(parts[12]),
        ObsidianRobotCost = (int.Parse(parts[18]), int.Parse(parts[21])),
        GeodeRobotCost = (int.Parse(parts[27]), int.Parse(parts[30])),
    };

    blueprints.Add(blueprint);
}

var qualityLevels = blueprints.Select(bps => SimulateBlueprint(bps, 24)).ToList();

Console.WriteLine($"Quality levels: {string.Join(",",  qualityLevels)}");

var qualityLevelsSum = qualityLevels.Sum();

Console.WriteLine(qualityLevelsSum);


int SimulateBlueprint(Blueprint blueprint, int minutes)
{
    var games = new List<Game> { new()
    {
        OreRobotsQty = 1
    }};
    
    for (var m = 0; m < minutes; m++)
    {
        var tempGames = new List<Game>(games.Count);
        foreach (var game in games)
        {
            var orders = game.PossibleOrders(blueprint);
            var tickedGames = orders.Select(order =>
            {
                var tickedGame = new Game
                {
                    OreRobotsQty = game.OreRobotsQty,
                    ClayRobotsQty = game.ClayRobotsQty,
                    ObsidianRobotsQty = game.ObsidianRobotsQty,
                    GeodeRobotsQty = game.GeodeRobotsQty,
                    OreRobotsQueue = game.OreRobotsQueue,
                    ClayRobotsQueue = game.ClayRobotsQueue,
                    ObsidianRobotsQueue = game.ObsidianRobotsQueue,
                    GeodeRobotsQueue = game.GeodeRobotsQueue,
                    Ore = game.Ore,
                    OreQueue = game.OreQueue,
                    Clay = game.Clay,
                    ClayQueue = game.ClayQueue,
                    Obsidian = game.Obsidian,
                    ObsidianQueue = game.ObsidianQueue,
                    Geode = game.Geode,
                    GeodeQueue = game.GeodeQueue
                };
                
                tickedGame.Tick(blueprint, order);
                return tickedGame;
            }).ToList();
            
            tempGames.AddRange(tickedGames);   
        }

        if (tempGames.Any())
        {
            // var bestGames = tempGames.OrderByDescending(g => 1000 * g.Geode + 100 * g.Obsidian + 10 * g.Clay + g.Ore).Take(30000).ToList();
            
            games = tempGames;
        }
    }

    return games.Max(g => g.Geode * blueprint.Id);
}