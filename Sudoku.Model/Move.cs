/************************************************************************************
 * Extremely simple 3D structure to specify a potential move in a Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College
 * 02/23/2015: Initial release
 ***********************************************************************************/

namespace Sudoku.Model
{
    /// <summary>
    /// 3D structure to identify potential move in a Sudoku puzzle
    /// </summary>
    public struct Move
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

        /// <summary>
        /// Solution for the Cell
        /// </summary>
        public char Value;
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// 3D structure to identify location in a Sudoku puzzle grid
        /// </summary>
        /// <param name="row">Vertical grouping</param>
        /// <param name="column">Horizontal grouping</param>
        /// <param name="value">Solution for the Cell</param>
        public Move(int row, int column, char value)
        {
            this.Row = row;
            this.Column = column;
            this.Value = value;
        }
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Short text representation of this Move
        /// </summary>
        /// <returns>Simple 3D coordinate set</returns>
        public override string ToString()
        {
            return string.Format("({0:d}, {1:d}): '{2}'", Row, Column, Value);
        }
        #endregion
    }
}