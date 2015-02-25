/**********************************************************************************
 * Unit tests for Utility DuadF class
 * Jeff Straw | Northwestern Michigan College
 * 02/18/2015: Initial release
 * 02/23/2015: Added methods
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Utility.UnitTests
{
    /// <summary>
    /// Unit tests for Utility.DuadF struct
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DuadFUnitTests
    {
        #region [ Setup ]
        // Unit test configuration setup
        private Random rand = new Random();
        #endregion

        #region [ Constructor tests ]
        /// <summary>
        /// Does the new DuadF instantiate correctly?
        /// </summary>
        [TestMethod]
        public void DuadF_DefaultConstructor_InstantiationSuccessful()
        {
            var t = new DuadF();
            Assert.IsInstanceOfType(t, typeof(DuadF));
        }

        /// <summary>
        /// Do properties in a new DuadF instantiate correctly?
        /// </summary>
        [TestMethod]
        public void DuadF_DefaultConstructor_DefaultProperties()
        {
            var t = new DuadF();

            Assert.AreEqual(t.Row, default(int));
            Assert.AreEqual(t.Column, default(int));
        }

        /// <summary>
        /// Do parameters in a new DuadF constructor configure correctly?
        /// </summary>
        [TestMethod]
        public void DuadF_Constructor_ParametersSet()
        {
            var t = new DuadF(int.MinValue, int.MaxValue);

            Assert.AreEqual(t.Row, int.MinValue);
            Assert.AreEqual(t.Column, int.MaxValue);
        } 
        #endregion

        #region [ Property tests ]
        /// <summary>
        /// Does Row parameter in a new DuadF persist?
        /// </summary>
        [TestMethod]
        public void DuadF_RowParameter_Persists()
        {
            var t = new DuadF();
            var stimulus = rand.Next();
            t.Row = stimulus;

            var expected = stimulus;
            var actual = t.Row;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does Column parameter in a new DuadF persist?
        /// </summary>
        [TestMethod]
        public void DuadF_ColumnParameter_Persists()
        {
            var t = new DuadF();
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
        public void DuadF_ToString_Valid()
        {
            var row = (float)(1000 * rand.NextDouble());
            var column = (float)(1000 * rand.NextDouble());

            var d = new DuadF(row, column);

            var expected = "(" + row.ToString("f") + ", " + column.ToString("f") + ")";
            var actual = d.ToString();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}