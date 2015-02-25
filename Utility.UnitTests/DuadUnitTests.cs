/**********************************************************************************
 * Unit tests for Utility Duad class
 * Jeff Straw | Northwestern Michigan College
 * 02/18/2015: Initial release
 * 02/23/2015: Added methods
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Utility.UnitTests
{
    /// <summary>
    /// Unit tests for Utility.Duad struct
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DuadUnitTests
    {
        #region [ Setup ]
        // Unit test configuration setup
        private Random rand = new Random();
        #endregion

        #region [ Constructor tests ]
        /// <summary>
        /// Does the new Duad instantiate correctly?
        /// </summary>
        [TestMethod]
        public void Duad_DefaultConstructor_InstantiationSuccessful()
        {
            var t = new Duad();
            Assert.IsInstanceOfType(t, typeof(Duad));
        }

        /// <summary>
        /// Do properties in a new Duad instantiate correctly?
        /// </summary>
        [TestMethod]
        public void Duad_DefaultConstructor_DefaultProperties()
        {
            var t = new Duad();

            Assert.AreEqual(t.Row, default(int));
            Assert.AreEqual(t.Column, default(int));
        }

        /// <summary>
        /// Do parameters in a new Duad constructor configure correctly?
        /// </summary>
        [TestMethod]
        public void Duad_Constructor_ParametersSet()
        {
            var t = new Duad(int.MinValue, int.MaxValue);

            Assert.AreEqual(t.Row, int.MinValue);
            Assert.AreEqual(t.Column, int.MaxValue);
        } 
        #endregion

        #region [ Property tests ]
        /// <summary>
        /// Does Row parameter in a new Duad persist?
        /// </summary>
        [TestMethod]
        public void Duad_RowParameter_Persists()
        {
            var t = new Duad();
            var stimulus = rand.Next();
            t.Row = stimulus;

            var expected = stimulus;
            var actual = t.Row;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does Column parameter in a new Duad persist?
        /// </summary>
        [TestMethod]
        public void Duad_ColumnParameter_Persists()
        {
            var t = new Duad();
            var stimulus = rand.Next();
            t.Column = stimulus;

            var expected = stimulus;
            var actual = t.Column;
            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region [ Method tests ]
        /// <summary>
        /// Does default ToString match design plan?
        /// </summary>
        [TestMethod]
        public void Duad_ToString_Valid()
        {
            var row = rand.Next();
            var column = rand.Next();
  
            var d = new Duad(row, column);

            var expected = "(" + row.ToString("d") + ", " + column.ToString("d") + ")";
            var actual = d.ToString();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}