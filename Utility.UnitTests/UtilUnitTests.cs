/**********************************************************************************
 * Unit tests for Utility Util class
 * Jeff Straw | Northwestern Michigan College
 * 02/20/2015: Initial release
 * 02/09/2015: StringToCharArray and ScrambleChars added
 * 03/10/2015: Array mirroring tests added
 * 03/11/2015: Array randomization adjusted to pass test coverage
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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

        #region [ Combinations tests ]
        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void Util_Combinations_Valid()
        {
            // Sample data posted by author
            var actual = Util.Combinations(new[] { 1, 2, 3, 4, 5 }, 3).ToList();
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 3 }, actual[0].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 4 }, actual[1].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 5 }, actual[2].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 1, 3, 4 }, actual[3].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 1, 3, 5 }, actual[4].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 1, 4, 5 }, actual[5].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 2, 3, 4 }, actual[6].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 2, 3, 5 }, actual[7].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 2, 4, 5 }, actual[8].ToArray());
            CollectionAssert.AreEquivalent(new int[] { 3, 4, 5 }, actual[9].ToArray());
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

        #region [ StringToCharArray tests ]
        /// <summary>
        /// Does the conversion method properly convert a valid string?
        /// </summary>
        [TestMethod()]
        public void PuzzleFactory_StringToCharArray_Valid()
        {
            var expected = new char[,]
            {
                {'0', '1', '2' },
                {'3', '4', '5' },
                {'6', '7', '8' }
            };
            var actual = Util.StringToCharArray("012345678"); // Nine-character string - a perfect square
            CollectionAssert.AreEquivalent(expected, actual);
        }

        /// <summary>
        /// Does the conversion method properly throw an Exception when the parameter isn't square?
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void PuzzleFactory_StringToCharArray_Exception()
        {
            var stimulus = "01234"; // Five-character string - not a perfect square
            var response = Util.StringToCharArray(stimulus);   // Should throw Exception
        }
        #endregion

        #region [ ScrambleChars tests ]
        /// <summary>
        /// Does ScrambleChars keep the same values?
        /// </summary>
        [TestMethod()]
        public void Util_ScrambleChars_SameValues()
        {
            var ignore = '-';
            var stimulus = new char[4, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'},
                {'-', '2', '4', '-'}
            };

            var response = Util.ScrambleChars(stimulus, ignore);

            // Find all values in both stimulus and response
            var allStimuli = new List<char>();
            for (var row = 0; row < stimulus.GetLength(0); row++)
                for (var column = 0; column < stimulus.GetLength(1); column++)
                    allStimuli.Add(stimulus[row, column]);

            var allResponses = new List<char>();
            for (var row = 0; row < response.GetLength(0); row++)
                for (var column = 0; column < response.GetLength(1); column++)
                    allResponses.Add(response[row, column]);

            // Find unique values in both stimulus and response
            var uniqueStimuli = allStimuli
                .Distinct()
                .OrderBy(v => v)
                .ToArray();

            var uniqueResponses = allResponses
                .Distinct()
                .OrderBy(v => v)
                .ToArray();

            CollectionAssert.AreEquivalent(uniqueStimuli, uniqueResponses);
        }

        /// <summary>
        /// Does ScrambleChars ignore flagged chars - and only them?
        /// </summary>
        [TestMethod()]
        public void Util_ScrambleChars_ExcludeFlagged()
        {
            var ignore = '-';
            var stimulus = new char[4, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'},
                {'-', '2', '4', '-'}
            };

            var response = Util.ScrambleChars(stimulus, ignore);
            Assert.AreEqual(response[0, 0], ignore);
            Assert.AreNotEqual(response[0, 1], ignore);
            Assert.AreNotEqual(response[0, 2], ignore);
            Assert.AreEqual(response[0, 3], ignore);

            Assert.AreEqual(response[1, 0], ignore);
            Assert.AreEqual(response[1, 1], ignore);
            Assert.AreEqual(response[1, 2], ignore);
            Assert.AreNotEqual(response[1, 3], ignore);

            Assert.AreNotEqual(response[2, 0], ignore);
            Assert.AreEqual(response[2, 1], ignore);
            Assert.AreEqual(response[2, 2], ignore);
            Assert.AreEqual(response[2, 3], ignore);

            Assert.AreEqual(response[3, 0], ignore);
            Assert.AreNotEqual(response[3, 1], ignore);
            Assert.AreNotEqual(response[3, 2], ignore);
            Assert.AreEqual(response[3, 3], ignore);
        }
        #endregion

        #region [ Array mirroring and rotation tests ]
        /// <summary>
        /// Does FlipHorizontal correctly mirror a two-dimensional char array?
        /// </summary>
        [TestMethod()]
        public void Util_FlipHorizontal_Valid()
        {
            var stimulus = new char[4, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'},
                {'-', '2', '4', '-'}
            };
            var expected = new char[4, 4]
            {
                {'-', '1', '3', '-'},
                {'3', '-', '-', '-'},
                {'-', '-', '-', '4'},
                {'-', '4', '2', '-'}
            };
            var actual = Util.FlipHorizontal(stimulus);

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does FlipVertical correctly mirror a two-dimensional char array?
        /// </summary>
        [TestMethod()]
        public void Util_FlipVertical_Valid()
        {
            var stimulus = new char[4, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'},
                {'-', '2', '4', '-'}
            };
            var expected = new char[4, 4]
            {
                {'-', '2', '4', '-'},
                {'4', '-', '-', '-'},
                {'-', '-', '-', '3'},
                {'-', '3', '1', '-'}
            };
            var actual = Util.FlipVertical(stimulus);

            CollectionAssert.AreEqual(expected, actual);
        } 

        /// <summary>
        /// Does RotateClockwise by 90° correctly turn a two-dimensional char array?
        /// </summary>
        [TestMethod()]
        public void Util_RotateClockwise_90degreesValid()
        {
            var stimulus = new char[3, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'}
            };
            var expected = new char[4, 3]
            {
                {'4', '-', '-'},
                {'-', '-', '3'},
                {'-', '-', '1'},
                {'-', '3', '-'}
            };
            var actual = Util.RotateClockwise(stimulus, 1);

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does RotateClockwise by 180° correctly turn a two-dimensional char array?
        /// </summary>
        [TestMethod()]
        public void Util_RotateClockwise_180degreesValid()
        {
            var stimulus = new char[3, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'}
            };
            var expected = new char[3, 4]
            {
                {'-', '-', '-', '4'},
                {'3', '-', '-', '-'},
                {'-', '1', '3', '-'}
            };
            var actual = Util.RotateClockwise(stimulus, 2);

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does RotateClockwise by 270° correctly turn a two-dimensional char array?
        /// </summary>
        [TestMethod()]
        public void Util_RotateClockwise_270degreesValid()
        {
            var stimulus = new char[3, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'}
            };
            var expected = new char[4, 3]
            {
                {'-', '3', '-'},
                {'1', '-', '-'},
                {'3', '-', '-'},
                {'-', '-', '4'}
            };
            var actual = Util.RotateClockwise(stimulus, -1);

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Does RotateClockwise by 360° correctly leave a two-dimensional char array unchanged?
        /// </summary>
        [TestMethod()]
        public void Util_RotateClockwise_360degreesValid()
        {
            var stimulus = new char[3, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'}
            }; 
            var actual = Util.RotateClockwise(stimulus, 4);

            CollectionAssert.AreEqual(stimulus, actual);
        }

        /// <summary>
        /// Does RandomizeArray correctly return a two-dimensional char array?
        /// </summary>
        [TestMethod()]
        public void Util_RandomizeArray_Valid()
        {
            var stimulus = new char[3, 4]
            {
                {'-', '3', '1', '-'},
                {'-', '-', '-', '3'},
                {'4', '-', '-', '-'}
            };

            // Due to random nature, one or two random clauses may not be tested
            stimulus = Util.RandomizeArray(stimulus, '-');
            Assert.IsInstanceOfType(stimulus, typeof(char[,]));
        }
        #endregion
    }
}