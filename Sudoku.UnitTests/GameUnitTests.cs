﻿/**********************************************************************************
 * Unit tests for grid of a Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College
 * 02/06/2015: Initial release
 * 02/14/2015: Cell value switched to char type
 * 02/23/2015: DeepClone added
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Utility;

namespace Sudoku.Model.UnitTests
{
    /// <summary>
    /// Unit tests for Sudoku.Model.Grid
    /// </summary>
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GameUnitTests
    {
        #region [ Configuration ]
        // Unit test configuration setup
        private static byte SideLength = 9;             // Try with 4, 9, 16, 25 (25 should currently fail several tests)
        private byte ImperfectSquare = 8;               // An integral number that is NOT a perfect square
        private static char LowestPuzzleValue = '1';    // Try with 'A' for counts over 9
        private static int PuzzleValueCount = 9;        // Try with 1, 4, 9, 16, 25
        private Random rand = new Random();             // Random number generator used in tests
        private char[,] emptyStimulus = new char[SideLength, SideLength];
        private char[,] sparseStimulus = new char[SideLength, SideLength];
        private Game threeParameterGrid;
        private static char LowestValInSample4x4Game = '1';
        private static char[,] sample4x4Game = new char[4, 4]
        {
            {' ', '3', '1', ' '},                       // Do not modify - tests depend on this pattern
            {' ', ' ', ' ', '3'},
            {'4', ' ', ' ', ' '},
            {' ', '2', '4', ' '}
        };

        /// <summary>
        /// Unit tests for Sudoku.Model.Grid
        /// </summary>
        public GameUnitTests ()
	    {
            // Constructor - setup...
            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                {
                    emptyStimulus[column, row] = Cell.valueWhenUnsolved;
                    sparseStimulus[column, row] = Cell.valueWhenUnsolved;
                }
            sparseStimulus[0, 0] = LowestPuzzleValue;   // Set just one location
            threeParameterGrid = new Game(LowestPuzzleValue, SideLength, emptyStimulus);
        }
        #endregion

        #region [ Constructor tests ]
        /// <summary>
        /// Does the new Grid instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Game_Constructor2Param_Valid()
        {
            var g = new Game(LowestPuzzleValue, SideLength);

            // Class correct type?
            Assert.IsInstanceOfType(g, typeof(Game));

            // Constructor's parameters persist?
            var expected = LowestPuzzleValue;
            var actual = g.Cells[0, 0].RemainingPossibilities[0];
            Assert.AreEqual(expected, actual);
            var expectedInt = SideLength;
            var actualInt = g.SideLength;
            Assert.AreEqual(expectedInt, actualInt);
        }

        /// <summary>
        /// Does the new Grid reject non-perfect-square argument?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Game_Constructor2Param_ImperfectSquare()
        {
            var g = new Game(LowestPuzzleValue, ImperfectSquare);                      // Should throw Exception immediately
        }

        /// <summary>
        /// Does the new Grid refuse argument too small?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Game_Constructor2Param_ArgumentTooSmall()
        {
            var g = new Game(LowestPuzzleValue, (Game.MinDimensionValue - 1));   // Should throw Exception immediately
        }

        /// <summary>
        /// Does the new Grid refuse argument too large?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Game_Constructor2Param_ArgumentTooLarge()
        {
            var g = new Game(LowestPuzzleValue, (Game.MaxDimensionValue + 1));   // Should throw Exception immediately
        }

        /// <summary>
        /// Is the two-dimensional array created correctly?
        /// </summary>
        [TestMethod()]
        public void Game_Constructor2Param_CellsOfValidType()
        {
            var g = new Game(LowestPuzzleValue, SideLength);

            // Array correct type?
            Assert.IsInstanceOfType(g.Cells, typeof(Cell[,]));

            // Individual cells correct type?
            var standardCell = new Cell(LowestPuzzleValue, PuzzleValueCount);
            for (var column = SideLength - 1; column >= 0; column--)
                for (var row = SideLength - 1; row >= 0; row--)
                    Assert.IsInstanceOfType(g.Cells[row, column], typeof(Cell));
        }

        /// <summary>
        /// Does the new Grid instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Game_Constructor3Param_ValidGrid()
        {
            // Class correct type?
            Assert.IsInstanceOfType(threeParameterGrid, typeof(Game));

            // Constructor's parameters persist?
            var expected = LowestPuzzleValue;
            var actual = threeParameterGrid.Cells[0, 0].RemainingPossibilities[0];
            Assert.AreEqual(expected, actual);
            var expectedInt = SideLength;
            var actualInt = threeParameterGrid.SideLength;
            Assert.AreEqual(expectedInt, actualInt);
            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                {
                    var expectedChar = Cell.valueWhenUnsolved;
                    var actualChar = threeParameterGrid.Cells[column, row].CurrentValue;
                    Assert.AreEqual(expectedChar, actualChar);
                }
        }

        /// <summary>
        /// Does the new Grid instantiate known Cells correctly?
        /// </summary>
        [TestMethod()]
        public void Game_Constructor3Param_ValidKnownCell()
        {
            var threeParameterGrid = new Game(LowestPuzzleValue, SideLength, sparseStimulus);
            var expected = LowestPuzzleValue;
            var actual = threeParameterGrid.Cells[0, 0].CurrentValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does the new Grid reject non-perfect-square argument?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Game_Constructor3Param_ImperfectSquare()
        {
            var g = new Game(LowestPuzzleValue, ImperfectSquare, emptyStimulus);     // Should throw Exception immediately
        }

        /// <summary>
        /// Does the new Grid refuse argument too small?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Game_Constructor3Param_ArgumentTooSmall()
        {
            var g = new Game(LowestPuzzleValue, (Game.MinDimensionValue - 1), emptyStimulus);   // Should throw Exception immediately
        }

        /// <summary>
        /// Does the new Grid refuse argument too large?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Game_Constructor3Param_ArgumentTooLarge()
        {
            var g = new Game(LowestPuzzleValue, (Game.MaxDimensionValue + 1), emptyStimulus);   // Should throw Exception immediately
        }

        /// <summary>
        /// Does the new Grid refuse mismatched arguments?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Game_Constructor3Param_ArgumentDimensionsMismatched()
        {
            var g = new Game('1', 4, new Char[9,9]);   // Should throw Exception immediately - 4x4 and 9x9 are different
        }

        /// <summary>
        /// Is the two-dimensional array created correctly?
        /// </summary>
        [TestMethod()]
        public void Game_Constructor3Param_CellsOfValidType()
        {
            var g = new Game(LowestPuzzleValue, SideLength, emptyStimulus);

            // Array correct type?
            Assert.IsInstanceOfType(g.Cells, typeof(Cell[,]));

            // Individual cells correct type?
            var standardCell = new Cell(LowestPuzzleValue, PuzzleValueCount);
            for (var column = SideLength - 1; column >= 0; column--)
                for (var row = SideLength - 1; row >= 0; row--)
                    Assert.IsInstanceOfType(g.Cells[row, column], typeof(Cell));
        }
        #endregion

        #region [ Property tests ]
        /// <summary>
        /// Is the count of correctly solved Cells right?
        /// </summary>
        [TestMethod()]
        public void Game_SolvedCells_Correct()
        {
            // Using fixed 4x4 puzzle for simplicity
            char[,] sampleSolution      // Sample 4x4 Sudoku values solution
                = { { '1', '2', '3', '4' }, { '3', '4', '1', '2' }, { '2', '3', '4', '1' }, { '4', '1', '2', '3' } };

            var g = new Game(LowestPuzzleValue, 4);
            for (var row = 0; row < 4; row++)
                for (var column = 0; column < 4; column++)
                    g.Cells[row, column].Set(sampleSolution[row, column]);

            var expected = 4 * 4;       // Count of Cells in 4x4 puzzle
            var actual = g.SolvedCells;

            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region [ Method tests ]
        /// <summary>
        /// Is the region for a Cell correctly identified?
        /// </summary>
        [TestMethod()]
        public void Game_IdentifyRegion_Correct()
        {
            // Test lower-left in 4 x 4 Grid
            var g = new Game(LowestPuzzleValue, 4);
            var expected = 2;
            var actual = g.IdentifyRegion(2, 0);
            Assert.AreEqual(expected, actual);

            // Test middle-right in 9 x 9 Grid
            g = new Game(LowestPuzzleValue, 9);
            expected = 5;
            actual = g.IdentifyRegion(3, 8);
            Assert.AreEqual(expected, actual);

            // Test near center of 16 x 16 Grid
            g = new Game(LowestPuzzleValue, 16);
            expected = 9;
            actual = g.IdentifyRegion(9, 5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Do rows and columns of identified cells match the specified region?
        /// </summary>
        [TestMethod()]
        public void Game_LocationsInRegion_RandomRegionValid()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var selectedRegion = rand.Next(SideLength);            // Select region randomly

            // Test backwards - check that the region identified by each cell matches original
            foreach (var c in g.LocationsInRegion(selectedRegion))
                Assert.AreEqual(g.IdentifyRegion(c.Row, c.Column), selectedRegion);
        }

        /// <summary>
        /// Does CellsByRow(random) match the same row of the Grid?
        /// </summary>
        [TestMethod()]
        public void Game_CellsByRow_RandomRowValid()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var selectedRow = rand.Next(SideLength);            // Select row randomly

            var expected = new Cell[SideLength];
            for (var column = 0; column < SideLength; column++)
                expected[column] = g.Cells[selectedRow, column];
            var actual = g.CellsByRow((byte)selectedRow);

            CollectionAssert.AreEquivalent(expected, actual);   // Checks same contents, any sequence
        }

        /// <summary>
        /// Does CellsByColumn(random) match the same column of the Grid?
        /// </summary>
        [TestMethod()]
        public void Game_CellsByColumn_RandomColumnValid()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var selectedColumn = rand.Next(SideLength);         // Select column randomly

            var expected = new Cell[SideLength];
            for (var row = 0; row < SideLength; row++)
                expected[row] = g.Cells[row, selectedColumn];
            var actual = g.CellsByColumn((byte)selectedColumn);

            CollectionAssert.AreEquivalent(expected, actual);   // Checks same contents, any sequence
        }

        /// <summary>
        /// Does CellsByRegion(random) match the same region of the Grid?
        /// </summary>
        [TestMethod()]
        public void Game_CellsByRegion_RandomRegionValid()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var regionDimension = Convert.ToByte(Math.Round(Math.Sqrt(SideLength), 0));
            // Make sure integral square root worked
            Assert.AreEqual(SideLength, regionDimension * regionDimension);
            // Pick a region randomly
            var selectedRegion = rand.Next(SideLength);
            var rowOffset = (selectedRegion / regionDimension) * regionDimension;
            var columnOffset = (selectedRegion % regionDimension) * regionDimension;

            var expected = new List<Cell>();
            for (var row = 0; row < regionDimension; row++)
                for (var column = 0; column < regionDimension; column++)
                    expected.Add(g.Cells[row + rowOffset, column + columnOffset]);
            var actual = g.CellsByRegion((byte)selectedRegion);

            CollectionAssert.AreEquivalent(expected, actual);   // Checks same contents, any sequence - mixing array and List
        }

        /// <summary>
        /// Does setting a Cell value correctly disqualify others in same row, column, and region?
        /// <remarks>Depends on Cell.IdentifyRegion, tested elsewhere</remarks>
        /// </summary>
        [TestMethod()]
        public void Game_SetCell_ValidDisqualificationVerification()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var selectedRow = rand.Next(SideLength);            // Select row randomly
            var selectedColumn = rand.Next(SideLength);         // Select column randomly
            var selectedRegion = g.IdentifyRegion((byte)selectedColumn, (byte)selectedRow);
            var selectedCell = g.Cells[selectedRow, selectedColumn];
            var testValue = selectedCell.MaxValue;

            g.SetCell((byte)selectedRow, (byte)selectedColumn, testValue);

            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                {
                    var region = g.IdentifyRegion((byte)column, (byte)row);
                    var cell = g.Cells[row, column];
                    if (row == selectedRow || column == selectedColumn || region == selectedRegion)
                    {
                        if (cell == selectedCell)
                            Assert.AreEqual(cell.CurrentValue, testValue);
                        else
                            Assert.IsFalse(cell.RemainingPossibilities.Contains(testValue));
                    }
                    else
                        Assert.IsTrue(cell.RemainingPossibilities.Contains(testValue));
                }
        }

        /// <summary>
        /// Does setting a Cell value correctly disqualify others in same row, column, and region?
        /// <remarks>Depends on Cell.IdentifyRegion, tested elsewhere</remarks>
        /// </summary>
        [TestMethod()]
        public void Game_SetCellFromMoveObject_ValidDisqualificationVerification()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var selectedRow = rand.Next(SideLength);            // Select row randomly
            var selectedColumn = rand.Next(SideLength);         // Select column randomly
            var selectedRegion = g.IdentifyRegion((byte)selectedColumn, (byte)selectedRow);
            var selectedCell = g.Cells[selectedRow, selectedColumn];
            var testValue = selectedCell.MaxValue;

            g.SetCell(new Move((int)selectedRow, (int)selectedColumn, testValue));

            for (var row = 0; row < SideLength; row++)
                for (var column = 0; column < SideLength; column++)
                {
                    var region = g.IdentifyRegion((byte)column, (byte)row);
                    var cell = g.Cells[row, column];
                    if (row == selectedRow || column == selectedColumn || region == selectedRegion)
                    {
                        if (cell == selectedCell)
                            Assert.AreEqual(cell.CurrentValue, testValue);
                        else
                            Assert.IsFalse(cell.RemainingPossibilities.Contains(testValue));
                    }
                    else
                        Assert.IsTrue(cell.RemainingPossibilities.Contains(testValue));
                }
        }

        /// <summary>
        /// Does attempting to set a Cell to a disqualified value throw an Exception
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Game_SetCell_DisqualifiedValue()
        {
            var g = new Game(LowestPuzzleValue, SideLength);
            var selectedRow = rand.Next(SideLength);            // Select row randomly
            var selectedColumn = rand.Next(SideLength);         // Select column randomly
            var selectedRegion = g.IdentifyRegion((byte)selectedColumn, (byte)selectedRow);
            var selectedCell = g.Cells[selectedRow, selectedColumn];
            var testValue = selectedCell.MinValue;

            g.Cells[(byte)selectedRow, (byte)selectedColumn].Disqualify(testValue);
            g.SetCell((byte)selectedRow, (byte)selectedColumn, testValue);  // This should throw Exception
        }

        /// <summary>
        /// Is the text representation of the Grid correct for a solved 4x4 puzzle?
        /// </summary>
        [TestMethod()]
        public void Game_ToString_SolvedValid()
        {
            // Using fixed 4x4 puzzle for simplicity
            char[,] sampleSolution      // Sample 4x4 Sudoku values solution
                = { { '1', '2', '3', '4' }, { '3', '4', '1', '2' }, { '2', '3', '4', '1' }, { '4', '1', '2', '3' } };

            var g = new Game(LowestPuzzleValue, 4);
            for (var row = 0; row < 4; row++)
                for (var column = 0; column < 4; column++)
                    g.Cells[row, column].Set(sampleSolution[row, column]);

            var expected = "Puzzle successfully solved";
            var actual = g.ToString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Is the clone equivalent?
        /// </summary>
        [TestMethod()]
        public void Game_DeepClone_SameValues()
        {
            var orig = new Game(LowestValInSample4x4Game, 4, sample4x4Game);
            var clone = orig.DeepClone;

            for (var row = 0; row < 4; row++)
                for (var column = 0; column < 4; column++)
                {
                    Assert.AreEqual(
                        orig.Cells[row, column].CurrentValue,
                        clone.Cells[row, column].CurrentValue);
                    CollectionAssert.AreEquivalent(
                        orig.Cells[row, column].RemainingPossibilities,
                        clone.Cells[row, column].RemainingPossibilities);
                }
        }

        /// <summary>
        /// Is the clone distinct from the original?
        /// </summary>
        [TestMethod()]
        public void Game_DeepClone_DifferentInstances()
        {
            var orig = new Game(LowestValInSample4x4Game, 4, sample4x4Game);
            var clone = orig.DeepClone;

            orig.SetCell(0, 0, '2');
            Assert.IsTrue(orig.Cells[0, 0].IsSolved);
            Assert.IsFalse(clone.Cells[0, 0].IsSolved);
            Assert.IsFalse(orig.Cells[1, 0].IsPotentialSolution('2'));  // Disqualified - same region
            Assert.IsTrue(clone.Cells[1, 0].IsPotentialSolution('2'));  // Still qualified
        }

        /// <summary>
        /// Is the text representation of the Grid correct for an unsolved puzzle?
        /// </summary>
        [TestMethod()]
        public void Game_ToString_UnSolvedValid()
        {
            var g = new Game(LowestPuzzleValue, SideLength);

            var expected = new Regex(@"^\d+ of \d+ cells solved$");
            var actual = g.ToString();

            StringAssert.Matches(actual, expected);
        } 
        #endregion
    }
}