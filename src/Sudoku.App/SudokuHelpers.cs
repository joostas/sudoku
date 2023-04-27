namespace Sudoku.App;

public static class SudokuHelpers
{
    public static int[,] CreateSudokuGrid(int[][] input)
    {
        int[,] grid = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = input[i][j];
            }
        }
        return grid;
    }

    public static void PrintGrid(int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
                Console.WriteLine("+---+---+---+");
            for (int j = 0; j < 9; j++)
            {
                if (j % 3 == 0)
                {
                    Console.Write("|");
                }
                Console.Write(grid[i, j] == 0 ? " " : $"{P(grid[i, j])}");
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("+---+---+---+");
    }

    public static void PrintGridForCopy(int[,] grid)
    {
        Console.WriteLine("new int[][] {");
        for (int i = 0; i < 9; i++)
        {
            Console.Write("    new[] {");
            for (int j = 0; j < 9; j++)
            {
                Console.Write(P(grid[i, j]));
                if (j < 8)
                    Console.Write(",");
            }
            Console.WriteLine("},");
        }
        Console.WriteLine("}");
    }

    private static string P(int number) => $"{number:X}";
}