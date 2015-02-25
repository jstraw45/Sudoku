/**********************************************************************************
 * Unit tests for individual cell of a Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College
 * 02/06/2015: Initial release
 * 02/14/2015: Cell value switched to char type
 * 02/19/2015: Added tests for 3-param constructor
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku.Model;
using System;
using System.Collections.Generic;

namespace Sudoku.Model.UnitTests
{
    /// <summary>
    /// Unit tests for Sudoku.Model.Cell
    /// </summary>
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CellUnitTests
    {
        #region [ Setup ]
        // Unit test configuration setup
        private static char LowestPuzzleValue = '1';            // Try with 'A' for counts over 9
        private static int PuzzleValueCount = 9;                // Try with 1, 4, 9, 16, 25
        private char HighestPuzzleValue = (char)((int)LowestPuzzleValue + PuzzleValueCount - 1);
        private static char DefaultLowestPuzzleValue = '1';
        private static int DefaultPuzzleValueCount = 9;
        #endregion

        #region [ Constructor tests ]
        /// <summary>
        /// Does the new Cell instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Cell_Constructor_InstantiationSuccessful()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            // Correct type?
            Assert.IsInstanceOfType(c, typeof(Cell));

            // Constructor's parameters persist?
            var expected = LowestPuzzleValue;
            var actual = c.MinValue;
            Assert.AreEqual(expected, actual);

            expected = HighestPuzzleValue;
            actual = c.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does the new Cell instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Cell_Constructor_InstantiationNoParamsSuccessful()
        {
            var c = new Cell();

            // Correct type?
            Assert.IsInstanceOfType(c, typeof(Cell));

            // Constructor's parameters persist?
            var expected = DefaultLowestPuzzleValue;
            var actual = c.MinValue;
            Assert.AreEqual(expected, actual);

            expected = (char)((int)DefaultLowestPuzzleValue + DefaultPuzzleValueCount - 1);
            actual = c.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does the new Cell contain all feasible answers?
        /// </summary>
        [TestMethod()]
        public void Cell_Constructor_AllPossibilitiesRemain()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            var expected = new List<char>();
            for (var possibility = LowestPuzzleValue; possibility <= HighestPuzzleValue; possibility++)
                expected.Add(possibility);
            var actual = c.RemainingPossibilities as List<char>;

            //CollectionAssert.AreEquivalent(expected, actual);   // Checks same contents, any sequence
            CollectionAssert.AreEqual(expected, actual);        // Check same contents, same sequence
            Assert.IsTrue(c.Solvable);                          // Check 
        }

        /// <summary>
        /// Are there possible solutions that have not been disqualified?
        /// </summary>
        [TestMethod()]
        public void Cell_Constructor_IsSolvable()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);
            Assert.IsTrue(c.Solvable);                          // Can this new Cell be solved?
        }

        /// <summary>
        /// Is the new Cell correctly marked as answer unknown?
        /// </summary>
        [TestMethod()]
        public void Cell_Constructor_CurrentValueStateValid()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);
            var actual = c.CurrentValue;

            Assert.IsFalse(c.IsSolved);                         // Should not know value if multiple possibilities exist
        }

        /// <summary>
        /// Does the new Cell instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Cell_ConstructorSolved_InstantiationSuccessful()
        {
            var c = new Cell(DefaultLowestPuzzleValue, DefaultLowestPuzzleValue, DefaultPuzzleValueCount);

            // Correct type?
            Assert.IsInstanceOfType(c, typeof(Cell));

            // Constructor's parameters persist?
            var expected = DefaultLowestPuzzleValue;
            var actual = c.MinValue;
            Assert.AreEqual(expected, actual);

            expected = (char)((int)DefaultLowestPuzzleValue + DefaultPuzzleValueCount - 1);
            actual = c.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does an out-of-range parameter throw correct Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Cell_ConstructorSolved_BadParameter()
        {
            var stimulus = (char)((int)LowestPuzzleValue - 1);  // Out of range - below lowest legal
            var c = new Cell(stimulus, DefaultLowestPuzzleValue, DefaultPuzzleValueCount);  // should throw Exception
        }
        #endregion

        #region [ Method tests ]
        /// <summary>
        /// Are valid possibilities correctly identified?
        /// </summary>
        [TestMethod()]
        public void Cell_IsPotentialSolution_Valid()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            // All possibilities are initially included
            for (var possibility = LowestPuzzleValue; possibility <= HighestPuzzleValue; possibility++)
                Assert.IsTrue(c.IsPotentialSolution(possibility));
        }

        /// <summary>
        /// Does an out-of-range value throw an Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Cell_IsPotentialSolution_TooLow()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            var shouldThrowException = c.IsPotentialSolution((char)((int)LowestPuzzleValue - 1));
        }

        /// <summary>
        /// Does an out-of-range value throw an Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Cell_IsPotentialSolution_TooHigh()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            var shouldThrowException = c.IsPotentialSolution((char)((int)HighestPuzzleValue + 1));
        }

        /// <summary>
        /// Does Disqualify remove the value from consideration?
        /// </summary>
        [TestMethod()]
        public void Cell_Disqualify_SuccessfullyRemovesPossibility()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            // Disqualified possibilities are excluded
            c.Disqualify(LowestPuzzleValue);
            c.Disqualify(HighestPuzzleValue);

            Assert.IsFalse(c.IsPotentialSolution(LowestPuzzleValue));
            Assert.IsFalse(c.IsPotentialSolution(HighestPuzzleValue));
        }

        /// <summary>
        /// Does Disqualify throw exception if value is too low?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Cell_Disqualify_TooLow()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            // Disqualified possibilities are excluded
            c.Disqualify((char)((int)LowestPuzzleValue - 1));
        }

        /// <summary>
        /// Does Disqualify throw exception if value is too high?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Cell_Disqualify_TooHigh()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            // Disqualified possibilities are excluded
            c.Disqualify((char)((int)HighestPuzzleValue + 1));
        }

        /// <summary>
        /// Can an in-range value be successfully set?
        /// </summary>
        [TestMethod()]
        public void Cell_Set_Valid()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);
            c.Set(HighestPuzzleValue);

            var expected = HighestPuzzleValue;
            var actual = c.CurrentValue;

            Assert.AreEqual(expected, actual);                      // Primary aspect
            Assert.IsTrue(c.Solvable);                              // Additional aspect
            Assert.IsTrue(c.RemainingPossibilities.Count == 1);     // Additional aspect
        }

        /// <summary>
        /// Does an out-of-range value throw an Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Cell_Set_TooHigh()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            c.Set((char)((int)HighestPuzzleValue + 1));      // This should throw Exception
        }

        /// <summary>
        /// Does a disqualified possibility throw an Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ApplicationException))]
        public void Cell_Set_Disqualified()
        {
            // Use a 4-unit Cell for simplicity
            var c = new Cell('1', 4);
            c.Disqualify('3');
            c.Set('3');      // This should throw Exception
        }

        /// <summary>
        /// Does an unsolvable Cell throw an Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ApplicationException))]
        public void Cell_Set_Unsolvable()
        {
            // Use a 4-unit Cell for simplicity
            var c = new Cell('1', 4);
            for (var potentialValue = '1'; potentialValue <= '4'; potentialValue++)
                c.Disqualify(potentialValue);

            c.Set((char)((int)HighestPuzzleValue + 1));      // This should throw Exception
        }

        /// <summary>
        /// Does Clue properly represent legal square puzzles
        /// </summary>
        [TestMethod()]
        public void Cell_Clue_Valid()
        {
            var c = new Cell('1', 1);
            var expected = new List<char>()
            {
                '1'
            };
            var actual = c.Domain as List<char>;
            CollectionAssert.AreEqual(expected, actual);   // Checks same contents, same sequence

            c = new Cell('1', 4);
            expected = new List<char>()
            {
                '1', '2', '3', '4'
            };
            actual = c.Domain as List<char>;
            CollectionAssert.AreEqual(expected, actual);   // Checks same contents, same sequence

            c = new Cell('1', 9);
            expected = new List<char>()
            {
                '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };
            actual = c.Domain as List<char>;
            CollectionAssert.AreEqual(expected, actual);   // Checks same contents, same sequence

            // Check that disqualified possibilities are replaced with blanks
            c.Disqualify('3');
            c.Disqualify('5');
            c.Disqualify('7');
            expected = new List<char>()
            {
                '1', '2', ' ', '4', ' ', '6', ' ', '8', '9'
            };
            actual = c.Domain as List<char>;
            CollectionAssert.AreEqual(expected, actual);   // Checks same contents, same sequence
        }

        /// <summary>
        /// Is string representation of unsolved Cell correct?
        /// </summary>
        [TestMethod()]
        public void Cell_ToString_ValidUnsolved()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);

            var expected = " ";
            var actual = c.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Is string representation of solved Cell correct?
        /// </summary>
        [TestMethod()]
        public void Cell_ToString_ValidSolved()
        {
            var c = new Cell(LowestPuzzleValue, PuzzleValueCount);
            c.Set(LowestPuzzleValue);

            var expected = LowestPuzzleValue.ToString();
            var actual = c.ToString();

            Assert.AreEqual(expected, actual);
        } 
        #endregion
    }
}