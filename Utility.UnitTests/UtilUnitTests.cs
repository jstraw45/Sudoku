/**********************************************************************************
 * Unit tests for Utility Util class
 * Jeff Straw | Northwestern Michigan College
 * 02/20/2015: Initial release
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Utility.UnitTests
{
    /// <summary>
    /// Unit tests for Utility.Util members
    /// </summary>
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UtilUnitTests
    {
        #region [ Setup ]
        Random rand = new Random(); 
        #endregion

        #region [ IsPerfectSquare tests ]
        /// <summary>
        /// Is a perfect square correctly evaluated? Is the calculated square root correct?
        /// </summary>
        [TestMethod()]
        public void Util_IsPerfectSquare_ValidRandomPerfectSquare()
        {
            var stimulus = rand.Next(46341);        // 46,340 is largest int whose square doesn't overflow Int32

            int actualSqrt;
            var actualBool = Util.IsPerfectSquare(stimulus * stimulus, out actualSqrt);

            // Did the method declare that the perfect square is indeed one?
            Assert.IsTrue(actualBool);
            // Did the square root calculation work correctly?
            Assert.AreEqual(stimulus, actualSqrt);
        }

        /// <summary>
        /// Does an imperfect square return false? (No need for Exception)
        /// </summary>
        [TestMethod()]
        public void Util_IsPerfectSquare_ImperfectSquare()
        {
            var imperfect = 11;

            int actualSqrt;
            var actualBool = Util.IsPerfectSquare(imperfect, out actualSqrt);

            // Did the method declare that the imperfect square is not a perfect square?
            Assert.IsFalse(actualBool);
            // Did the method return zero for imperfect square?
            Assert.AreEqual(0, actualSqrt);
        }

        /// <summary>
        /// Does a negative value return false? (No need for Exception)
        /// </summary>
        [TestMethod()]
        public void Util_IsPerfectSquare_NegativeSource()
        {
            var stimulus = rand.Next(46341);        // 46,340 is largest int whose square doesn't overflow Int32

            int actualSqrt;
            var actualBool = Util.IsPerfectSquare(stimulus * stimulus * -1, out actualSqrt);

            // Did the method declare that the perfect square is indeed one?
            Assert.IsFalse(actualBool);
            // Did the method return zero for non-solvable problem?
            Assert.AreEqual(0, actualSqrt);
        } 
        #endregion

        #region [ PointFromDuad tests ]
        /// <summary>
        /// Did parameters swap correctly?
        /// </summary>
        [TestMethod()]
        public void Util_PointFromDuad_RandomValid()
        {
            var row = rand.Next();
            var column = rand.Next();

            var stimulus = new Duad(row, column);
            var result = Util.PointFromDuad(stimulus);

            Assert.AreEqual(result.X, stimulus.Column);
            Assert.AreEqual(result.Y, stimulus.Row);
        } 
        #endregion

        #region [ PointFFromDuadF tests ]
        /// <summary>
        /// Did parameters swap correctly?
        /// </summary>
        [TestMethod()]
        public void Util_PointFFromDuadF_RandomValid()
        {
            var row = (float)rand.NextDouble();
            var column = (float)rand.NextDouble();

            var stimulus = new DuadF(row, column);
            var result = Util.PointFFromDuadF(stimulus);

            Assert.AreEqual(result.X, stimulus.Column);
            Assert.AreEqual(result.Y, stimulus.Row);
        } 
        #endregion

        #region [ IntFromFP tests ]
        /// <summary>
        /// Does float round to correct int?
        /// </summary>
        [TestMethod()]
        public void Util_IntFromFP_FloatValid()
        {
            // Round down?
            var stimulus = 123.499f;    // Should round down
            var expected = (int)Math.Floor(stimulus) + (stimulus % 1 >= 0.5f ? 1 : 0);
            var actual = Util.IntFromFP(stimulus);
            Assert.AreEqual(expected, actual);

            // Round up?
            stimulus = 123.500f;          // Should round up
            expected = (int)Math.Floor(stimulus) + (stimulus % 1 >= 0.5f ? 1 : 0);
            actual = Util.IntFromFP(stimulus);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does double round to correct int?
        /// </summary>
        [TestMethod()]
        public void Util_IntFromFP_DoubleValid()
        {
            // Round down?
            var stimulus = 123.499;     // Should round down
            var expected = (int)Math.Floor(stimulus) + (stimulus % 1 >= 0.5 ? 1 : 0);
            var actual = Util.IntFromFP(stimulus);
            Assert.AreEqual(expected, actual);

            // Round up?
            stimulus = 123.500;           // Should round up
            expected = (int)Math.Floor(stimulus) + (stimulus % 1 >= 0.5 ? 1 : 0);
            actual = Util.IntFromFP(stimulus);
            Assert.AreEqual(expected, actual);
        } 
        #endregion
    }
}