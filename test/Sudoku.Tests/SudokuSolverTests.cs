using Sudoku.Lib;

namespace Sudoku.Tests;

public class ExampleSudoku
{
    // How looks puzzle generated for 2023, Level.VeryEasy
    // +---+---+---+
    // |   |  4|1  |
    // |   |19 | 53|
    // |3 1|  7|  8|
    // +---+---+---+
    // | 2 |   |   |
    // |  7|98 | 4 |
    // |   |4  | 75|
    // +---+---+---+
    // |   | 73| 9 |
    // |   |8 6| 3 |
    // |   | 4 |2 1|
    // +---+---+---+
    public int[,] Puzzle { get; }
    public ExampleSudoku()
    {
        var generator = new SudokuGenerator();
        (_, Puzzle) = generator.GenerateSudoku(2023, Level.Extreme);
    }
}

public class SudokuSolverTest : IClassFixture<ExampleSudoku>
{
    private readonly int[,] _puzzle;
    private readonly SudokuSolver _solver = new();
    public SudokuSolverTest(ExampleSudoku example)
    {
        var n = example.Puzzle.GetLength(0);
        _puzzle = new int[n, n];
        Array.Copy(example.Puzzle, _puzzle, example.Puzzle.Length);
    }

    [Fact]
    public void CanSolve()
    {
        var result = _solver.SolveSudoku(_puzzle);
        result.Should().Be(SudokuStatus.Solved);
    }

    [Fact]
    public void RowViolationRuleIsDetected()
    {
        _puzzle[0, 0] = 4; //first row has double 4 (see puzzle above)
        var result = _solver.SolveSudoku(_puzzle);
        result.Should().Be(SudokuStatus.RowRuleViolated);
    }

    [Fact]
    public void ColumnViolationRuleIsDetected()
    {
        _puzzle[0, 1] = 2; //secound column has double 2 (see puzzle above)
        var result = _solver.SolveSudoku(_puzzle);
        result.Should().Be(SudokuStatus.ColumnRuleViolated);
    }

    [Fact]
    public void BoxViolationRuleIsDetected()
    {
        _puzzle[0, 1] = 3; //first box has double 3 (see puzzle above)
        var result = _solver.SolveSudoku(_puzzle);
        result.Should().Be(SudokuStatus.BoxRuleViolated);
    }

    [InlineData(-10)]
    [InlineData(10)]
    [Theory]
    public void NumberOutOfRangeIsDetected(int invalidNumber)
    {
        _puzzle[8, 8] = invalidNumber;
        var result = _solver.SolveSudoku(_puzzle);
        result.Should().Be(SudokuStatus.NumberOutOfRange);
    }
}