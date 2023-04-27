namespace Sudoku.Lib;

public class SudokuSolver
{

    // Currently sudoku solver is fixed to 9x9 matrix, but it is coded that it could be expanded to different size
    // e.g. 2 => 4x4, or 4 => 16x16
    public static readonly int M = 3;
    public static readonly int N = M * M;

    private int[] _rowNumbers = new int[N];
    private int[] _columnNumbers = new int[N];
    private int[] _boxNumbers = new int[N];

    public SudokuStatus SolveSudoku(int[,] grid)
    {
        SudokuStatus status = FillNumbers(grid);
        if (status != SudokuStatus.Valid)
            return status;
        return SolveSudoku(grid, 0) ?
            SudokuStatus.Solved :
            SudokuStatus.Unsolvable;
    }

    private bool SolveSudoku(int[,] grid, int index)
    {
        if (index == N * N)
            return true;
        var row = index / N;
        var column = index % N;
        if (grid[row, column] > 0)
        {
            return SolveSudoku(grid, index + 1);
        }
        else
        {
            for (int no = 1; no <= N; no++)
            {
                if (IsValid(no, row, column))
                {
                    grid[row, column] = no;
                    MarkCell(no, row, column);
                    bool canSolve = SolveSudoku(grid, index + 1);
                    if (canSolve)
                        return true;
                    grid[row, column] = 0;
                    CleanupCell(no, row, column);
                }
            }
        }
        return false;
    }

    private SudokuStatus FillNumbers(int[,] grid)
    {
        _rowNumbers = new int[N];
        _columnNumbers = new int[N];
        _boxNumbers = new int[N];
        for (var i = 0; i < N; i++)
        {
            for (var j = 0; j < N; j++)
            {
                var cellNumber = grid[i, j];
                if (cellNumber == 0)
                    continue;
                if (cellNumber < 0 || cellNumber > N)
                {
                    return SudokuStatus.NumberOutOfRange;
                }
                var numberBit = GetNumberBit(cellNumber);
                if ((_rowNumbers[i] | numberBit) == _rowNumbers[i])
                    return SudokuStatus.RowRuleViolated;
                if ((_columnNumbers[j] | numberBit) == _columnNumbers[j])
                    return SudokuStatus.ColumnRuleViolated;
                var boxIndex = GetBoxIndex(i, j);
                if ((_boxNumbers[boxIndex] | numberBit) == _boxNumbers[boxIndex])
                    return SudokuStatus.BoxRuleViolated;
                MarkCell(cellNumber, i, j);
            }
        }
        return SudokuStatus.Valid;
    }

    private bool IsValid(int number, int row, int column)
    {
        if (number == 0)
            return true;
        if (number < 0 || number > N)
            return false;
        var numberBit = GetNumberBit(number);
        return
            CanBeAdded(_rowNumbers[row]) &&
            CanBeAdded(_columnNumbers[column]) &&
            CanBeAdded(_boxNumbers[GetBoxIndex(row, column)]);

        bool CanBeAdded(int numbers) =>
            (numbers | numberBit) != numbers;
    }

    private void MarkCell(int number, int row, int column)
    {
        if (number < 1)
            throw new NotSupportedException($"Can be marked with positive number only. Given {number}");
        var numberBit = GetNumberBit(number);
        _rowNumbers[row] |= numberBit;
        _columnNumbers[column] |= numberBit;
        _boxNumbers[GetBoxIndex(row, column)] |= numberBit;
    }

    private void CleanupCell(int number, int row, int column)
    {
        if (number < 1)
            throw new NotSupportedException($"Can be cleaned up from positive number only. Given {number}");
        var numberBit = GetNumberBit(number);
        _rowNumbers[row] ^= numberBit;
        _columnNumbers[column] ^= numberBit;
        _boxNumbers[GetBoxIndex(row, column)] ^= numberBit;
    }

    private int GetNumberBit(int number)
    {
        if (number < 0)
            throw new ArgumentOutOfRangeException(nameof(number), number, null);
        if (number == 0)
            return 0;
        return 1 << (number - 1);
    }

    private static int GetBoxIndex(int row, int column) => row / M * M + column / M;

}