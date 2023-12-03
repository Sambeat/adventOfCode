namespace AdventOfCode.Day19;

public class Game
{
    public int OreRobotsQty { get; set; }
    public int ClayRobotsQty { get; set; }
    public int ObsidianRobotsQty { get; set; }
    public int GeodeRobotsQty { get; set; }

    public int OreRobotsQueue { get; set; }
    public int ClayRobotsQueue { get; set; }
    public int ObsidianRobotsQueue { get; set; }
    public int GeodeRobotsQueue { get; set; }

    public int Ore { get; set; }
    public int OreQueue { get; set; }
    public int Clay { get; set; }
    public int ClayQueue { get; set; }

    public int Obsidian { get; set; }
    public int ObsidianQueue { get; set; }

    public int Geode { get; set; }
    public int GeodeQueue { get; set; }


    public void TickOre()
    {
    }

    public void Tick(Blueprint blueprint, string? order)
    {
        OreQueue += OreRobotsQty;
        ClayQueue += ClayRobotsQty;
        ObsidianQueue += ObsidianRobotsQty;
        GeodeQueue += GeodeRobotsQty;

        if (order == "Geode")
        {
            var newRobotsQty = 1;
            // var newRobotsQty = Math.Min(Obsidian / blueprint.GeodeRobotCost.obsidianCost,
            // Ore / blueprint.GeodeRobotCost.oreCost);
            Obsidian -= newRobotsQty * blueprint.GeodeRobotCost.obsidianCost;
            Ore -= newRobotsQty * blueprint.GeodeRobotCost.oreCost;
            GeodeRobotsQueue += newRobotsQty;
        }

        if (order == "Obsidian")
        {
            var newRobotsQty = 1;
            // var newRobotsQty = Math.Min(Clay / blueprint.ObsidianRobotCost.clayCost,
            // Ore / blueprint.ObsidianRobotCost.oreCost);
            Clay -= newRobotsQty * blueprint.ObsidianRobotCost.clayCost;
            Ore -= newRobotsQty * blueprint.ObsidianRobotCost.oreCost;
            ObsidianRobotsQueue += newRobotsQty;
        }

        if (order == "Clay")
        {
            var newRobotsQty = 1;
            // var newRobotsQty = Ore / blueprint.ClayRobotCost;
            Ore -= newRobotsQty * blueprint.ClayRobotCost;
            ClayRobotsQueue += newRobotsQty;
        }

        if (order == "Ore")
        {
            var newRobotsQty = 1;
            // var newRobotsQty = Ore / blueprint.OreRobotCost;
            Ore -= newRobotsQty * blueprint.OreRobotCost;
            OreRobotsQueue += newRobotsQty;
        }

        OreRobotsQty += OreRobotsQueue;
        ClayRobotsQty += ClayRobotsQueue;
        ObsidianRobotsQty += ObsidianRobotsQueue;
        GeodeRobotsQty += GeodeRobotsQueue;

        OreRobotsQueue = 0;
        ClayRobotsQueue = 0;
        ObsidianRobotsQueue = 0;
        GeodeRobotsQueue = 0;

        Ore += OreQueue;
        Clay += ClayQueue;
        Obsidian += ObsidianQueue;
        Geode += GeodeQueue;

        OreQueue = 0;
        ClayQueue = 0;
        ObsidianQueue = 0;
        GeodeQueue = 0;
    }

    public List<string> PossibleOrders(Blueprint blueprint)
    {
        var orders = new List<string>();

        if (Obsidian >= blueprint.GeodeRobotCost.obsidianCost && Ore >= blueprint.GeodeRobotCost.oreCost)
        {
            orders.Add("Geode");
        } else 

        if (Clay >= blueprint.ObsidianRobotCost.clayCost && Ore >= blueprint.ObsidianRobotCost.oreCost)
        {
            orders.Add("Obsidian");
        } else

        if (Ore >= blueprint.ClayRobotCost)
        {
            orders.Add("Clay");
        }  

        if (Ore >= blueprint.OreRobotCost)
        {
            orders.Add("Ore");
        } 

        if (!orders.Contains("Geode"))
        {
            orders.Add("Do Nothing");
        }

        return orders;
    }

    protected bool Equals(Game other)
    {
        return OreRobotsQty == other.OreRobotsQty && ClayRobotsQty == other.ClayRobotsQty &&
               ObsidianRobotsQty == other.ObsidianRobotsQty && GeodeRobotsQty == other.GeodeRobotsQty &&
               Ore == other.Ore && Clay == other.Clay && Obsidian == other.Obsidian && Geode == other.Geode;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Game)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(234*OreRobotsQty, 123*ClayRobotsQty,214* ObsidianRobotsQty, 114*GeodeRobotsQty, 34*Ore, 17* Clay, 119* Obsidian,
            637*Geode);
    }
}