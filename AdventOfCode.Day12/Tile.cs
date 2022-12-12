namespace AdventOfCode.Day12;

public class Tile
{
    public int Cost { get; set; }
    public int FullCost { get; set; }
    public int Height { get; set; }
    public Tile Parent { get; set; }

    public int I { get; set; }
    public int J { get; set; }

    protected bool Equals(Tile other)
    {
        return I == other.I && J == other.J;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Tile)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(I, J);
    }
}