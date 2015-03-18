/**************************************************************************
 * Individual cell for Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College 
 * 02/07/2015 (approx): development began
 * 02/14/2015: converted from ints and bytes to chars
 * 02/19/2015: added constructor for pre-known Cell
 * 02/20/2015: removed check for perfect square, refactored
 * 02/23/2015: Serializable; changed IList to List
 * 02/27/2015: added ValueAtIndex and IndexOfValue, ScratchPad
 * 03/03/2015: added Row/Column/Region to simplify solving algorithms
 *************************************************************************/
using System;
using System.Collections.Generic;

namespace Sudoku.Model
{
    /// <summary>
    /// Individual Sudoku location on game grid
    /// </summary>
    [Serializable]
    public class Cell
    {
        #region [ Public fields ]
        /// <summary>
        /// Value that represents a Cell that has not yet been solved
        /// </summary>
        public static readonly char valueWhenUnsolved = ' ';    // Intentionally public 
        #endregion

        #region [ Private fields ]
        private char _currentValue = valueWhenUnsolved;
        private int _row;
        private int _column;
        private int _region;
        private char _minValue;
        private char _maxValue;
        private List<char> _remainingPossibilities;
        private List<char> _scratchPad;
        #endregion

        #region [ Properties ]
        /// <summary>
        /// Solution, if applicable, or valueWhenUnsolved if not solved yet
        /// </summary>
        public char CurrentValue
        {
            get { return _currentValue; }
            private set { _currentValue = value; }
        }
        
        /// <summary>
        /// Vertical location in Sudoku puzzle grid
        /// </summary>
        public int Row
        {
            get { return _row; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Row", "Must be be non-negative");
                if (value > MaxValue - MinValue)
                    throw new ArgumentOutOfRangeException("Row",
                        string.Format("Must be no larger than {0:d}", MaxValue - MinValue));
                _row = value; 
            }
        }

        /// <summary>
        /// Horizontal location in Sudoku puzzle grid
        /// </summary>
        public int Column
        {
            get { return _column; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Column", "Must be be non-negative");
                if (value > MaxValue - MinValue)
                    throw new ArgumentOutOfRangeException("Column",
                        string.Format("Must be no larger than {0:d}", MaxValue - MinValue));
                _column = value;
            }
        }

        /// <summary>
        /// Square zone location in Sudoku puzzle grid
        /// </summary>
        public int Region
        {
            get { return _region; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Region", "Must be be non-negative");
                if (value > MaxValue - MinValue)
                    throw new ArgumentOutOfRangeException("Region",
                        string.Format("Must be no larger than {0:d}", MaxValue - MinValue));
                _region = value;
            }
        }
        
        
        /// <summary>
        /// Smallest legal value for this cell
        /// </summary>
        public char MinValue
        {
            get { return _minValue; }
            private set { _minValue = value; }
        }

        /// <summary>
        /// Largest legal value for this cell
        /// </summary>
        public char MaxValue
        {
            get { return _maxValue; }
            private set { _maxValue = value; }
        }

        /// <summary>
        /// Determine if a unique solution has been determined
        /// </summary>
        public bool IsSolved
        {
            get { return CurrentValue != valueWhenUnsolved; }
        }

        /// <summary>
        /// Whether this cell still has hope of being solved
        /// (makes no claim regarding full game solvability)
        /// </summary>
        public bool Solvable
        {
            // If all potential values have been disqualified, Cell can't be solved
            get { return (RemainingPossibilities.Count >= 1); }
        }

        /// <summary>
        /// Measure of source (and therefore certainty) of answer
        /// </summary>
        public CellStatus Status { get; set; }

        /// <summary>
        /// Ordered list of values this Cell can contain
        /// </summary>
        public List<char> RemainingPossibilities
        {
            get { return _remainingPossibilities; }
            // Set this property via constructor, modify with IList methods
            private set { _remainingPossibilities = value; }
        }

        /// <summary>
        /// Ordered list representation of all possibilities, including disqualified ones
        /// </summary>
        public List<char> Domain
        {
            get
            {
                var results = new List<char>();

                // Investigate each possibility, representing it with still-valid value or with valueWhenUnsolved
                for (var possibility = MinValue; possibility <= MaxValue; possibility++)
                {
                    if (RemainingPossibilities.Contains(possibility))
                        results.Add(possibility);
                    else
                        results.Add(valueWhenUnsolved);           // Represent disqualified possibility
                }

                return results;
            }
        }

        /// <summary>
        /// Ordered list representation of potential value guesses, initially all off.
        /// </summary>
        public List<char> ScratchPad
        {
            get { return _scratchPad; }
            private set { _scratchPad = value; }
        }
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// Individual Sudoku location
        /// </summary>
        /// <param name="minValue">Smallest legal value for this cell; default 1 for standard Sudoku puzzle</param>
        /// <param name="numValues">Quantity of legal values for this cell; default 9 for standard Sudoku puzzle</param>
        /// <param name="row">Vertical location in Sudoku puzzle grid</param>
        /// <param name="column">Horizontal location in Sudoku puzzle grid</param>
        /// <param name="region">Square zone location in Sudoku puzzle grid</param>
        public Cell(char minValue, int numValues, int row = 0, int column = 0, int region = 0)
        {
            MinValue = minValue;
            MaxValue = (char)((int)minValue + numValues - 1);

            // Identify location on the Game grid
            Row = row;
            Column = column;
            Region = region;

            // Start with sequential series of all possible solutions
            RemainingPossibilities = new List<char>();
            for (var c = MinValue; c <= MaxValue; c++)
                RemainingPossibilities.Add(c);

            // Start with declaration that all scratches are unknown
            ScratchPad = new List<char>();
            for (var c = MinValue; c <= MaxValue; c++)
                ScratchPad.Add(valueWhenUnsolved);

            // Declare default status - that the value has not yet been determined
            Status = CellStatus.Unknown;
        }

        /// <summary>
        /// Individual Sudoku location
        /// </summary>
        /// <param name="value">Pre-known solution for this Cell</param>
        /// <param name="minValue">Smallest legal value for this cell; default 1 for standard Sudoku puzzle</param>
        /// <param name="numValues">Quantity of legal values for this cell; default 9 for standard Sudoku puzzle</param>
        /// <param name="row">Vertical location in Sudoku puzzle grid</param>
        /// <param name="column">Horizontal location in Sudoku puzzle grid</param>
        /// <param name="region">Square zone location in Sudoku puzzle grid</param>
        public Cell(char value, char minValue, int numValues, int row = 0, int column = 0, int region = 0)
        {
            MinValue = minValue;
            MaxValue = (char)((int)minValue + numValues - 1);

            // Identify location on the Game grid
            Row = row;
            Column = column;
            Region = region;

            // Start with sequential series of all possible solutions
            RemainingPossibilities = new List<char>();
            for (var c = MinValue; c <= MaxValue; c++)
                RemainingPossibilities.Add(c);

            // Start with declaration that all scratches are unknown
            ScratchPad = new List<char>();
            for (var c = MinValue; c <= MaxValue; c++)
                RemainingPossibilities.Add(valueWhenUnsolved);

            // Range validation
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("value",
                    string.Format("Value ({0}) must be between {1} and {2}", value, MinValue, MaxValue));

            // Save state of this Cell
            Set(value);                         // Declare the solved value of this Cell
            RemainingPossibilities =
                new List<char>() { value };     // Declare that the solution represents all possible values
            Status = CellStatus.Known;          // Mark the status of this Cell as being certain
        }
        #endregion

        #region [ Methods ]
        /// <summary>
        /// Determine if a potential solution value is still possible
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="value">Value to check</param>
        public bool IsPotentialSolution(char value)
        {
            // Range validation
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("IsPotentialSolution",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    value, MinValue, MaxValue));

            // True if not yet disqualified
            return RemainingPossibilities.Contains(value);
        }

        /// <summary>
        /// Remove a potential solution value from consideration
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="value">Value to exclude from consideration</param>
        public void Disqualify(char value)
        {
            // Range validation
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("Disqualify",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    value, MinValue, MaxValue));

            // Remove if value is currently qualified; ignore (benignly) if previously disqualified
            if (RemainingPossibilities.Contains(value))
                RemainingPossibilities.Remove(value);
        }

        /// <summary>
        /// Identify a particular location as a potential solution location (not declaring so, just "guessing")
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="index">Zero-based index of location to say "maybe"</param>
        public void AddScratch(int index)
        {
            // Range validation
            if (index < 0 || index > (MaxValue - MinValue))
                throw new ArgumentOutOfRangeException("AddScratch",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    index, 0, (MaxValue - MinValue)));

            ScratchPad[index] = ValueAtIndex(index);
         }

        /// <summary>
        /// Identify a particular value as a potential solution (not declaring so, just "guessing")
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="value">Value to say "maybe"</param>
        public void AddScratch(char value)
        {
            // Range validation
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("AddScratch",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    value, MinValue, MaxValue));

            ScratchPad[IndexOfValue(value)] = value;
        }

        /// <summary>
        /// Identify a particular location as unlikely as a potential solution location (not declaring, just "guessing")
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="index">Zero-based index of location to say "probably not"</param>
        public void RemoveScratch(int index)
        {
            // Range validation
            if (index < 0 || index > (MaxValue - MinValue))
                throw new ArgumentOutOfRangeException("AddScratch",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    index, 0, (MaxValue - MinValue)));

            ScratchPad[index] = valueWhenUnsolved;
        }

        /// <summary>
        /// Identify a particular value as unlikely as a potential solution (not declaring, just "guessing")
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="value">Value to say "probably not"</param>
        public void RemoveScratch(char value)
        {
            // Range validation
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("AddScratch",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    value, MinValue, MaxValue));

            ScratchPad[IndexOfValue(value)] = valueWhenUnsolved;
        }

        /// <summary>
        /// Enter the solution to this Cell
        /// </summary>
        /// <exception cref="ApplicationException">Thrown when all possible solutions have been disqualified</exception>
        /// <exception cref="ApplicationException">Thrown when argument has been previously disqualified</exception>
        /// <param name="value">Correct value for this Cell</param>
        public void Set(char value)
        {
            // Check whether prior disqualifications have caused an unsolvable situation
            if (!Solvable)
                throw new ApplicationException("This puzzle cannot be solved - all possibilities have been disqualified");

            // Verify that the specified choice has not itself been disqualified earlier
            if (!IsPotentialSolution(value))
                throw new ApplicationException(string.Format(
                    "{0} is not valid for this Cell - only {1} allowed",
                    value,
                    string.Join(", ", this.RemainingPossibilities)));

            // Save value as the unique solution for this Cell
            CurrentValue = value;

            // Disqualify all other choices - once a value is set, nothing else is possible
            // (foreach won't work - InvalidOperation is thrown due to changes to collection)
            for (int index = RemainingPossibilities.Count - 1; index >= 0; index-- )
                if (RemainingPossibilities[index] != value)
                    RemainingPossibilities.RemoveAt(index);
        }

        /// <summary>
        /// Identify the sole potential value that could be valid at the given location
        /// </summary>
        /// /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="index">Zero-based index of value</param>
        /// <returns>Value that could be legal at this index</returns>
        public char ValueAtIndex(int index)
        {
            // Range validation
            if (index < 0 || index > (MaxValue - MinValue))
                throw new ArgumentOutOfRangeException("ValueAtIndex",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    index, 0, (MaxValue - MinValue)));

            return (char)((int)MinValue + index);
        }

        /// <summary>
        /// Identify the index where a specified value will be legal
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when argument is too small or too large</exception>
        /// <param name="value">Value that could be a solution for this Cell</param>
        /// <returns>Zero-based index where value might be legal</returns>
        public int IndexOfValue(char value)
        {
            // Range validation
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException("IndexOfValue",
                    string.Format("Argument ({0:d}) must be between {1} and {2}",
                    value, MinValue, MaxValue));

            return value - MinValue;
        }

        /// <summary>
        /// Text representation of this Cell
        /// </summary>
        /// <returns>Solution if solved, valueWhenUnsolved if not solved</returns>
        public override string ToString()
        {
            return CurrentValue.ToString();
        }
        #endregion
    }
}