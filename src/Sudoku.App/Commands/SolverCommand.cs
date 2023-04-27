using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Spectre.Console;
using Spectre.Console.Cli;

using Sudoku.Lib;

namespace Sudoku.App;

public sealed class SolverCommand : Command<SolverCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("File with sudoku puzzle")]
        [CommandOption("-i|--input")]
        public string? Input { get; set; }

        [Description("Id of sudoku to solve (is ignored if option -f is provided)")]
        [CommandOption("-n|--id")]
        public int? Id { get; set; }

        [Description("Supported values: " +
              nameof(Level.VeryEasy) + ", " +
              nameof(Level.Easy) + ", " +
              nameof(Level.Normal) + ", " +
              nameof(Level.Hard) + ", " +
              nameof(Level.Extreme) + " (is ignored if option -f is provided)")
        ]
        [CommandOption("-l|--level")]
        [DefaultValue(Level.Normal)]
        public Level Level { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.Input is null && settings.Id is null)
        {
            throw new Exception("Eithe option -i or option -n should be provided");
        }
        int[,] puzzle;
        string header;
        if (settings.Input is not null)
        {
            (header, puzzle) = IOHelpers.ReadInputFromFile(settings.Input);
        }
        else
        {
            var generator = new SudokuGenerator();
            (_, puzzle) = generator.GenerateSudoku(settings.Id!.Value, settings.Level);
            header = $"Sudoku id: {settings.Id}, Level: {settings.Level}";
        }
        IOHelpers.PrintOutputToConsole(header, puzzle, OutputFormat.Print);
        var solver = new SudokuSolver();
        var status = solver.SolveSudoku(puzzle);
        switch (status)
        {
            case SudokuStatus.Solved:
                IOHelpers.PrintOutputToConsole("Solution: ", puzzle, OutputFormat.Print);
                return 0;
            case SudokuStatus.Unsolvable:
                AnsiConsole.MarkupLine($"[yellow]Program wasn't able to find solution for given sudoku[/]");
                return 0;
            case SudokuStatus.None:
                throw new Exception($"Solution status is not supported {status}");
            default:
                AnsiConsole.MarkupLine($"Incorrect puzzle provided: rule violated [yellow]{status}[/]");
                return 0;
        }
    }
}