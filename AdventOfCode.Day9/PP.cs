namespace AdventOfCode.Day9;

public class PP
{
    private static int idxCounter = 0;
    
    public int index { get; set; }
    public int x { get; set; }
    public int y { get; set; }

    public PP(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;

        this.index = idxCounter;
        idxCounter++;
    }

    protected bool Equals(PP other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PP)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    private sealed class XYEqualityComparer : IEqualityComparer<PP>
    {
        public bool Equals(PP x, PP y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.x == y.x && x.y == y.y;
        }

        public int GetHashCode(PP obj)
        {
            return HashCode.Combine(obj.x, obj.y);
        }
    }

    public static IEqualityComparer<PP> XYComparer { get; } = new XYEqualityComparer();
}