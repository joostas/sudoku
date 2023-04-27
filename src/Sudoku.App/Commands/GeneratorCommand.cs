using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Spectre.Console;
using Spectre.Console.Cli;

using Sudoku.Lib;

namespace Sudoku.App;

public sealed class GeneratorCommand : Command<GeneratorCommand.Settings>
{

    public sealed class Settings : CommandSettings
    {

        [Description("Same id will generate same sudoku")]
        [CommandOption("-n|--id")]
        [DefaultValue(42)]
        public int Id { get; init; }

        [Description("Supported values: " +
              nameof(Level.VeryEasy) + ", " +
              nameof(Level.Easy) + ", " +
              nameof(Level.Normal) + ", " +
              nameof(Level.Hard) + ", " +
              nameof(Level.Extreme))
        ]
        [CommandOption("-l|--level")]
        [DefaultValue(Level.Normal)]
        public Level Level { get; init; }

        [Description("File name for generated sudoku. If this option is skiped, output is printed to console")]
        [CommandOption("-o|--output")]
        public string? OutputFile { get; init; }

        [Description("Supported values: " +
              nameof(OutputFormat.Print) + ", (default for console output)" +
              nameof(OutputFormat.Csv) + " (default for console output)")
        ]
        [CommandOption("-f|--format")]
        public OutputFormat? Format { get; set; }

    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        SudokuGenerator generator = new();
        AnsiConsole.MarkupLine($"Generating sudoku with id [green]{settings.Id}[/] and Level [green]{settings.Level}[/]...");
        var (_, puzzle) = generator.GenerateSudoku(settings.Id, settings.Level);
        if (settings.OutputFile is null)
        {
            string header = $"Sudoku id: {settings.Id}, level: {settings.Level}";
            OutputFormat format = settings.Format.GetValueOrDefault(OutputFormat.Print);
            IOHelpers.PrintOutputToConsole(header, puzzle, format);
        }
        else
        {
            string header = $"{settings.Id},{settings.Level}";
            OutputFormat format = settings.Format.GetValueOrDefault(OutputFormat.Csv);
            IOHelpers.PrintOutputToFile(header, puzzle, settings.OutputFile, format);
        }

        return 0;
    }
}