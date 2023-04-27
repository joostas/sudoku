# Sudoku
Sudoku generator and Sudoku solver program.

Program is implemented as dotnet tool (special NuGet package that contains a console application). More details about dotnet tools can be found [here](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools).

Functionality of program:
* Generate sudoku puzzle
* Solve sudoku puzzle

## How to run
### Install
Program is built using `.NET 7`. To be able to install tool you need to have .`NET SDK 7.0` or later.
Package is published to MyGet package feed.

Run this command to install tool:
```bash
dotnet tool install sudoku --add-source https://www.myget.org/F/demos/api/v3/index.json -g
```
After installing the tool you can run in console command `sudoku`. It will provide output with short description

```
USAGE:
    Sudoku.dll [OPTIONS] <COMMAND>

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    generate
    solve
```
### Run
* Get help for generate command:
```
sudoku generate -h
```
```
USAGE:
    Sudoku.dll generate [OPTIONS]

OPTIONS:
                    DEFAULT
    -h, --help                 Prints help information
    -n, --id        42         Same id will generate same sudoku
    -l, --level     Normal     Supported values: VeryEasy, Easy, Normal, Hard, Extreme
    -o, --output               File name for generated sudoku. If this option is skiped, output is printed to console
    -f, --format               Supported values: Print, (default for console output)Csv (default for console output)
```
* Get help for solve command:
```
sudoku solve -h
```
```
USAGE:
    Sudoku.dll solve [OPTIONS]

OPTIONS:
                   DEFAULT
    -h, --help                Prints help information
    -i, --input               File with sudoku puzzle
    -n, --id                  Id of sudoku to solve (is ignored if option -f is provided)
    -l, --level    Normal     Supported values: VeryEasy, Easy, Normal, Hard, Extreme (is ignored if option -f is
                              provided)
```
### Examples
Generate sudoku puzzle and output it into console:
```
sudoku generate
```
Generator supports different levels:
```
sudoku generate --level Easy
```
Generator is repeatable. It means that it will generate same puzzle for same id and difficulty level.
To generate specific puzzle you need to provide its id:
```
sudoku generate --id 123
```
By default puzzle is printed to console. But it is possible to write it to file:
```
sudoku generate -o sudoku.txt
```
Sudoku can be solved using input from file. Solution is printed to console.
```
sudoku solve -i sudoku.txt
```
It is also posible to get solution for specific sudoku that was generated by this program.
```
sudoku solve --id 42 --level Normal
```

## Implementation

Solution consist of three projects:
* `Sudoku.Lib` - contains main logic for generating and solving sudoku puzzles
* `Sudoku.App` - exposes sudoku functionality through cli
* `Sudoku.Tests` - tests for sudoku generation and solving logic

### Main classes:
`SudokuSolver` - It solves provided sudoku puzzle.
Puzzle is provided as two dimensional array of numbers.

Algorithm:
1. Builds bitmaps for rows, lines and boxes. Also checks if sudoku rules are not violated.
1. Goes through sudoku cells
    1. If cell is not empty go to next one
    1. Add number that does not violate sudoku rules and go to the next cell
    1. If there is no valid number to add to cell, return to previous cell and try different number
1. If all cells are filled than sudoku is solved, otherwise it is treated as unsolvable.

`SudokuGenerator`
Sudoku generator takes as input two parameters:
* id -  it is used as seed to make generation reprodusible.
* level - dificulty level (basically it defines how much empty cells generated puzzle should have)

Algorithm:
1. Fill diagonal boxes with randomly ordered numbers from 1 to 9 (no need to check other sudoku rules)
1. Go through all cells and fill with numbers using same algorithm that is used in `SudokuSolver`
1. Get number of cells that needs to be cleaned up (this is derived from difficulty level)
1. Randomly cleanup cells

Instead of filling cell numbers sequencely generator follows randomly generated sequence of cell indexes.
Generator makes reproducable puzzles. It means that generator with same input will generate same puzzle each time.
This is managed by providing id (this is used as seed).

### Other notes
Program supports writing sudoku puzzle to file and reading it from file.

There must be provided one header row and nine sudoku rows:
```
header row
sudoku rows
```
* header row - free form
* sudoku row format: nine numbers separated with commas. If cell is empty - 0 is used
```
1,2,0,3,4,5,6,7,0,0
```
## How to build and test
To build solution
```
dotnet build
```
To test solution
```
dotnet test
```
To pack as dotnet tool
```
dotnet pack
```
Install tool that was built locally
```
dotnet tool install sudoku --add-source .\src\Sudoku.App\nupkg\ -g
```
How to run see insturctions above.