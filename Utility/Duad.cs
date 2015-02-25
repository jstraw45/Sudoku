/******************************************************************************
 * Extremely simple 2D structure to identify location in a Sudoku puzzle grid
 * Similar to Point structure, but swaps axes for intuitive clarity
 * Jeff Straw | Northwestern Michigan College
 * 02/21/2015: Initial release
 * 02/23/2015: Added ToString()
 *****************************************************************************/

namespace Utility
{
    /// <summary>
    /// 2D structure to identify location in a Sudoku puzzle grid
    /// </summary>
    public struct Duad
    {
        #region [ Properties ]
        /// <summary>
        /// Vertical grouping
        /// </summary>
        public int Row;

        /// <summary>
        /// Horizontal grouping
        /// </summary>
        public int Column; 
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// 2D structure to identify location in a Sudoku puzzle grid
        /// </summary>
        /// <param name="row">Vertical grouping</param>
        /// <param name="column">Horizontal grouping</param>
        public Duad(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        } 
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Short text representation of this Duad
        /// </summary>
        /// <returns>Simple 2D coordinate set</returns>
        public override string ToString()
        {
            return string.Format("({0:d}, {1:d})", Row, Column);
        } 
        #endregion
    }
}