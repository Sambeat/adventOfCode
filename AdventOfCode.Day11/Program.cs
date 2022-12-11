// See https://aka.ms/new-console-template for more information

using AdventOfCode.Events;
using MoreLinq;

var today = await Calendar.OpenPuzzleAsync(2022, 11);

today.PrintLines();

var lines = today.InputLinesTrimmed;

var monkeys = new List<List<long>> { new() { 83, 62, 93 }, new () {90, 55}, new() {91, 78, 80, 97, 79, 88}, new () {64, 80, 83, 89, 59}, new (){98, 92, 99, 51}, new() {68, 57, 95, 85, 98, 75, 98, 75}, new() {74}, new() {68,64,60,68,87,80,82} };
// var monkeys = new List<List<long>> { new() { 79, 98 }, new() { 54, 65, 75, 74 }, new() { 79, 60, 97 }, new() { 74 } };

// var numRounds = 20;
var mod = 2*17*19*3*5*13*7*11;

var numRounds = 10000;
var monkeyInspections = new long[8];
for (var r = 0; r < numRounds; r++)
{
    foreach (var (monkey, i) in monkeys.Select((m, i) => (m, i)))
    {
        monkeyInspections[i] += monkey.Count;
        switch (i)
        {
            case 0:
                doMonkey(monkey, (initWorry) => initWorry * 17, 2, 1, 6);
                break;
            case 1:
                doMonkey(monkey, (iw) => iw + 1, 17, 6, 3);
                break;
            case 2:
                doMonkey(monkey, (iw) => iw + 3, 19, 7, 5);
                break;
            case 3:
                doMonkey(monkey, (iw) => iw + 5, 3, 7, 2);
                break;
            case 4:
                doMonkey(monkey, (iw) => iw * iw, 5, 0, 1);
                break;
            case 5:
                doMonkey(monkey, (iw) => iw + 2, 13, 4, 0);
                break;
            case 6:
                doMonkey(monkey, (iw) => iw + 4, 7, 3, 2);
                break;
            case 7:
                doMonkey(monkey, (iw) => iw * 19, 11, 4, 5);
                break;
            
            // case 0:
            //     doMonkey(monkey, (iw) => iw * 19L, 23L, 2, 3);
            //     break;
            // case 1:
            //     doMonkey(monkey, (iw) => iw +6L, 19L, 2, 0);
            //     break;
            // case 2:
            //     doMonkey(monkey, (iw) => iw * iw, 13L, 1, 3);
            //     break;
            // case 3: 
            //     doMonkey(monkey, (iw) => iw + 3L, 17L, 0, 1);
            //     break;
        }
    }
}


var sortedInspections = monkeyInspections.OrderByDescending(i => i).ToList();
Console.WriteLine($"{sortedInspections[0]} * {sortedInspections[1]}");
Console.WriteLine(sortedInspections[0] * sortedInspections[1]);

void doMonkey(List<long> monkey, Func<long, long> operation, long divisible, int onTrue, int onFalse)
{
    foreach (var item in monkey)
    {
        var opResult = operation(item);

        // var afterBored = opResult / ;
        var afterBored = opResult % mod;
        
        if (afterBored % divisible == 0L)
        {
            monkeys[onTrue].Add(afterBored);
        }
        else
        {
            monkeys[onFalse].Add(afterBored);
        }
    }
    
    monkey.Clear();
}