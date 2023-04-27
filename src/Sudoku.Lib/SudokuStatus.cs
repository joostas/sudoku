namespace Sudoku.Lib;

public enum SudokuStatus : byte
{
    None = 0,
    Valid = 1,
    Solved = 2,
    RowRuleViolated = 3,
    ColumnRuleViolated = 4,
    BoxRuleViolated = 5,
    NumberOutOfRange = 6,
    Unsolvable = 7,
}
