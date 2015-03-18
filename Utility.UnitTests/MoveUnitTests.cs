/**********************************************************************************
 * Unit tests for Utility Move class
 * Jeff Straw | Northwestern Michigan College
 * 02/23/2015: Initial release
 * 02/28/2015: Moved from Model to Utility
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Utility.UnitTests
{
    /// <summary>
    /// Unit tests for Utility.Move struct
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class MoveUnitTests
    {   
        #region [ Setup ]
        // Unit test configuration setup
        private Random rand = new Random();
        #endregion

        #region [ Constructor tests ]
        /// <summary>
        /// Does the new Move instantiate correctly?
        /// </summary>
        [TestMethod]
        public void Move_DefaultConstructor_InstantiationSuccessful()
        {
            var m = new Move();
            Assert.IsInstanceOfType(m, typeof(Move));
        }

        /// <summary>
        /// Do properties in a new Move instantiate correctly?
        /// </summary>
        [TestMethod]
        public void Move_DefaultConstructor_DefaultProperties()
        {
            var m = new Move();

            Assert.AreEqual(m.Row, default(int));
            Assert.AreEqual(m.Column, default(int));
            Assert.AreEqual(m.Value, default(char));
        }

        /// <summary>
        /// Do parameters in a new Move constructor configure correctly?
        /// </summary>
        [TestMethod]
        public void Move_Constructor_ParametersSet()
        {
            var m = new Move(int.MinValue, int.MaxValue, default(char), MoveType.Solution);

            Assert.AreEqual(m.Row, int.MinValue);
            Assert.AreEqual(m.Column, int.MaxValue);
            Assert.AreEqual(m.Value, default(char));
            Assert.AreEqual(m.Kind, MoveType.Solution);
        } 
        #endregion

        #region [ Property tests ]
        /// <summary>
        /// Does Row parameter in a new Move persist?
        /// </summary>
        [TestMethod]
        public void Move_RowParameter_Persists()
        {
            var m = new Move();
            var stimulus = rand.Next();
            m.Row = stimulus;

            var expected = stimulus;
            var actual = m.Row;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does Column parameter in a new Move persist?
        /// </summary>
        [TestMethod]
        public void Move_ColumnParameter_Persists()
        {
            var m = new Move();
            var stimulus = rand.Next();
            m.Column = stimulus;

            var expected = stimulus;
            var actual = m.Column;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does Value parameter in a new Move persist?
        /// </summary>
        [TestMethod]
        public void Move_ValueParameter_Persists()
        {
            var m = new Move();
            var stimulus = (char)rand.Next(32, 127);
            m.Value = stimulus;

            var expected = stimulus;
            var actual = m.Value;
            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region [ Method tests ]
        /// <summary>
        /// Does default ToString match design plan?
        /// </summary>
        [TestMethod]
        public void Move_ToString_Valid()
        {
            var row = rand.Next();
            var column = rand.Next();
            var value = (char)rand.Next(32, 127);
            var kind = (MoveType)rand.Next(2);

            var m = new Move(row, column, value, kind);

            var expected =
                "(" + row.ToString("d") + ", " + column.ToString("d") + ")"
                + ": '" + value.ToString() + "'"
                + " (" + kind.ToString("g") + ")";
            var actual = m.ToString();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}