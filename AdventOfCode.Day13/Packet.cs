namespace AdventOfCode.Day13;

public class Packet
{
    public int Value { get; set; }
    public List<Packet> Children { get; set; } = new List<Packet>();
    public Packet Parent { get; set; }

    public string Type { get; set; }

    // public bool CompareOrder(Packet otherPacket)
    // {
    //     
    // }
}