/****************************************************************
 * Completeness measure for individual cell in Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College 
 * 02/19/2015 initial design
 ***************************************************************/

namespace Sudoku.Model
{
    /// <summary>
    /// Measure of completeness in solving a Cell
    /// </summary>
    public enum CellStatus
    {
        /// <summary>
        /// Value defined originally by puzzle
        /// </summary>
        Known,

        /// <summary>
        /// Value not yet known
        /// </summary>
        Unknown,

        /// <summary>
        /// Value believed to be true, declared by player using logic
        /// </summary>
        DerivedManually,

        /// <summary>
        /// Value believed to be true, solved by computer, assumes no player errors
        /// </summary>
        DerivedSemiAutomatically,

        /// <summary>
        /// Partial solution showing subset of hints declared by player
        /// </summary>
        Scratchpad
    }
}