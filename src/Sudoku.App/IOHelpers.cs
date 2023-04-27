using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.App;

public static class IOHelpers
{
    public static void PrintOutputToFile(string header, int[,] grid, string filePath, OutputFormat format)
    {
        File.WriteAllLines(filePath, ContentLines());

        IEnumerable<string> ContentLines()
        {
            yield return header ?? ",";
            yield return format switch
            {
                OutputFormat.Print => PrintGrid(grid),
                OutputFormat.Csv => GridToCsv(grid),
                _ => throw new NotSupportedException(nameof(format)),
            };
        }
    }

    public static void PrintOutputToConsole(string header, int[,] grid, OutputFormat format)
    {
        Console.WriteLine(header);
        var content = format switch
        {
            OutputFormat.Print => PrintGrid(grid),
            OutputFormat.Csv => GridToCsv(grid),
            _ => throw new NotSupportedException(nameof(format)),
        };
        Console.WriteLine(content);
    }

    public static (string header, int[,] grid) ReadInputFromFile(string filePath)
    {
        var lines = File.ReadLines(filePath).ToArray();
        if (lines.Length < 10)
        {
            throw new Exception("At least 10 lines shuold be provided");
        }
        var header = lines[0];
        int[,] grid = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            var symbols = lines[i + 1].Split(',', StringSplitOptions.TrimEntries);
            if (symbols.Length != 9)
            {
                throw new Exception($"Sudoku line must have 9 numbers, provided {symbols.Length} symbols");
            }
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = int.Parse(symbols[j]);
            }
        }
        return (header, grid);
    }

    private static string PrintGrid(int[,] grid)
    {
        StringBuilder builder = new();
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
                builder.AppendLine("+---+---+---+");
            for (int j = 0; j < 9; j++)
            {
                if (j % 3 == 0)
                {
                    builder.Append("|");
                }
                builder.Append(grid[i, j] == 0 ? " " : $"{grid[i, j]}");
            }
            builder.AppendLine("|");
        }
        builder.AppendLine("+---+---+---+");
        return builder.ToString();
    }

    private static string GridToCsv(int[,] grid)
    {
        StringBuilder builder = new();
        for (int i = 0; i < 9; i++)
            builder.AppendLine(string.Join(',',
                Enumerable.Range(0, 9).Select(j => grid[i, j])));
        return builder.ToString();
    }
}