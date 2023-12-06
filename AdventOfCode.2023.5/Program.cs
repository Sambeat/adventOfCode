// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq;

var today = await Calendar.OpenPuzzleAsync(2023, 5);

var lines = today.InputLinesTrimmed;

var seeds = new List<long>();
var seedToSoilMap = new Dictionary<(long,long), long>();
var soilToFertilizerMap = new Dictionary<(long,long), long>();
var fertilizerToWaterMap = new Dictionary<(long,long), long>();
var waterToLightMap = new Dictionary<(long,long), long>();
var lightToTemperatureMap = new Dictionary<(long,long), long>();
var temperatureToHumidityMap = new Dictionary<(long,long), long>();
var humidityToLocationMap = new Dictionary<(long,long), long>();

var currentMap = seedToSoilMap;
for (int i = 0; i < lines.Length; i++)
{
    var line = lines[i];

    if (i == 0)
    {
        seeds = line.Split(":")[1].Trim().Split(" ").Select(long.Parse).ToList();
        continue;
    }
    
    if (line == string.Empty)
    {
        continue;
    }
    
    if(line.Contains("seed-to-soil map"))
    {
        currentMap = seedToSoilMap;
        continue;
    }
    
    if(line.Contains("soil-to-fertilizer map"))
    {
        currentMap = soilToFertilizerMap;
        continue;
    }
    
    if(line.Contains("fertilizer-to-water map"))
    {
        currentMap = fertilizerToWaterMap;
        continue;
    }
    
    if(line.Contains("water-to-light map"))
    {
        currentMap = waterToLightMap;
        continue;
    }
    
    if(line.Contains("light-to-temperature map"))
    {
        currentMap = lightToTemperatureMap;
        continue;
    }
    
    if(line.Contains("temperature-to-humidity map"))
    {
        currentMap = temperatureToHumidityMap;
        continue;
    }
    
    if(line.Contains("humidity-to-location map"))
    {
        currentMap = humidityToLocationMap;
        continue;
    }
    
    var map = line.Split(" ").Select(long.Parse).ToArray();
    currentMap.Add((map[1], map[1] + map[2] - 1), map[0] - map[1]);
}

var locations = new List<long>();

foreach (var seed in seeds)
{
    
    var soil = seed + seedToSoilMap.Where(kvp => kvp.Key.Item1 <= seed && kvp.Key.Item2 >= seed).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    var fertilizer = soil + soilToFertilizerMap.Where(kvp => kvp.Key.Item1 <= soil && kvp.Key.Item2 >= soil).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    var water = fertilizer + fertilizerToWaterMap.Where(kvp => kvp.Key.Item1 <= fertilizer && kvp.Key.Item2 >= fertilizer).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    var light = water + waterToLightMap.Where(kvp => kvp.Key.Item1 <= water && kvp.Key.Item2 >= water).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    var temperature = light + lightToTemperatureMap.Where(kvp => kvp.Key.Item1 <= light && kvp.Key.Item2 >= light).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    var humidity = temperature + temperatureToHumidityMap.Where(kvp => kvp.Key.Item1 <= temperature && kvp.Key.Item2 >= temperature).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    var location = humidity + humidityToLocationMap.Where(kvp => kvp.Key.Item1 <= humidity && kvp.Key.Item2 >= humidity).Select(kvp => kvp.Value).DefaultIfEmpty(0).First();
    
    locations.Add(location);
    
}

Console.WriteLine(locations.Min());

var locations2 = new List<long>();
seeds.Chunk(2).ForEach(seedPair =>
{
    var soilPairs = seedToSoilMap.Where(kvp => kvp.Key.Item1 >= seedPair[0] || kvp.Key.Item2 <= seedPair[0] + seedPair[1] - 1).Select(kvp =>
        (Math.Max(kvp.Key.Item1, seedPair[0]), Math.Min(seedPair[0] + seedPair[1] - 1,kvp.Key.Item2))).ToArray();
    
    var fertilizerPairs = soilPairs.Select(sp => soilToFertilizerMap.Where(kvp => kvp.Key.Item1 >= sp.Item1 || kvp.Key.Item2 <= sp.Item2).Select(kvp =>
        (Math.Max(kvp.Key.Item1, sp.Item1), Math.Min(sp.Item2,kvp.Key.Item2))).ToArray()).SelectMany(sp => sp).ToArray();
    
    var waterPairs = fertilizerPairs.Select(sp => fertilizerToWaterMap.Where(kvp => kvp.Key.Item1 >= sp.Item1 || kvp.Key.Item2 <= sp.Item2).Select(kvp =>
        (Math.Max(kvp.Key.Item1, sp.Item1), Math.Min(sp.Item2,kvp.Key.Item2))).ToArray()).SelectMany(sp => sp).ToArray();
    
    var lightPairs = waterPairs.Select(sp => waterToLightMap.Where(kvp => kvp.Key.Item1 >= sp.Item1 || kvp.Key.Item2 <= sp.Item2).Select(kvp =>
        (Math.Max(kvp.Key.Item1, sp.Item1), Math.Min(sp.Item2,kvp.Key.Item2))).ToArray()).SelectMany(sp => sp).ToArray();
    
    var temperaturePairs = lightPairs.Select(sp => lightToTemperatureMap.Where(kvp => kvp.Key.Item1 >= sp.Item1 || kvp.Key.Item2 <= sp.Item2).Select(kvp =>
        (Math.Max(kvp.Key.Item1, sp.Item1), Math.Min(sp.Item2,kvp.Key.Item2))).ToArray()).SelectMany(sp => sp).ToArray();
    
    var humidityPairs = temperaturePairs.Select(sp => temperatureToHumidityMap.Where(kvp => kvp.Key.Item1 >= sp.Item1 || kvp.Key.Item2 <= sp.Item2).Select(kvp =>
        (Math.Max(kvp.Key.Item1, sp.Item1), Math.Min(sp.Item2,kvp.Key.Item2))).ToArray()).SelectMany(sp => sp).ToArray();
    
    var locationPairs = humidityPairs.Select(sp => humidityToLocationMap.Where(kvp => kvp.Key.Item1 >= sp.Item1 || kvp.Key.Item2 <= sp.Item2).Select(kvp =>
        (Math.Max(kvp.Key.Item1, sp.Item1), Math.Min(sp.Item2,kvp.Key.Item2))).ToArray()).SelectMany(sp => sp).ToArray();
    
    locations2.AddRange(locationPairs.Select(lp => lp.Item1));
    
});

Console.WriteLine(locations2.Min());