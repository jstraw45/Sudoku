/****************************************************************
 * Generator of Sudoku puzzles
 * Jeff Straw | Northwestern Michigan College 
 * 02/19/2015: initial
 * 02/24/2015: more choices
 * 03/07/2015: added 16x16 sample
 * 03/09/2015: simplified literals
 ***************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Sudoku.Model
{
    /// <summary>
    /// Generate Sudoku puzzles
    /// </summary>
    public static class PuzzleFactory
    {
        #region [ Sample puzzles ]
        // Should work for perfect squares 4, 9, 16, and 25
        // Consider http://www.extremesudoku.info/sudoku.html

        // Templates (use for user-entered puzzles or leverage as a typing aid)
        private static char[,] FourXFourT = new char[4, 4]
        {
            {' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' '}
        };

        private static char[,] NineXNineT = new char[9, 9] 
        {
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}
        };

        private static char[,] SixteenXSixteenT = new char[16, 16] 
        {
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
			{' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}
        };

        // Four-by-four puzzles
        private static string FourXFourEmpty = "                ";

        private static string FourXFourA = " 31    34    24 ";

        private static string FourXFourB = "1 3     2 4     ";

        // Nine-by-nine puzzles
        private static string NineXNineEmpty =
            "                                                                                 ";

        private static string NineXNineA =
            " 1 72 3  2   5 68    4 8  9   2  5 88 69457 25 7  6   1  5 7    82 3   1  3 92 7 ";

        private static string NineXNineB = 
            "  26  9   8   7 1 6     7 543      6   3 5   2      393 4 1   7 5 8   2   6  35  ";

        private static string NineXNineC =
            " 5     7     9     8 7 2 3   89 36   4  5  9   14 65   7 3 8 5     1     9     8 ";

        private static string NineXNineD =
            "7  185   8   2 9   1     78   23  1    5 7    3  46   59     2   7 5   9   869  4";

        private static string NineXNineE =
            "1       3   9 4 2 3 4 7 95  49  8            2  5   1  17 5 4 6 3   9   9       7";

        private static string DN20150307 =
            "  7  5  1 2  3 9     42  6  4  9 857    7    978 1  2  5  46     2 5  9 7  2  3  ";

        private static string DFP20150308 =
            "61 7 9 4 9 3   1          5   4 8  1 3 5   2 7  6 3   3          2   5 4 8 9 2 36";

        // Extra Challenging (Very Hard) Puzzle #104 from puzzles.about.com 03/07/2015
        private static string NineXNineVH =
            " 1 6  3  5   3  18 2 5     3      2    7 4    9      7     6 7 15  9   2  6  3 5 ";

        // Set first Char to 'J' for this puzzle
        private static string NineXNineAlpha =
            " L JN M  N   P QK    R K  O   N  P KK QORPJ NP J  Q   L  P J    KN M   L  M ON J ";

        // Sixteen-by-sixteen puzzles
        private static string SixteenXSixteenEmpty =
            "                                                                " +
            "                                                                " +
            "                                                                " +
            "                                                                ";
        #endregion

        /// <summary>
        /// Instantiate a Sudoku puzzle
        /// </summary>
        /// <returns>A Sudoku puzzle</returns>
        public static Game GetSudoku()
        {
            //return new Game('1', 9, Util.StringToCharArray(NineXNineB));
            return new Game('1', 9, Util.RandomizeArray(Util.StringToCharArray(NineXNineB), Cell.valueWhenUnsolved));
        }
    }
}