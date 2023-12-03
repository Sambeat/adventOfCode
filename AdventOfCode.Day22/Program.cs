// See https://aka.ms/new-console-template for more information


using AdventOfCode.Day22;
using AdventOfCode.Events;

var today = await Calendar.OpenPuzzleAsync(2022, 22);

// today.PrintLines();

// var lines = today.InputLinesTrimmed;
var lines = File.ReadLines("test.txt").ToArray();

// var puzzleSplit = lines.Split("").ToList();

// var map = puzzleSplit[0].ToList();
var map = lines.Take(200).ToList();

var mapHeight = map.Count;
var mapWidth = map.Max(l => l.Length);
    // +1;

//Pad map with empty space
// map.Insert(0, new string(Enumerable.Range(0, mapWidth).Select(_ => '\0').ToArray()));
// map.Add(new string(Enumerable.Range(0, mapWidth).Select(_ => '\0').ToArray()));


var mapArray = new char[mapWidth, mapHeight];

for (int y = 0; y < mapHeight; y++)
{
    var line = map[y];
    for (int x = 0; x < line.Length; x++)
    {
        // if (x == 0)
        // {
        //     mapArray[x, y] = '\0';
        // }
        mapArray[x, y] = line[x];
    }
}

var playerPos = WrapRight(0, 0, 0);
Console.WriteLine(playerPos);

var playerDirection = Direction.Right;

var commands = lines.TakeLast(1).ToList().First().ToList();
var currentNumber = "";
for (var i = 0; i < commands.Count; i++)
{
    if (char.IsDigit(commands[i]))
    {
        currentNumber += commands[i];
        continue;
    }
    else
    {
        var numberOfSteps = int.Parse(currentNumber);
        currentNumber = "";
        // Execute move
        playerPos = playerDirection switch
        {
            Direction.Up => WrapUp(playerPos.Item1, playerPos.Item2, numberOfSteps),
            Direction.Down => WrapDown(playerPos.Item1, playerPos.Item2, numberOfSteps),
            Direction.Left => WrapLeft(playerPos.Item1, playerPos.Item2, numberOfSteps),
            Direction.Right => WrapRight(playerPos.Item1, playerPos.Item2, numberOfSteps),
            _ => throw new ArgumentOutOfRangeException()
        };
        Console.WriteLine($"{playerPos} - {numberOfSteps}{playerDirection}");


        //Turn
        switch (commands[i])
        {
            case 'R':
                playerDirection = playerDirection.Next();
                break;
            case 'L':
                playerDirection = playerDirection.Previous();
                break;
        }
    }
}

Console.WriteLine(playerPos);

Console.WriteLine(1000 * (playerPos.Item2 + 1) + 4 * (playerPos.Item1 + 1) + (int) playerDirection );


(int, int) WrapRight(int currentX, int currentY, int steps)
{
    var currentPosition = (currentX, currentY);
    for (var x = currentX + 1; x % mapWidth < mapWidth; x++)
    {
        var newX = x % mapWidth;

        if (mapArray[newX, currentY] == '#')
        {
            return currentPosition;
        }
        else if (mapArray[newX, currentY] == '.')
        {
            currentPosition = (newX, currentY);
            steps--;
        }
        else
        {
            switch (mapArray[newX, currentY])
            {
                case '\0':
                    continue;
                case '#':
                    return currentPosition;
                case '.':
                    currentPosition = (newX, currentY);
                    steps--;
                    break;
            }
        }
        
        if (steps <= 0)
        {
            return currentPosition;
        }
    }

    return (currentX, currentY);
}

(int, int) WrapLeft(int currentX, int currentY, int steps)
{
    var currentPosition = (currentX, currentY);
    for (var x = currentX == 0 ? mapWidth - 1 : currentX - 1; Math.Abs(x % mapWidth) >= 0; x--)
    {
        var newX = (mapWidth + x) % mapWidth;

        if (mapArray[newX, currentY] == '#')
        {
            return currentPosition;
        }
        else if (mapArray[newX, currentY] == '.')
        {
            currentPosition = (newX, currentY);
            steps--;
        }
        else
        {
            switch (mapArray[newX, currentY])
            {
                case '\0':
                    continue;
                case '#':
                    return currentPosition;
                case '.':
                    currentPosition = (newX, currentY);
                    steps--;
                    break;
            }
        }
        
        if (steps <= 0)
        {
            return currentPosition;
        }
    }

    return (currentX, currentY);
}

(int, int) WrapUp(int currentX, int currentY, int steps)
{
    var currentPosition = (currentX, currentY);

    
    for (var y = currentY == 0 ? mapHeight - 1 : currentY - 1; Math.Abs(y % mapHeight) >= 0; y--)
    {
        var newY = (mapHeight + y) % mapHeight;

        if (mapArray[currentX, newY] == '#')
        {
            return currentPosition;
        }
        else if (mapArray[currentX, newY] == '.')
        {
            currentPosition = (currentX, newY);
            steps--;
        }
        else
        {
            // newY++;
            // Console.WriteLine($"before up {newY}");
            //
            // while (mapArray[currentX, newY] != '\0')
            // {
            //     newY++;
            //
            //     Console.WriteLine($"During up {newY}");
            //
            //     if (newY == mapHeight)
            //     {
            //         break;
            //     }
            // }
            //
            // newY--;
            // Console.WriteLine($"After up {newY}");

            switch (mapArray[currentX, newY])
            {
                case '\0':
                    continue;
                case '#':
                    return currentPosition;
                case '.':
                    currentPosition = (currentX, newY);
                    steps--;
                    break;
            }
        }
        
        if (steps <= 0)
        {
            return currentPosition;
        }
    }

    return (currentX, currentY);
}

(int, int) WrapDown(int currentX, int currentY, int steps)
{
    var currentPosition = (currentX, currentY);

    
    for (var y = currentY + 1; y % mapHeight < mapHeight; y++)
    {
        var newY = Math.Abs(y % mapHeight);

        if (mapArray[currentX, newY] == '#')
        {
            return currentPosition;
        }
        else if (mapArray[currentX, newY] == '.')
        {
            currentPosition = (currentX, newY);
            steps--;
        }
        else
        {
            // newY = Math.Abs(newY - 1 % mapHeight);
            // Console.WriteLine($"before d {newY}");
            // while (mapArray[currentX, newY] != '\0')
            // {
            //     newY--;
            //
            //     if (newY == -1)
            //     {
            //         break;
            //     }
            //     Console.WriteLine($"During d {newY}");
            //
            // }
            //
            // newY++;
            // Console.WriteLine($"After d {newY}");

            switch (mapArray[currentX, newY])
            {
                case '\0':
                    continue;
                case '#':
                    return currentPosition;
                case '.':
                    currentPosition = (currentX, newY);
                    steps--;
                    break;
            }
        }
        
        if (steps <= 0)
        {
            return currentPosition;
        }
    }

    return (currentX, currentY);
}