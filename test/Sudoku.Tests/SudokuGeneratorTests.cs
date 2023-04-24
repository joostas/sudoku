using Sudoku.Lib;

namespace Sudoku.Tests;

public class SudokuGeneratorTests
{
    [Theory]
    [InlineData(Level.VeryEasy)]
    [InlineData(Level.Easy)]
    [InlineData(Level.Normal)]
    [InlineData(Level.Hard)]
    [InlineData(Level.Extreme)]
    public void GeneratedSudokuIsSolvable(Level level)
    {
        SudokuGenerator generator = new();
        (_, int[,] puzzle) = generator.GenerateSudoku(2023, level);
        SudokuSolver solver = new();
        SudokuStatus canBeSolved = solver.SolveSudoku(puzzle);
        canBeSolved.Should().Be(SudokuStatus.Solved);
    }

    [Theory]
    [InlineData(Level.Easy)]
    [InlineData(Level.Hard)]
    public void SamePuzzleIsGeneratedForSameIdAndLevel(Level level)
    {
        int id = 2023;
        SudokuGenerator generator = new();
        (_, int[,] puzzle) = generator.GenerateSudoku(id, level);
        (_, int[,] puzzle2) = generator.GenerateSudoku(id, level);
        puzzle.Should().BeEquivalentTo(puzzle2);
    }

    [Fact]
    public void DifferentPuzzleIsGeneratedForSameLevelButDifferentId()
    {
        int id = 2023;
        SudokuGenerator generator = new();
        (_, int[,] puzzle) = generator.GenerateSudoku(id, Level.Easy);
        (_, int[,] puzzle2) = generator.GenerateSudoku(id + 1, Level.Easy);
        puzzle.Should().NotBeEquivalentTo(puzzle2);
    }

    [Fact]
    public void DifferentPuzzleIsGeneratedForSameIdButDifferentLevel()
    {
        int id = 2023;
        SudokuGenerator generator = new();
        (_, int[,] puzzle) = generator.GenerateSudoku(id, Level.Easy);
        (_, int[,] puzzle2) = generator.GenerateSudoku(id, Level.VeryEasy);
        puzzle.Should().NotBeEquivalentTo(puzzle2);
    }
}