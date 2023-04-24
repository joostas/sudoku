namespace Sudoku.Lib;

public class SudokuGenerator
{
    public static readonly int M = 3;
    public static readonly int N = M * M;

    private int[] _rowNumbers = new int[N];
    private int[] _columnNumbers = new int[N];
    private int[] _boxNumbers = new int[N];

    private readonly int _maxIndex = 0;

    private readonly int[,]? _grid;
    private Random _random;
    private int[] _indexes;

    private void Shuffle(int[] indexes)
    {
        var length = indexes.Length;
        for (var i = length - 1; i > 0; i--)
        {
            var rand = _random.Next(i);
            (indexes[i], indexes[rand]) = (indexes[rand], indexes[i]);
        }
    }

    /// <summary>
    /// Fills diagonal boxes of sudoku.
    /// Only one rule should be validated - values in box are unique.
    /// </summary>
    private int[,] FillDiagonalBoxes(int[,] grid)
    {
        int[] numbers = Enumerable.Range(1, N).ToArray();
        for (int i = 0; i < M; i++)
        {
            Shuffle(numbers);
            int j = 0;
            foreach (var number in numbers)
            {
                var (row, column) = GetGlobalCoordinates(i, j);
                grid[row, column] = number;
                MarkCell(number, row, column);
                j++;
            }
        }

        return grid;

        static (int row, int col) GetGlobalCoordinates(int boxNumber, int sequenceNo)
        {
            var row = boxNumber * M + sequenceNo / M;
            var col = boxNumber * M + sequenceNo % M;
            return (row, col);
        }
    }

    public (bool canBeGenerated, int[,] grid) GenerateSudoku(int seed, Level difficulty)
    {
        return GenerateSudoku(seed, () => GetCluesCount(difficulty));

        short GetCluesCount(Level dificulty) =>
            (short)(difficulty switch
            {
                Level.VeryEasy => _random.Next(47, 60),
                Level.Easy => _random.Next(36, 47),
                Level.Normal => _random.Next(32, 36),
                Level.Hard => _random.Next(28, 32),
                Level.Extreme => _random.Next(17, 28),
            });
    }

    private (bool canBeGenerated, int[,] grid) GenerateSudoku(int seed, Func<short> getNumberOfClues)
    {
        Initialize(seed);
        var grid = new int[N, N];
        FillDiagonalBoxes(grid);
        var canBeGenerated = GenerateSudoku(grid, 0);
        if (!canBeGenerated)
        {
            return (false, new int[0, 0]);
        }
        var itemsToRemove = (N * N) - getNumberOfClues();
        while (itemsToRemove > 0)
        {
            var rnd = _random.Next(N * N);
            var (row, column) = (rnd / N, rnd % N);
            if (grid[row, column] == 0)
                continue;
            grid[row, column] = 0;
            itemsToRemove--;
        }
        return (true, grid);

        /// <summary>
        /// This method initializes/resets solver internal state.
        /// It must be called before generating new puzzle.
        /// </summary>
        void Initialize(int seed)
        {
            _rowNumbers = new int[N];
            _columnNumbers = new int[N];
            _boxNumbers = new int[N];
            _random = new Random(seed);
            _indexes = Enumerable.Range(0, N * N).ToArray();
            Shuffle(_indexes);
        }
    }

    private bool GenerateSudoku(int[,] grid, int sequenceNo)
    {
        if (sequenceNo == N * N)
            return true;
        var (row, column) = GetCoordinates(sequenceNo);
        if (grid[row, column] > 0)
        {
            return GenerateSudoku(grid, sequenceNo + 1);
        }
        else
        {
            for (int no = 1; no <= N; no++)
            {
                if (IsValid(no, row, column))
                {
                    grid[row, column] = no;
                    MarkCell(no, row, column);
                    bool canSolve = GenerateSudoku(grid, sequenceNo + 1);
                    if (canSolve)
                        return true;
                    grid[row, column] = 0;
                    CleanupCell(no, row, column);
                }
            }
        }
        return false;
    }

    private (int row, int column) GetCoordinates(int sequenceNumber)
    {
        var index = _indexes[sequenceNumber];
        return (index / N, index % N);
    }

    public bool IsValid(int number, int row, int column)
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

    private static int GetBoxIndex(int row, int column) => row / M * M + column / M;

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
}