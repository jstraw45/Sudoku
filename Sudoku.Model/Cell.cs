/********************************************************************
 * Individual cell for Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College 
 * 02/07/2015 (approx): development began
 * 02/14/2015: converted from ints and bytes to chars
 * 02/19/2015: added constructor for pre-known Cell
 * 02/20/2015: removed check for perfect square, refactored
 * 02/23/2015: Serializable; changed IList to List
 *******************************************************************/
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
        private char _minValue;
        private char _maxValue;
        private char _currentValue = valueWhenUnsolved;       
        private List<char> _remainingPossibilities;
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
        #endregion

        #region [ Constructors ]
         /// <summary>
        /// Individual Sudoku location
        /// </summary>
        /// <param name="minValue">Smallest legal value for this cell; default 1 for standard Sudoku puzzle</param>
        /// <param name="numValues">Quantity of legal values for this cell; default 9 for standard Sudoku puzzle</param>
        public Cell(char minValue = '1', int numValues = 9)
        {
            MinValue = minValue;
            MaxValue = (char)((int)minValue + numValues - 1);

            // Start with sequential series of all possible solutions
            RemainingPossibilities = new List<char>();
            for (var c = MinValue; c <= MaxValue; c++)
                RemainingPossibilities.Add(c);

            // Declare default status - that the value has not yet been determined
            Status = CellStatus.Unknown;
        }

        /// <summary>
        /// Individual Sudoku location
        /// </summary>
        /// <param name="value">Pre-known solution for this Cell</param>
        /// <param name="minValue">Smallest legal value for this cell; default 1 for standard Sudoku puzzle</param>
        /// <param name="numValues">Quantity of legal values for this cell; default 9 for standard Sudoku puzzle</param>
        public Cell(char value, char minValue = '1', int numValues = 9)
        {
            MinValue = minValue;
            MaxValue = (char)((int)minValue + numValues - 1);

            // Start with sequential series of all possible solutions
            RemainingPossibilities = new List<char>();
            for (var c = MinValue; c <= MaxValue; c++)
                RemainingPossibilities.Add(c);
            MaxValue = (char)((int)minValue + numValues - 1);

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