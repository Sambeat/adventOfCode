// See https://aka.ms/new-console-template for more information

using AdventOfCode.Day13;
using AdventOfCode.Events;
using MoreLinq;

var today = await Calendar.OpenPuzzleAsync(2022, 13);

// today.PrintLines();

var lines = today.InputLines;
// var lines = File.ReadLines("test.txt").ToArray();

string first = string.Empty;
string second = string.Empty;
var sum = 0;
var currentNumberRead = "";

var packets = new List<Packet>();
for (var i = 0; i < lines.Length; i++)
{
    if (i % 3 == 0)
    {
        first = lines[i];
    }

    if (i % 3 == 1)
    {
        second = lines[i];
    }

    if (i % 3 == 2)
    {
        var (packetFirst, packetSecond) = comparePackets(first, second);
        var inOrder = comparePacketsDeep(packetFirst, packetSecond) < 0;

        var index = i / 3 + 1;
        Console.WriteLine($"{index}: {inOrder}");
        
        if (inOrder)
        {
            sum+= i / 3 + 1;
        }
        
        packets.Add(packetFirst);
        packets.Add(packetSecond);
    }
}

Console.WriteLine(sum);

var (dividerFirst, dividerSecond) = comparePackets("[[2]]", "[[6]]");
packets.Add(dividerFirst);
packets.Add(dividerSecond);

packets.Sort(comparePacketsDeep);

Console.WriteLine( (packets.IndexOf(dividerFirst) + 1) * (packets.IndexOf(dividerSecond) + 1));

(Packet first, Packet second) comparePackets(string first, string second)
{
    
    first = first.Substring(1, first.Length - 2);
    second = second.Substring(1, second.Length - 2);

    var rootPacket = new Packet();
    var currentPacket = rootPacket;

    for (int i = 0; i < first.Length; i++)
    {
        if (first[i] == '[')
        {
            currentPacket.Type = "a";
            var childPacket = new Packet { Parent = currentPacket };
            currentPacket.Children.Add(childPacket);
            currentPacket = childPacket;
        } else if (first[i] == ']')
        {
            currentPacket = currentPacket.Parent;
        }

        
        while (i < first.Length && char.IsDigit(first[i]))
        {
            currentNumberRead += first[i];
            i++;
        }

        if (currentNumberRead != string.Empty)
        {
            if (int.TryParse(currentNumberRead, out var number))
            {
                var numberPacket = new Packet { Type = "n", Value = number, Parent = currentPacket };
                currentPacket.Children.Add(numberPacket);
            }

            // ;)
            i--;
            currentNumberRead = "";
        }

    }
    
    var rootPacket2 = new Packet();
    currentPacket = rootPacket2;
    
    for (int i = 0; i < second.Length; i++)
    {
        if (second[i] == '[')
        {
            currentPacket.Type = "a";
            var childPacket = new Packet { Parent = currentPacket };
            currentPacket.Children.Add(childPacket);
            currentPacket = childPacket;
        } else if (second[i] == ']')
        {
            currentPacket = currentPacket.Parent;
        }

        while (i < second.Length && char.IsDigit(second[i]))
        {
            currentNumberRead += second[i];
            i++;
        }

        if (currentNumberRead != string.Empty)
        {
            if (int.TryParse(currentNumberRead, out var number))
            {
                var numberPacket = new Packet { Type = "n", Value = number, Parent = currentPacket };
                currentPacket.Children.Add(numberPacket);
            }

            // ;)
            i--;
            currentNumberRead = "";
        }
        
    }

    return (rootPacket, rootPacket2);
}


int comparePacketsDeep(Packet packetFirst, Packet packetSecond)
{
    if (packetFirst.Type == "n")
    {
        if (packetSecond.Type == "n")
        {
            Console.WriteLine($"Comparing {packetFirst.Value} and {packetSecond.Value}");

            return packetFirst.Value - packetSecond.Value;
        }
        else
        {
            return comparePacketsDeep(new Packet {Type = "a", Children = {packetFirst}}, packetSecond);
        }
    }
    else
    {
        if (packetSecond.Type == "n")
        {
            return comparePacketsDeep(packetFirst, new Packet{Type = "a", Children = {packetSecond}});
        }

        var zipped = packetFirst.Children.Zip(packetSecond.Children);
        foreach (var (first, second) in zipped)
        {
            var inOrder = comparePacketsDeep(first, second);

            if (inOrder < 0 || inOrder > 0)
                return inOrder;
        }
        
        Console.WriteLine($"Ran out of children to compare, returning {packetFirst.Children.Count - packetSecond.Children.Count}");
        return packetFirst.Children.Count - packetSecond.Children.Count;
    }

    return -1;
}