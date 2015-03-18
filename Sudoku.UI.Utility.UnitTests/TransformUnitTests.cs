/**********************************************************************************
 * Unit tests for Sudoku.UI.Utility Transform class
 * Jeff Straw | Northwestern Michigan College
 * 02/22/2015: Initial release
 * 03/08/2015: Added wide and narrow rectangle format constructor tests
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using Utility;
namespace Sudoku.UI.Utility.UnitTests
{
    /// <summary>
    /// Unit tests for Sudoku.UI.Utility.Tranform members
    /// </summary>
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TransformUnitTests
    {        
        #region [ Configuration ]
        // Unit test configuration setup
        Transform trans;

        /// <summary>
        /// Unit tests for Sudoku.UI.Utility.Tranform members
        /// </summary>
        public TransformUnitTests()
        {
            // Instantiate transform with easy-to-calculate 100 pixels per Cell
            trans = new Transform(4, new Size(400 + 2 * Transform.PuzzlePadding, 400 + Transform.PuzzlePadding));
        }
        #endregion

        #region [ Constructor unit tests ]
        /// <summary>
        /// Instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Transform_Constructor_Instantiate()
        {
            // Instantiated in test file's constructor
            Assert.IsInstanceOfType(trans, typeof(Transform));
        }

        /// <summary>
        /// Instantiate correctly?
        /// </summary>
        [TestMethod()]
        public void Transform_Constructor_NonSquare()
        {
            var wide = new Transform(9, new Size(200, 100));
            var narrow = new Transform(9, new Size(100, 200));
            // Instantiated in test file's constructor
            Assert.IsInstanceOfType(wide, typeof(Transform));
            Assert.IsInstanceOfType(narrow, typeof(Transform));
        } 
        #endregion

        #region [ Method unit tests ]
        /// <summary>
        /// Is Triad correctly mapped to pixels?
        /// </summary>
        [TestMethod()]
        public void Transform_PhysicalFromGame_TriadValid()
        {
            var row = 1;        // Second row from the top: starts at 100 pixels
            var column = 2;     // Third column from the left: starts at 200 pixels
            var hintZone = 2;   // First hint in second row of hints - center at 33, 37
            var actual = trans.PhysicalFromGame(new Triad(row, column, hintZone));
            var expected = new Point(200 + 33 + Transform.PuzzlePadding, 100 + 67 + Transform.PuzzlePadding);
            var deltaHoriz = (int)Math.Abs(expected.X - actual.X);
            var deltaVert = (int)Math.Abs(expected.Y - actual.Y);

            // Within one pixel?
            Assert.IsTrue(deltaHoriz <= 1);
            Assert.IsTrue(deltaVert <= 1);
        }

        /// <summary>
        /// Is lack of HintZone in Triad correctly throw Exception?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Transform_PhysicalFromGame_TriadNoHintZone()
        {
            var row = 1;        // Second row from the top: starts at 100 pixels
            var column = 2;     // Third column from the left: starts at 200 pixels
            var actual = trans.PhysicalFromGame(new Triad(row, column, trans.HintZoneUnknown));
        }

        /// <summary>
        /// Is Duad correctly mapped to pixels for upper-left corner of Cell?
        /// </summary>
        [TestMethod()]
        public void Transform_PhysicalFromGame_DuadValid()
        {
            var row = 2;        // Third row from the top: starts at 200 pixels
            var column = 1;     // Second column from the left: starts at 100 pixels

            var actual = trans.PhysicalFromGame(new Duad(row, column));
            var expected = new Point(100 + Transform.PuzzlePadding, 200 + Transform.PuzzlePadding);
            var deltaHoriz = (int)Math.Abs(expected.X - actual.X);
            var deltaVert = (int)Math.Abs(expected.Y - actual.Y);

            // Within one pixel?
            Assert.IsTrue(deltaHoriz <= 1);
            Assert.IsTrue(deltaVert <= 1);
        }

        /// <summary>
        /// Is Duad correctly mapped to pixels for center of Cell?
        /// </summary>
        [TestMethod()]
        public void Transform_PhysicalCellCenterFromGame_DuadValid()
        {
            var row = 2;        // Third row from the top: starts at 200 pixels
            var column = 1;     // Second column from the left: starts at 100 pixels

            var actual = trans.PhysicalCellCenterFromGame(new Duad(row, column));
            var expected = new Point(100 + 50 + Transform.PuzzlePadding, 200 + 50 + Transform.PuzzlePadding);
            var deltaHoriz = (int)Math.Abs(expected.X - actual.X);
            var deltaVert = (int)Math.Abs(expected.Y - actual.Y);

            // Within one pixel?
            Assert.IsTrue(deltaHoriz <= 1);
            Assert.IsTrue(deltaVert <= 1);
        }

        /// <summary>
        /// Do pixels correctly map to Cell row, column, and hint zone?
        /// </summary>
        [TestMethod()]
        public void Transform_GameFromPhysical_CorrectCellAndHintZone()
        {
            // Aim for 3 1/3 of 4 across, 2 2/3 of 4 down
            var actual = trans.GameFromPhysical(new Point(300 + 33, 200 + 66));
            var expected = new Triad(2, 3, 2);

            Assert.AreEqual(expected.Row, actual.Row);
            Assert.AreEqual(expected.Column, actual.Column);
            Assert.AreEqual(expected.HintZone, actual.HintZone);
        }

        /// <summary>
        /// Do pixels correctly map to Cell row and column?
        /// </summary>
        [TestMethod()]
        public void Transform_GameFromPhysical_CorrectCellNoHintZone()
        {
            // Aim for 3 1/2 of 4 across, 2 2/3 of 4 down
            // 1/2 across is in center between hint zones, so hint zone should be undefined
            var actual = trans.GameFromPhysical(new Point(300 + 50, 200 + 66));
            var expected = new Triad(2, 3, trans.HintZoneUnknown);

            Assert.AreEqual(expected.Row, actual.Row);
            Assert.AreEqual(expected.Column, actual.Column);
            Assert.AreEqual(expected.HintZone, actual.HintZone);
        }

        /// <summary>
        /// Do physical coordinates correctly identify as in-grid / out-of-grid?
        /// </summary>
        [TestMethod()]
        public void Transform_OnGame_Valid()
        {
            Assert.IsTrue(trans.OnGame(new Point(1, 1)));
            Assert.IsTrue(trans.OnGame(new Point(400, 1)));
            Assert.IsTrue(trans.OnGame(new Point(1, 400)));
            Assert.IsTrue(trans.OnGame(new Point(400, 400)));
            Assert.IsFalse(trans.OnGame(new Point(0, 1)));
            Assert.IsFalse(trans.OnGame(new Point(401, 1)));
            Assert.IsFalse(trans.OnGame(new Point(1, 401)));
            Assert.IsFalse(trans.OnGame(new Point(401, 401)));
        }
        #endregion
    }
}