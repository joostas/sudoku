namespace Sudoku.Lib
{
    /// <summary>
    /// Based on https://www.researchgate.net/figure/Number-of-clues-for-each-difficulty-level_tbl1_259525699
    /// </summary>
    public enum Level : byte
    {
        None = 0,
        VeryEasy = 1,
        Easy = 2,
        Normal = 3,
        Hard = 4,
        Extreme = 5,
    }
}