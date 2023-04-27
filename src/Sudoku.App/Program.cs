// See https://aka.ms/new-console-template for more information
using Spectre.Console.Cli;

using Sudoku.App;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<GeneratorCommand>("generate");
    config.AddCommand<SolverCommand>("solve");
});

return app.Run(args);