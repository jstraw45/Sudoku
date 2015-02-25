/******************************************************************************
 * Extremely simple 3D structure to identify location in a Sudoku puzzle grid
 * Jeff Straw | Northwestern Michigan College
 * 02/16/2015: Initial release
 * 02/18/2015: Moved to Utility project
 * 02/23/2015: Added ToString()
 *****************************************************************************/
namespace Utility
{
    /// <summary>
    /// 3D structure to identify location in a Sudoku puzzle grid
    /// </summary>
    public struct Triad
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
        /// Central zone identity
        /// </summary>
        public int HintZone; 
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// 3D structure to identify location in a Sudoku puzzle grid
        /// </summary>
        /// <param name="row">Vertical grouping</param>
        /// <param name="column">Horizontal grouping</param>
        /// <param name="hintZone">Central zone identity</param>
        public Triad(int row, int column, int hintZone)
        {
            this.Row = row;
            this.Column = column;
            this.HintZone = hintZone;
        } 
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Short text representation of this Triad
        /// </summary>
        /// <returns>Simple 3D coordinate set</returns>
        public override string ToString()
        {
            return string.Format("({0:d}, {1:d}, {2:d})", Row, Column, HintZone);
        }

        /// <summary>
        /// Short text representation of this Triad
        /// </summary>
        /// <param name="excludeHint">If HintZone equals this value, do not include it</param>
        /// <returns>Text representation</returns>
        public string ToString(int excludeHint)
        {
            var result = string.Format("Row: {0:d}, Col: {1:d}", Row, Column);
            if (HintZone != excludeHint)
                result += string.Format(", Hint: {0}", HintZone);

            return result;
        } 
        #endregion
    }
}