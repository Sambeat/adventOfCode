// See https://aka.ms/new-console-template for more information

using AdventOfCode.Puzzles;

Console.WriteLine("Hello, World!");

var day1 = await Calendar.FetchPuzzleAsync(2022, 1);

var caloriesLines = day1.InputLines;

var caloriesTotals = new List<int>();

var currentCaloriesSum = 0;
foreach (var line in caloriesLines){
  if (line == string.Empty)
  {
    caloriesTotals.Add(currentCaloriesSum);
    currentCaloriesSum = 0;
  }
  else
  {
    var calories = int.Parse(line);
    currentCaloriesSum += calories;
  }
}

var maxCalories = caloriesTotals.Max();

// Part 1 solution
Console.WriteLine(maxCalories);

caloriesTotals.Remove(maxCalories);

var maxCalories2 = caloriesTotals.Max();

caloriesTotals.Remove(maxCalories2);

var maxCalories3 = caloriesTotals.Max();

// Part 2 solution
Console.WriteLine(maxCalories + maxCalories2 + maxCalories3);

