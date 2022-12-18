namespace AdventOfCode.Day18;

public class Point3D
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    protected bool Equals(Point3D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Point3D)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((X+10)^2, (Y+30)^3, (Z+50)^4);
    }
}