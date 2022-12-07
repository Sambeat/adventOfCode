namespace AdventOfCode.Day7;

public class AoCFile
{
    public static long SizeUnder100K = 0;
    
    public static List<long> DeleteCandidates = new List<long>();
    public List<AoCFile> SubFiles { get; set; }

    public string Name { get; }
    public long Size { get; set; }

    public AoCFile? Parent { get; set; }

    public AoCFile(string name, long size, AoCFile? parent)
    {
        Name = name;
        Size = size;
        Parent = parent;
        SubFiles = new List<AoCFile>();
    }

    public long DirectorySize()
    {
        if (SubFiles.Any())
        {
            return SubFiles.Sum(x => x.DirectorySize());
        }
        else
        {
            return Size;
        }
    }
    
    public long TotalSizeUnder1000000()
    {
        SizeUnder100K = 0;
        Traverse100k();

        return SizeUnder100K;
    }

    public long FindSmallest(long minimum)
    {
        DeleteCandidates.Clear();
        TraverseSmallest(minimum);

        return DeleteCandidates.Min();
    }

    private void TraverseSmallest(long minimum)
    {
        if (Size == 0 && DirectorySize() > minimum)
        {
            DeleteCandidates.Add(DirectorySize());
        }

        foreach (var subFile in SubFiles)
        {
            subFile.TraverseSmallest(minimum);
        }
    }

    private void Traverse100k()
    {
        if (Size == 0 && DirectorySize() <= 100000)
        {
            SizeUnder100K += DirectorySize();
        }

        foreach (var subFile in SubFiles)
        {
            subFile.Traverse100k();
        }
    }
}