/**********************************************************************************
 * Unit tests for logic used to solve a Sudoku puzzle
 * Jeff Straw | Northwestern Michigan College
 * 02/28/2015: initial release
 * 03/01/2015: added ElsewhereInGroup
 * 03/06/2015: updated ElsewhereInGroup to catch loners
 * 03/08/2015: added IntersectingExclusions
 * 03/10/2015: added SolveRecursively
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Sudoku.Model.Tests
{
    /// <summary>
    /// Unit tests for Sudoku.Model.Logic
    /// </summary>
    [TestClass()]
    public class LogicTests
    {
        #region [ Configuration ]
        Random rand = new Random();
        // Do not edit these puzzle designs - tests depend on them
        private static string sampleLevel2 =
            "7  185   8   2 9   1     78   23  1    5 7    3  46   59     2   7 5   9   869  4";
        private static string sampleLevel4 =
            "  26  9   8   7 1 6     7 543      6   3 5   2      393 4 1   7 5 8   2   6  35  ";
        #endregion

        #region [ Loners ]
        /// <summary>
        /// Are all lone values - and only those - found?
        /// <remarks>Relies on manual verification of a specific puzzle</remarks>
        /// </summary>
        [TestMethod()]
        public void Logic_Loners_Valid()
        {
            var expected = new List<Move>()
            {   new Move(2, 4, '9', MoveType.Solution),
                new Move(3, 5, '8', MoveType.Solution),
                new Move(5, 3, '9', MoveType.Solution),
                new Move(8, 1, '2', MoveType.Solution) };
            var actual = Logic.Loners(new Game('1', 9, Util.StringToCharArray(sampleLevel2)));

            CollectionAssert.AreEquivalent(expected, actual);
        } 
        #endregion

        #region [ Unique ]
        /// <summary>
        /// Are all unsolved Cells where only one Cell in a row/column/region can provide
        /// a specific value - and only those - found?
        /// <remarks>Relies on manual verification of a specific puzzle</remarks>
        /// </summary>
        [TestMethod()]
        public void Logic_Unique_Valid()
        {
            var expected = new List<Move>()
            {   new Move(0, 2, '9', MoveType.Solution),
                new Move(1, 3, '7', MoveType.Solution), 
                new Move(1, 8, '1', MoveType.Solution), 
                new Move(3, 1, '7', MoveType.Solution),
                new Move(3, 5, '8', MoveType.Solution),
                new Move(4, 4, '1', MoveType.Solution),
                new Move(6, 4, '7', MoveType.Solution),
                new Move(7, 5, '2', MoveType.Solution),
                new Move(8, 6, '7', MoveType.Solution) };

            var actual = Logic.Unique(new Game('1', 9, Util.StringToCharArray(sampleLevel2)));

            CollectionAssert.AreEquivalent(expected, actual);
        } 
        #endregion

        #region [ ElsewhereInGroup ]
        /// <summary>
        /// Are Moves that must be excluded due to requirement in other Cells in same row/column/region found?
        /// <remarks>Relies on manual verification of a specific puzzle</remarks>
        /// </summary>
        [TestMethod()]
        public void Logic_ElsewhereInGroup_Valid()
        {
            // EXCLUDE these as possibilities...
            var sampleMoves = new List<Move>()
            {
                // Loner exclusions
                new Move(2, 0, '9', MoveType.Disqualification),    // Lone 9 in row
                new Move(2, 2, '9', MoveType.Disqualification),    // Lone 9 in row
                new Move(2, 3, '9', MoveType.Disqualification),    // Lone 9 in row
                new Move(3, 1, '8', MoveType.Disqualification),    // Lone 8 in row
                new Move(3, 2, '8', MoveType.Disqualification),    // Lone 8 in row
                new Move(3, 6, '8', MoveType.Disqualification),    // Lone 8 in row
                new Move(5, 0, '9', MoveType.Disqualification),    // Lone 9 in row
                new Move(5, 2, '9', MoveType.Disqualification),    // Lone 9 in row
                new Move(5, 7, '9', MoveType.Disqualification),    // Lone 9 in row
                new Move(8, 0, '2', MoveType.Disqualification),    // Lone 2 in row
                new Move(8, 2, '2', MoveType.Disqualification),    // Lone 2 in row

                new Move(0, 1, '2', MoveType.Disqualification),    // Lone 2 in column
                new Move(4, 1, '2', MoveType.Disqualification),    // Lone 2 in column
                new Move(7, 1, '2', MoveType.Disqualification),    // Lone 2 in column
                new Move(2, 3, '9', MoveType.Disqualification),    // Lone 9 in column
                new Move(4, 4, '9', MoveType.Disqualification),    // Lone 9 in column

                new Move(2, 3, '9', MoveType.Disqualification),    // Lone 9 in region
                new Move(4, 4, '9', MoveType.Disqualification),    // Lone 9 in region
                new Move(7, 0, '2', MoveType.Disqualification),    // Lone 2 in region
                new Move(7, 1, '2', MoveType.Disqualification),    // Lone 2 in region
                new Move(8, 0, '2', MoveType.Disqualification),    // Lone 2 in region
                new Move(8, 2, '2', MoveType.Disqualification),    // Lone 2 in region

                // Row exclusions
                new Move(0, 2, '2', MoveType.Disqualification),    // Exclude due to quad (2, 3, 4, 6) at (0, 2), (0, 6), (0, 7), (0, 8)
                new Move(0, 2, '3', MoveType.Disqualification),
                new Move(0, 2, '4', MoveType.Disqualification),
                new Move(0, 2, '6', MoveType.Disqualification),
                
                new Move(1, 3, '3', MoveType.Disqualification),    // Exclude due to quad (3, 4, 5, 6) at (1, 1), (1, 2), (1, 5), (1, 7)
                new Move(1, 3, '4', MoveType.Disqualification),
                new Move(1, 3, '6', MoveType.Disqualification), 
                new Move(1, 8, '3', MoveType.Disqualification),
                new Move(1, 8, '5', MoveType.Disqualification),
                new Move(1, 8, '6', MoveType.Disqualification),

                new Move(8, 6, '1', MoveType.Disqualification),    // Exclude due to trio (1, 2, 3) at (8, 0), (8, 1), (8, 2)
                new Move(8, 6, '3', MoveType.Disqualification),
                new Move(8, 7, '3', MoveType.Disqualification),

                new Move(8, 6, '5', MoveType.Disqualification),    // Exclude due to quad (1, 2, 3, 5) at (8, 0), (8, 1), (8, 2), (8, 7)

                // Column exclusions
                new Move(1, 1, '4', MoveType.Disqualification),    // Exclude due to quad (2, 4, 6, 8) at (0, 1), (4, 1), (7, 1), (8, 1)
                new Move(1, 1, '6', MoveType.Disqualification),

                new Move(3, 1, '4', MoveType.Disqualification),    // Exclude due to quint (2, 4, 5, 6, 8) at (0, 1), (1, 1), (4, 1), (7, 1), (8, 1)
                new Move(3, 1, '5', MoveType.Disqualification),
                new Move(3, 1, '6', MoveType.Disqualification),
                new Move(3, 1, '8', MoveType.Disqualification),

                new Move(6, 4, '1', MoveType.Disqualification),    // Exclude due to pair (1, 9) at (2, 4), (4, 4)

                new Move(6, 5, '3', MoveType.Disqualification),    // Exclude due to pair (3, 4) at (1, 5), (2,5)
                new Move(6, 5, '4', MoveType.Disqualification),
                new Move(7, 5, '3', MoveType.Disqualification),
                new Move(7, 5, '4', MoveType.Disqualification),

                new Move(7, 5, '1', MoveType.Disqualification),    // Exclude due to trio (1, 3, 4) at (1, 5), (2,5), (6, 5)

                // Region exclusions
                new Move(1, 3, '3', MoveType.Disqualification),    // Exclude due to pair (3, 4) at (1, 5), (2,5)
                new Move(1, 3, '4', MoveType.Disqualification),
                new Move(2, 3, '3', MoveType.Disqualification),
                new Move(2, 3, '4', MoveType.Disqualification),
                new Move(1, 3, '6', MoveType.Disqualification),    // Exclude due to quad (3, 4, 6, 9) at (1, 5), (2, 3), (2, 4), (2,5)
                new Move(2, 3, '9', MoveType.Disqualification),    // Exclude due to trio (3, 4, 9 ) at (1, 5),  (2, 4), (2,5)

                new Move(1, 8, '3', MoveType.Disqualification),    // Exclude due to quint (2, 3, 4, 5, 6) at (0, 6), (0, 7), (0, 8), (1, 7), (2, 6)
                new Move(1, 8, '5', MoveType.Disqualification),
                new Move(1, 8, '6', MoveType.Disqualification),

                new Move(3, 1, '4', MoveType.Disqualification),    // Exclude due to hex (1, 2, 4, 5, 6, 8, 9)
                new Move(3, 1, '5', MoveType.Disqualification),
                new Move(3, 1, '6', MoveType.Disqualification),
                new Move(3, 1, '8', MoveType.Disqualification),

                new Move(4, 4, '9', MoveType.Disqualification),    // Exclude due to single (1) (5, 3)

                new Move(6, 2, '1', MoveType.Disqualification),    // Exclude due to trio (1, 2, 3) at (8, 0), (8, 1), (8, 2)
                new Move(6, 2, '3', MoveType.Disqualification),
                new Move(7, 0, '1', MoveType.Disqualification),
                new Move(7, 0, '2', MoveType.Disqualification),
                new Move(7, 0, '3', MoveType.Disqualification),
                new Move(7, 1, '2', MoveType.Disqualification),

                new Move(7, 5, '1', MoveType.Disqualification),    // Exclude due to quad(1, 3, 4, 7) at (6, 3), (6, 4), (6, 5), (7, 3) 
                new Move(7, 5, '3', MoveType.Disqualification),
                new Move(7, 5, '4', MoveType.Disqualification) };

            var expected = sampleMoves.Distinct().OrderBy(r => r.Row).ThenBy(c => c.Column).ThenBy(v => v.Value).ToList();
            // var expected = sampleMoves.OrderBy(r => r.Row).ThenBy(c => c.Column).ThenBy(v => v.Value).ToList();
            var actual = Logic.ElsewhereInGroup(new Game('1', 9, Util.StringToCharArray(sampleLevel2)));

            CollectionAssert.AreEquivalent(expected, actual);
        } 
        #endregion

        #region [ IntersectingExclusions ]
        /// <summary>
        /// Are Moves that must be excluded due to intersection constraints between regions and rows/columns found?
        /// <remarks>Relies on manual verification of a specific puzzle</remarks>
        /// </summary>
        [TestMethod()]
        public void Logic_IntersectingExclusions_Valid()
        {
            // EXCLUDE these as possibilities...
            var expected = new List<Move>()
            {
                // Excluded by 1 in (1, 8)
                new Move(6, 8, '1', MoveType.Disqualification),

                // Excluded by 2 in (7, 5)
                new Move(7, 0, '2', MoveType.Disqualification),
                new Move(7, 1, '2', MoveType.Disqualification),

                // Excluded by 7 in (3, 1)
                new Move(3, 6, '7', MoveType.Disqualification),
                new Move(3, 8, '7', MoveType.Disqualification),

                // Excluded by 7 in (6, 4)
                new Move(6, 3, '7', MoveType.Disqualification),
                new Move(6, 6, '7', MoveType.Disqualification),
                new Move(6, 8, '7', MoveType.Disqualification),

                // Excluded by 8 in (3, 5)
                new Move(3, 1, '8', MoveType.Disqualification),
                new Move(3, 2, '8', MoveType.Disqualification),
                new Move(3, 6, '8', MoveType.Disqualification),

                // Excluded by 1 in (4, 4)
                new Move(4, 0, '1', MoveType.Disqualification),
                new Move(4, 2, '1', MoveType.Disqualification),
                new Move(6, 4, '1', MoveType.Disqualification),

                // Excluded by 9's in (2, 3) and (2, 4)
                new Move(2, 0, '9', MoveType.Disqualification),    
                new Move(2, 2, '9', MoveType.Disqualification),

                // Excluded by 9's in (4, 0) and (4, 2)
                new Move(4, 0, '9', MoveType.Disqualification),
                new Move(5, 0, '9', MoveType.Disqualification),
                new Move(4, 2, '9', MoveType.Disqualification),
                new Move(5, 2, '9', MoveType.Disqualification)
            };

            var actual = Logic.IntersectingExclusions(new Game('1', 9, Util.StringToCharArray(sampleLevel2)));
            CollectionAssert.AreEquivalent(expected, actual);
        } 
        #endregion

        #region [ SolveRecursively ]
        /// <summary>
        /// Are Moves that must be excluded due to intersection constraints between regions and rows/columns found?
        /// <remarks>Relies on manual verification of a specific puzzle</remarks>
        /// </summary>
        [TestMethod()]
        public void Logic_SolveRecursively_Valid()
        {
            var g = new Game('1', 9, Util.StringToCharArray(sampleLevel4));

            // Sample puzzle should be solvable, requiring algorithms through IntersectingExclusions
            var process = new List<Func<Game, List<Move>>>();
            process.Add(Logic.Loners);  
            process.Add(Logic.Unique);
            process.Add(Logic.ElsewhereInGroup);
            process.Add(Logic.IntersectingExclusions);

            Assert.IsFalse(g.IsSolved);             // Before attempt to solve
            Logic.SolveRecursively(process, 0, g);
            Assert.IsTrue(g.IsSolved);              // After attempt to solve
        }
        #endregion

        #region [ Logical equivalence after scrambling, rotating, and flipping ]
        /// <summary>
        /// Do puzzles with different char scrambling solve similarly?
        /// </summary>
        [TestMethod()]
        public void Logic_SolveRecursively_ScrambleCharsValid()
        {
            var origA = new Game('1', 9, Util.StringToCharArray(sampleLevel4));
            var origB = new Game('1', 9, Util.ScrambleChars(Util.StringToCharArray(sampleLevel4), Cell.valueWhenUnsolved));

            var first = origA.DeepClone;
            var second = origB.DeepClone;

            // Initially equivalent
            AssertCellEquality(first, second);

            var process = new List<Func<Game, List<Move>>>();
            process.Add(Logic.Loners);

            // Attempt to solve
            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.Unique);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.ElsewhereInGroup);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.IntersectingExclusions);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);
        }

        /// <summary>
        /// Do puzzles with different rotation solve similarly?
        /// </summary>
        [TestMethod()]
        public void Logic_SolveRecursively_RotateClockwiseValid()
        {
            var origA = new Game('1', 9, Util.StringToCharArray(sampleLevel4));
            var origB = new Game('1', 9, Util.RotateClockwise(Util.StringToCharArray(sampleLevel4), rand.Next()));

            var first = origA.DeepClone;
            var second = origB.DeepClone;

            // Initially equivalent
            AssertCellEquality(first, second);

            var process = new List<Func<Game, List<Move>>>();
            process.Add(Logic.Loners);

            // Attempt to solve
            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.Unique);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.ElsewhereInGroup);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.IntersectingExclusions);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);
        }

        /// <summary>
        /// Do puzzles with different horizontal mirroring solve similarly?
        /// </summary>
        [TestMethod()]
        public void Logic_SolveRecursively_FlipHorizontalValid()
        {
            var origA = new Game('1', 9, Util.StringToCharArray(sampleLevel4));
            var origB = new Game('1', 9, Util.FlipHorizontal(Util.StringToCharArray(sampleLevel4)));

            var first = origA.DeepClone;
            var second = origB.DeepClone;

            // Initially equivalent
            AssertCellEquality(first, second);

            var process = new List<Func<Game, List<Move>>>();
            process.Add(Logic.Loners);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.Unique);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.ElsewhereInGroup);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.IntersectingExclusions);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);

            AssertCellEquality(first, second);
        }

        /// <summary>
        /// Do puzzles with different vertical mirroring solve similarly?
        /// </summary>
        [TestMethod()]
        public void Logic_SolveRecursively_FlipVerticalValid()
        {
            var origA = new Game('1', 9, Util.StringToCharArray(sampleLevel4));
            var origB = new Game('1', 9, Util.FlipVertical(Util.StringToCharArray(sampleLevel4)));

            var first = origA.DeepClone;
            var second = origB.DeepClone;

            // Initially equivalent
            AssertCellEquality(first, second);

            var process = new List<Func<Game, List<Move>>>();
            process.Add(Logic.Loners);

            // Attempt to solve
            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.Unique);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.ElsewhereInGroup);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);

            // Deeper algorithm
            first = origA.DeepClone;
            second = origB.DeepClone;
            process.Add(Logic.IntersectingExclusions);

            Logic.SolveRecursively(process, 0, first);
            Logic.SolveRecursively(process, 0, second);
            AssertCellEquality(first, second);
        }

        /// <summary>
        /// Determine logical equivalence of two Games
        /// </summary>
        /// <param name="first">A Sudoku game in process of being solved</param>
        /// <param name="second">Another Sudoku game in process of being solved</param>
        private static void AssertCellEquality(Game first, Game second)
        {
            Assert.AreEqual(first.IsSolved, second.IsSolved);
            Assert.AreEqual(first.SolvedCells, second.SolvedCells);
            Assert.AreEqual(first.Difficulty, second.Difficulty);
            Assert.AreEqual(first.StepCount, second.StepCount);
        } 
        #endregion
    }
}