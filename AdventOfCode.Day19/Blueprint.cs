namespace AdventOfCode.Day19;

public class Blueprint
{
    public int Id { get; set; }
    public int OreRobotCost { get; set; }
    public int ClayRobotCost { get; set; }
    public (int oreCost, int clayCost) ObsidianRobotCost { get; set; }
    public (int oreCost, int obsidianCost) GeodeRobotCost { get; set; }
}