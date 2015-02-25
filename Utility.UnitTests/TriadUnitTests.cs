/**********************************************************************************
 * Unit tests for Utility Triad class
 * Jeff Straw | Northwestern Michigan College
 * 02/18/2015: Initial release
 * 02/23/2015: Added methods
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Utility.UnitTests
{
    /// <summary>
    /// Unit tests for Utility.Triad struct
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TriadUnitTests
    {   
        #region [ Setup ]
        // Unit test configuration setup
        private Random rand = new Random();
        #endregion

        #region [ Constructor tests ]
        /// <summary>
        /// Does the new Triad instantiate correctly?
        /// </summary>
        [TestMethod]
        public void Triad_DefaultConstructor_InstantiationSuccessful()
        {
            var t = new Triad();
            Assert.IsInstanceOfType(t, typeof(Triad));
        }

        /// <summary>
        /// Do properties in a new Triad instantiate correctly?
        /// </summary>
        [TestMethod]
        public void Triad_DefaultConstructor_DefaultProperties()
        {
            var t = new Triad();

            Assert.AreEqual(t.Row, default(int));
            Assert.AreEqual(t.Column, default(int));
            Assert.AreEqual(t.HintZone, default(int));
        }

        /// <summary>
        /// Do parameters in a new Triad constructor configure correctly?
        /// </summary>
        [TestMethod]
        public void Triad_Constructor_ParametersSet()
        {
            var t = new Triad(int.MinValue, int.MaxValue, default(int));

            Assert.AreEqual(t.Row, int.MinValue);
            Assert.AreEqual(t.Column, int.MaxValue);
            Assert.AreEqual(t.HintZone, default(int));
        } 
        #endregion

        #region [ Property tests ]
        /// <summary>
        /// Does Row parameter in a new Triad persist?
        /// </summary>
        [TestMethod]
        public void Triad_RowParameter_Persists()
        {
            var t = new Triad();
            var stimulus = rand.Next();
            t.Row = stimulus;

            var expected = stimulus;
            var actual = t.Row;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does Column parameter in a new Triad persist?
        /// </summary>
        [TestMethod]
        public void Triad_ColumnParameter_Persists()
        {
            var t = new Triad();
            var stimulus = rand.Next();
            t.Column = stimulus;

            var expected = stimulus;
            var actual = t.Column;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does HintZone parameter in a new Triad persist?
        /// </summary>
        [TestMethod]
        public void Triad_HintZoneParameter_Persists()
        {
            var t = new Triad();
            var stimulus = rand.Next();
            t.HintZone = stimulus;

            var expected = stimulus;
            var actual = t.HintZone;
            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region [ Method tests ]
        /// <summary>
        /// Does default ToString match design plan?
        /// </summary>
        [TestMethod]
        public void Triad_ToString_Valid()
        {
            var row = rand.Next();
            var column = rand.Next();
            var hintZone = rand.Next();

            var t = new Triad(row, column, hintZone);

            var expected = "(" + row.ToString("d") + ", " + column.ToString("d") + ", " + hintZone.ToString("d") + ")";
            var actual = t.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does ToString match design plan when HintZone is included?
        /// </summary>
        [TestMethod]
        public void Triad_ToStringParam_IncludingHint()
        {
            var row = rand.Next();
            var column = rand.Next();
            var hintZone = rand.Next();

            var t = new Triad(row, column, hintZone);

            var expected = "Row: " + row.ToString("d") + ", Col: " + column.ToString("d") + ", Hint: " + hintZone.ToString("d");
            var actual = t.ToString(hintZone - 1);      // Intentionally does not match HintZone value
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does ToString match design plan when HintZone is excluded?
        /// </summary>
        [TestMethod]
        public void Triad_ToStringParam_ExcludingHint()
        {
            var row = rand.Next();
            var column = rand.Next();
            var hintZone = rand.Next();

            var t = new Triad(row, column, hintZone);

            var expected = "Row: " + row.ToString("d") + ", Col: " + column.ToString("d");
            var actual = t.ToString(hintZone);          // Intentionally matches HintZone value
            Assert.AreEqual(expected, actual);
        } 
        #endregion
    }
}