/****************************************************************
 * Generator of Sudoku puzzles
 * Jeff Straw | Northwestern Michigan College 
 * 02/19/2015: initial
 * 02/24/2015: more choices
 ***************************************************************/

namespace Sudoku.Model
{
    /// <summary>
    /// Generate Sudoku puzzles
    /// </summary>
    public static class PuzzleFactory
    {
        // Puzzle specifications
        private static char firstChar = '1';            // '1' good for numeric 9x9; 'A' better for larger

        #region [ Sample puzzles ]
        // Should work for perfect squares 4, 9, 16, and 25
        private static char[,] blank9x9Game = new char[9, 9]    // Not intended to be solved - template for others
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

        private static char[,] simple9x9Game = new char[9, 9]
        {
            {' ', '1', ' ', '7', '2', ' ', '3', ' ', ' '},
            {'2', ' ', ' ', ' ', '5', ' ', '6', '8', ' '},
            {' ', ' ', ' ', '4', ' ', '8', ' ', ' ', '9'},
            {' ', ' ', ' ', '2', ' ', ' ', '5', ' ', '8'},
            {'8', ' ', '6', '9', '4', '5', '7', ' ', '2'},
            {'5', ' ', '7', ' ', ' ', '6', ' ', ' ', ' '},
            {'1', ' ', ' ', '5', ' ', '7', ' ', ' ', ' '},
            {' ', '8', '2', ' ', '3', ' ', ' ', ' ', '1'},
            {' ', ' ', '3', ' ', '9', '2', ' ', '7', ' '}
        };

        // Set firstChar to 'J' for this puzzle
        private static char[,] simpleAlpha9x9Game = new char[9, 9]
        {
            {' ', 'L', ' ', 'J', 'N', ' ', 'M', ' ', ' '},
            {'N', ' ', ' ', ' ', 'P', ' ', 'Q', 'K', ' '},
            {' ', ' ', ' ', 'R', ' ', 'K', ' ', ' ', 'O'},
            {' ', ' ', ' ', 'N', ' ', ' ', 'P', ' ', 'K'},
            {'K', ' ', 'Q', 'O', 'R', 'P', 'J', ' ', 'N'},
            {'P', ' ', 'J', ' ', ' ', 'Q', ' ', ' ', ' '},
            {'L', ' ', ' ', 'P', ' ', 'J', ' ', ' ', ' '},
            {' ', 'K', 'N', ' ', 'M', ' ', ' ', ' ', 'L'},
            {' ', ' ', 'M', ' ', 'O', 'N', ' ', 'J', ' '}
        };

        private static char[,] medium9x9Game = new char[9, 9]
        {
            {' ', ' ', '2', '6', ' ', ' ', '9', ' ', ' '},
            {' ', '8', ' ', ' ', ' ', '7', ' ', '1', ' '},
            {'6', ' ', ' ', ' ', ' ', ' ', '7', ' ', '5'},
            {'4', '3', ' ', ' ', ' ', ' ', ' ', ' ', '6'},
            {' ', ' ', ' ', '3', ' ', '5', ' ', ' ', ' '},
            {'2', ' ', ' ', ' ', ' ', ' ', ' ', '3', '9'},
            {'3', ' ', '4', ' ', '1', ' ', ' ', ' ', '7'},
            {' ', '5', ' ', '8', ' ', ' ', ' ', '2', ' '},
            {' ', ' ', '6', ' ', ' ', '3', '5', ' ', ' '}
        };

        private static char[,] sample4x4Game = new char[4, 4]
        {
            {' ', '3', '1', ' '},
            {' ', ' ', ' ', '3'},
            {'4', ' ', ' ', ' '},
            {' ', '2', '4', ' '}
        }; 
        #endregion

        /// <summary>
        /// Instantiate a Sudoku puzzle
        /// </summary>
        /// <returns>A Sudoku puzzle</returns>
        public static Game GetSudoku()
        {
            // Much more needs to be done...
            return new Game(firstChar, 9, simple9x9Game);
            // return new Game(firstChar, 4, sample4x4Game);
        }
    }
}