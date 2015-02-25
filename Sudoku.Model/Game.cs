/****************************************************************
 * Puzzle game containing Sudoku cells
 * Jeff Straw | Northwestern Michigan College 
 * 02/05/2015: development began
 * 02/14/2015: changed Cell data from int to char
 * 02/20/2015: refactored
 * 02/23/2015: DeepClone added
 ***************************************************************/
using System;
using System.Collections.Generic;
using Utility;

namespace Sudoku.Model
{
    /// <summary>
    /// Two-dimensional array representing a Sudoku puzzle
    /// </summary>
    [Serializable]
    public class Game
    {
        #region [ Fields ]
        // Fields
        private Cell[,] _cells;                             // SideLength:row * SideLength:column
        private int _sideLength;
        internal int squareRootOfSideLength = 0;
        #endregion

        #region [ Properties ]
        /// <summary>
        /// Smallest square grid dimension allowed
        /// </summary>
        public static readonly int MinDimensionValue = 4;   // Must be a perfect square, and 4 is smallest that is "interesting"

        /// <summary>
        /// Largest square grid dimension allowed (Compiled value can be edited)
        /// </summary>
        public static readonly int MaxDimensionValue = 25;  // To keep manageable - and even a 25 x 25 game may be unwieldy

        /// <summary>
        /// Game representation in two-dimensional array
        /// </summary>
        public Cell[,] Cells                                // SideLength:row x SideLength:column
        {
            get { return _cells; }
            private set { _cells = value; }
        }
        
        /// <summary>
        /// Number of rows, columns, and regions in the Game
        /// </summary>
        public int SideLength
        {
            get { return _sideLength; }
            private set { _sideLength = value; }            // Validated and set in constructor
        }

        /// <summary>
        /// Whether the puzzle has been successfully solved
        /// </summary>
        public bool IsSolved
        {
            get
            {
                // If any Cell is not yet solved, the game is not solved
                foreach (Cell c in Cells)
                    if (!c.IsSolved)
                        return false;

                // All Cells have been solved, so the game has been solved too
                return true;
            }
        }

        /// <summary>
        /// Number of Cells in the puzzle that have been solved
        /// </summary>
        public int SolvedCells
        {
            get
            {
                // Count the number of Cells that have been solved
                int result = 0;
                foreach (Cell c in Cells)
                    if (c.IsSolved)
                        result++;

                return result;
            }
        }

        /// <summary>
        /// Deep copy of this game
        /// </summary>///
        public Game DeepClone
        {
            get
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(ms, this);              // Copy all aspects of this to a memory stream
                    ms.Position = 0;

                    return (Game)formatter.Deserialize(ms);     // Recover all copied aspects to a new instance
                }
            }
        }
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// Two-dimensional array representing a Sudoku puzzle
        /// </summary>
        /// <param name="firstValue">First value of each Cell in the Game</param>
        /// <param name="sideLength">Number of rows / columns / regions in the Game and of values in each Cell;
        ///   default 9 for standard Sudoku puzzle
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when sideLength parameter is illegally large or small</exception>
        /// <exception cref="ArgumentException">Thrown when sideLength parameter is not a perfect square</exception>
        public Game(char firstValue = '1', int sideLength = 9)
        {
            // Range validation
            if (sideLength < MinDimensionValue || sideLength > MaxDimensionValue)
                throw new ArgumentOutOfRangeException("Game constructor", string.Format(
                    "Each dimension of the grid must be at least {0:d} but no more than {1:d}",
                    MinDimensionValue, MaxDimensionValue));

            // Verify that SideLength is a perfect square, simultaneously determining square root of SideLength
            if (!Util.IsPerfectSquare(sideLength, out squareRootOfSideLength))
            {
                throw new ArgumentException("Game constructor",
                    "Each grid dimension must be a perfect square such as 4, 9, or 16");
            }

            // All OK - construct Cell[,] and instantiate internal Cells
            SideLength = sideLength;
            Cells = new Cell[SideLength, SideLength];
            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                    Cells[row, column] = new Cell(minValue: firstValue, numValues: sideLength);
        }

        /// <summary>
        /// Two-dimensional array representing a Sudoku puzzle
        /// </summary>
        /// <param name="firstValue">First value possible for each Cell in the Game</param>
        /// <param name="sideLength">Number of rows / columns / regions in the Game and of values in each Cell</param>
        /// <param name="initialValues">Array of values representing pre-solved Cells</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when sideLength parameter is illegally large or small</exception>
        /// <exception cref="ArgumentException">Thrown when sideLength parameter is not a perfect square</exception>
        /// <exception cref="ArgumentException">Thrown when the size of parameter initialValues conflicts with parameter sideLength</exception>
        /// <exception cref="ArgumentException">Thrown when a value in initialValues cannot be configured due to conflict with other values</exception>
        public Game(char firstValue, int sideLength, char[,] initialValues)
        {
            // Range validation
            if (sideLength < MinDimensionValue || sideLength > MaxDimensionValue)
                throw new ArgumentOutOfRangeException("Game constructor", string.Format(
                    "Each dimension of the grid must be at least {0:d} but no more than {1:d}",
                    MinDimensionValue,
                    MaxDimensionValue));

            // Verify that SideLength is a perfect square, simultaneously determining square root of SideLength
            if (!Util.IsPerfectSquare(sideLength, out squareRootOfSideLength))
            {
                throw new ArgumentException("Game constructor",
                    "Each grid dimension must be a perfect square such as 4, 9, or 16");
            }

            if (initialValues.GetLength(0) != sideLength || initialValues.GetLength(1) != sideLength)
                throw new ArgumentException("Game constructor",
                    "Each dimension of parameter initialValues must be the size of parameter sideLength");

            // All OK - continue with persisting parameter and instantiating internal Cells
            SideLength = sideLength;
            Cells = new Cell[SideLength, SideLength];

            // Create all Cells before specifying any values
            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                    Cells[row, column] = new Cell(minValue: firstValue, numValues: sideLength);

            // Configure known Cells
            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                    if (initialValues[row, column] != Cell.valueWhenUnsolved)
                    {
                        SetCell(row, column, initialValues[row, column]);
                        Cells[row, column].Status = CellStatus.Known;
                    }
        }
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Determine region containing a given Cell location
        /// </summary>
        /// <param name="rowNumber">Row to be matched</param>
        /// <param name="columnNumber">Column to be matched</param>
        /// <returns>Corresponding region number</returns>
        public int IdentifyRegion(int rowNumber, int columnNumber)
        {
            return squareRootOfSideLength * (rowNumber / squareRootOfSideLength)
                + (columnNumber / squareRootOfSideLength);
        }

        /// <summary>
        /// Determine Cell locations for a given Region
        /// </summary>
        /// <param name="regionNumber">Region to be matched</param>
        /// <returns>List of Cell locations</returns>
        public List<Duad> LocationsInRegion(int regionNumber)
        {
            var result = new List<Duad>();
            var firstRow = squareRootOfSideLength * (regionNumber / squareRootOfSideLength);
            var firstColumn = squareRootOfSideLength * (regionNumber % squareRootOfSideLength);

            for (var row = firstRow; row < firstRow + squareRootOfSideLength; row++)
                for (var column = firstColumn; column < firstColumn + squareRootOfSideLength; column++)
                {
                    result.Add(new Duad(row, column));
                }

            return result;
        }

        /// <summary>
        /// Find subset of Cells in a specified row
        /// </summary>
        /// <param name="rowNumber">Row to be matched</param>
        /// <returns>Cells in one row</returns>
        public List<Cell> CellsByRow(int rowNumber)
        {
            var results = new List<Cell>();
            for (var column = 0; column < SideLength; column++)
                results.Add(Cells[rowNumber, column]);

            return results;
        }

        /// <summary>
        /// Find subset of Cells in a specified column
        /// </summary>
        /// <param name="columnNumber">Column to be matched</param>
        /// <returns>Cells in one column</returns>
        public List<Cell> CellsByColumn(int columnNumber)
        {
            var results = new List<Cell>();
            for (var row = 0; row < SideLength; row++)
                results.Add(Cells[row, columnNumber]);

            return results;
        }

        /// <summary>
        /// Find subset of Cells in a specified region
        /// </summary>
        /// <param name="regionNumber">Region to be matched</param>
        /// <returns>Cells in one region</returns>
        public List<Cell> CellsByRegion(int regionNumber)
        {
            var firstRowInRegion = squareRootOfSideLength * (regionNumber / squareRootOfSideLength);
            var firstColumnInRegion = squareRootOfSideLength * (regionNumber % squareRootOfSideLength);

            var results = new List<Cell>();
            for (var row = firstRowInRegion; row < firstRowInRegion + squareRootOfSideLength; row++)
                for (var column = firstColumnInRegion; column < firstColumnInRegion + squareRootOfSideLength; column++)
                    results.Add(Cells[row, column]);

            return results;
        }

        /// <summary>
        /// Enter the solution to a specified Cell
        /// </summary>
        /// <param name="rowNumber">Row to be matched</param>
        /// <param name="columnNumber">Column to be matched</param>
        /// <param name="value">Value to assign to this Cell</param>
        /// <exception cref="ArgumentException">Thrown when parameter value is not a legal setting for the Cell</exception>
        public void SetCell(int rowNumber, int columnNumber, char value)
        {
            var destination = Cells[rowNumber, columnNumber];
            try
            {
                destination.Set(value);     // If this can't be done, Cell will throw an Exception
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format("Game cannot set Cell[{0:d}, {1:d}] to {2}",
                    columnNumber, rowNumber, value),
                    ex);
            }

            // Disqualify others in same row
            foreach (var c in CellsByRow(rowNumber))
                if (c != destination)
                    c.Disqualify(value);

            // Disqualify others in same column
            foreach (var c in CellsByColumn(columnNumber))
                if (c != destination)
                    c.Disqualify(value);

            // Disqualify others in same region
            foreach (var c in CellsByRegion(IdentifyRegion(rowNumber, columnNumber)))
                if (c != destination)
                    c.Disqualify(value);
        }

        /// <summary>
        /// Enter the solution to a specified Cell
        /// </summary>
        /// <param name="m">3D structure to identify potential move in a Sudoku puzzle</param>
        public void SetCell(Move move)
        {
            this.SetCell(move.Row, move.Column, move.Value);
        }

        /// <summary>
        /// Simple text representation of the Game
        /// </summary>
        /// <returns>Text in one line</returns>
        public override string ToString()
        {
            if (IsSolved)
                return "Puzzle successfully solved";
            else
                return string.Format("{0:d} of {1:d} cells solved",
                    SolvedCells,
                    SideLength * SideLength
                    );
        } 
        #endregion
    }
}