using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BasicCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Perform Basic Calculation");
                Console.WriteLine("2. Calculate Square Root");
                Console.WriteLine("3. View History");
                Console.WriteLine("4. Exit");

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Enter your calculation (e.g., 5 + 3): ");
                    var input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                        continue;
                    }

                    try
                    {
                        var result = Calculate(input);
                        Console.WriteLine($"Result: {result}");
                        SaveHistory(input, result);
                    }
                    catch (DivideByZeroException)
                    {
                        Console.WriteLine("Error: Division by zero is not allowed.");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input. Please enter a numeric value.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else if (choice == "2")
                {
                    Console.Write("Enter a number to calculate its square root: ");
                    var input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input) || !double.TryParse(input, out double number))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue;
                    }

                    if (number < 0)
                    {
                        Console.WriteLine("Error: Cannot calculate the square root of a negative number.");
                        continue;
                    }

                    var result = Math.Sqrt(number);
                    var expression = $"√{number}";
                    Console.WriteLine($"Square Root: {expression} = {result}");
                    SaveHistory(expression, result);
                }
                else if (choice == "3")
                {
                    ViewHistory();
                }
                else if (choice == "4")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select 1, 2, 3, or 4.");
                }
            }
        }

        static double Calculate(string input)
        {
            var parts = input.Split(' ');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid calculation format. Please use the format: number operator number (e.g., 5 + 3)");
            }

            var num1 = Convert.ToDouble(parts[0]);
            var operation = parts[1];
            var num2 = Convert.ToDouble(parts[2]);

            return operation switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                "/" when num2 == 0 => throw new DivideByZeroException(),
                "/" => num1 / num2,
                _ => throw new InvalidOperationException("Invalid operation"),
            };
        }

        static void SaveHistory(string expression, double result)
        {
            var historyEntry = new HistoryEntry { Expression = expression, Result = result };
            var history = new List<HistoryEntry>();

            if (File.Exists("CS-calc-history.json"))
            {
                try
                {
                    var jsonString = File.ReadAllText("CS-calc-history.json");
                    history = JsonSerializer.Deserialize<List<HistoryEntry>>(jsonString) ?? new List<HistoryEntry>();
                }
                catch (JsonException)
                {
                    history = new List<HistoryEntry>();
                }
            }

            history.Add(historyEntry);
            var updatedJsonString = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("CS-calc-history.json", updatedJsonString);
        }

        static void ViewHistory()
        {
            if (!File.Exists("CS-calc-history.json"))
            {
                Console.WriteLine("No history found.");
                return;
            }

            try
            {
                var jsonString = File.ReadAllText("CS-calc-history.json");
                var history = JsonSerializer.Deserialize<List<HistoryEntry>>(jsonString) ?? new List<HistoryEntry>();

                Console.WriteLine("History:");
                for (int i = 0; i < history.Count; i++)
                {
                    var entry = history[i];
                    Console.WriteLine($"{i + 1}: {entry.Expression} = {entry.Result}");
                }
            }
            catch (JsonException)
            {
                Console.WriteLine("Error reading history. The file may be corrupted.");
            }
        }
    }

    class HistoryEntry
    {
        public string? Expression { get; set; }
        public double? Result { get; set; }
    }
}