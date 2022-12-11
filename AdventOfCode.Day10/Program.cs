// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;


var today = await Calendar.OpenPuzzleAsync(2022, 10);

// today.PrintLines();

var lines = today.InputLinesTrimmed;
// var lines = File.ReadLines("test.txt").Where(s => string.Empty != s).ToArray();


var cycle = 1 ;
var register = 1;
var signal = 0;
var picture = "";
foreach (var line in lines)
{

    var command = line.Split(" ");

    if (command[0] == "noop")
    {
        draw();
        cycle++;
        checkSignal();
        continue;
    }
    else
    {
        draw();
        cycle ++;
        checkSignal();
        draw();
        cycle++;
        register += int.Parse(command[1]);
        checkSignal();

    }
}

Console.WriteLine(signal);

Console.WriteLine(picture);

void checkSignal()
{
    if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
    {
        signal += register * cycle;
        
        Console.WriteLine($"{register} * {cycle} = {signal}");
    }
}

void draw()
{
    if (cycle == 41 || cycle == 81 || cycle == 121 || cycle == 161 || cycle == 201)
    {
        picture += '\n';
    }
    
    if (register == cycle % 40 || register + 1 == cycle % 40 || register + 2 == cycle % 40)
    {
        picture += "#";
    }
    else
    {
        picture += ".";
    }
}