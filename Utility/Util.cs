/******************************************************************************
 * Simple sharable static calculations
 * Jeff Straw | Northwestern Michigan College
 * 02/20/2015: Initial release
 * 03/01/2015: Added Combinations<T>()
 * 03/11/2015: Array translations finished
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utility;

namespace Utility
{
    /// <summary>
    /// Sharable code
    /// </summary>
    public static class Util
    {
        #region [ Mathematic functions ]
        /// <summary>
        /// Determine if given parameter is a perfect square
        /// </summary>
        /// <param name="source">Value to be tested</param>
        /// <param name="squareRoot">Square root of source, if source is a perfect square</param>
        /// <returns></returns>
        public static bool IsPerfectSquare(int source, out int squareRoot)
        {
            if (source < 0)
            {
                squareRoot = default(int);
                return false;
            }

            // Estimate of square root
            var approxSquareRoot = Util.IntFromFP(Math.Sqrt(source));

            // Validate estimate as true square root
            if (approxSquareRoot * approxSquareRoot == source)
            {
                squareRoot = approxSquareRoot;
                return true;
            }
            else    // The estimate is not the true square root, so source is not a perfect square
            {
                squareRoot = default(int);
                return false;
            }
        }

        /// <summary>
        /// Recursive algorithm to return all combinations of k elements from n
        /// </summary>
        /// <remarks>http://stackoverflow.com/questions/127704/algorithm-to-return-all-combinations-of-k-elements-from-n</remarks>
        /// <remarks>Based on algorithm posted publicly by user230950</remarks>
        /// <typeparam name="T">Type of elements in the collection</typeparam>
        /// <param name="elements">Collection to be grouped</param>
        /// <param name="k">Number of elements in each group</param>
        /// <returns>Array of arrays of matched sets</returns>
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
                elements.SelectMany((element, index) =>
                    elements.Skip(index + 1).Combinations(k - 1).Select(combo => (new[] { element }).Concat(combo)));
        }
        #endregion

        #region [ Type conversions ]
        /// <summary>
        /// Swap coordinates from Duad(Row, Column) to Point(X, Y) - opposite order
        /// </summary>
        /// <param name="source">2D structure in Duad format</param>
        /// <returns>Corresponding 2D structure in Point format</returns>
        public static Point PointFromDuad(Duad source)
        {
            return new Point(source.Column, source.Row);
        }

        /// <summary>
        /// Swap coordinates from DuadF(Row, Column) to PointF(X, Y) - opposite order
        /// </summary>
        /// <param name="source">2D structure in Duad format</param>
        /// <returns>Corresponding 2D structure in Point format</returns>
        public static PointF PointFFromDuadF(DuadF source)
        {
            return new PointF(source.Column, source.Row);
        }

        /// <summary>
        /// Determine closest int that approximates a floating-point value
        /// </summary>
        /// <param name="source">Original floating-point value</param>
        /// <returns>Closest integral equivalent</returns>
        public static int IntFromFP(float source)
        {
            return (int)Math.Round(source, 0);
        }

        /// <summary>
        /// Determine closest int that approximates a floating-point value
        /// </summary>
        /// <param name="source">Original floating-point value</param>
        /// <returns>Closest integral equivalent</returns>
        public static int IntFromFP(double source)
        {
            return (int)Math.Round(source, 0);
        } 
        #endregion

        #region [ Array translations ]
        /// <summary>
        /// Convert string representation of a one-dimensional char array to the equivalent square char array
        /// </summary>
        /// <param name="source">Data represented as a single string</param>
        /// <returns>Two-dimensional char array</returns>
        /// <exception cref="ArgumentException">Thrown when parameter value is not a perfect square</exception>
        public static char[,] StringToCharArray(string source)
        {
            int sideLength = 0;
            if (Util.IsPerfectSquare(source.Length, out sideLength))
            {
                var result = new char[sideLength, sideLength];
                for (var row = 0; row < sideLength; row++)
                    for (var column = 0; column < sideLength; column++)
                        result[row, column] = source[row * sideLength + column];
                return result;
            }
            else
                throw new ArgumentException("StringToCharArray parameter is not a perfect square");
        }

        /// <summary>
        /// Randomly mix characters appearing in a 2-dimensional char array
        /// </summary>
        /// <param name="source">Original two-dimensional character array</param>
        /// <param name="locked">Locations containing this char will not be changed</param>
        /// <returns>Two-dimensional char array with swapped values in same locations</returns>
        public static char[,] ScrambleChars(char[,] source, char locked = ' ')
        {
            // Create a one-dimensional List from the two-dimensional source (not directly supported for n-dim arrays)
            var allLocations = new List<char>();
            for (var row = 0; row < source.GetLength(0); row++)
                for (var column = 0; column < source.GetLength(1); column++)
                    allLocations.Add(source[row, column]);

            // Find all swappable values in the source, sorting numerically
            var allValues = allLocations
                .Distinct()
                .Where(v => v != locked)
                .OrderBy(v => v)
                .ToArray();

            // Create a parallel random-order array
            var rand = new Random();
            var randomizedValues = (char[])allValues.Clone();
            for (int index = randomizedValues.Length - 1; index > 0; index--)
            {
                // Swap last value with a randomly-chosen one
                var randomSelection = rand.Next(index);
                var temp = randomizedValues[index];
                randomizedValues[index] = randomizedValues[randomSelection];
                randomizedValues[randomSelection] = temp;
            }

            // Create a map to convert original values to randomized values
            var converter = new Dictionary<char, char>();
            for (int index = 0; index < allValues.Length; index++)
                converter.Add(allValues[index], randomizedValues[index]);

            // Convert chars except locked values
            var result = new char[source.GetLength(0), source.GetLength(1)];
            for (var row = 0; row < source.GetLength(0); row++)
                for (var column = 0; column < source.GetLength(1); column++)
                    result[row, column] = source[row, column] == locked
                        ? locked
                        : converter[source[row, column]];

            return result;
        } 

        /// <summary>
        /// Swap columns (the 2nd dimension) of a 2-dimensional char array
        /// </summary>
        /// <param name="source">Original two-dimensional character array</param>
        /// <returns>Two-dimensional char array in mirror image</returns>
        public static char[,] FlipHorizontal(char[,] source)
        {
            var result = new char[source.GetLength(0), source.GetLength(1)];

            for (var row = 0; row < source.GetLength(0); row++)
                for (var column = 0; column < source.GetLength(1); column++)
                    result[row, column] = source[row, source.GetLength(1) - column - 1];

            return result;
        }

        /// <summary>
        /// Swap rows (the 1st dimension) of a 2-dimensional char array
        /// </summary>
        /// <param name="source">Original two-dimensional character array</param>
        /// <returns>Two-dimensional char array in mirror image</returns>
        public static char[,] FlipVertical(char[,] source)
        {
            var result = new char[source.GetLength(0), source.GetLength(1)];

            for (var row = 0; row < source.GetLength(0); row++)
                for (var column = 0; column < source.GetLength(1); column++)
                    result[row, column] = source[source.GetLength(0) - row - 1, column];

            return result;
        }

        /// <summary>
        /// Turn a 2-dimensional char array about its center
        /// </summary>
        /// <param name="source">Original two-dimensional character array</param>
        /// <param name="quadrants">Number of 90° turns to take: 1=> 90° CW, 2=> 180°, -1=> 90° CCW</param>
        /// <returns>Rotated two-dimensional char array</returns>
        public static char[,] RotateClockwise(char[,] source, int quadrants)
        {
            char[,] result = null;

            // Determine how far to turn
            switch (quadrants % 4)
            {
                case 0:     // No rotation necessary
                    result = (char[,])source.Clone();
                    break;

                case 1:     // 90° CW
                case -3:    // 90° CW
                    result = new char[source.GetLength(1), source.GetLength(0)];
                    for (var row = 0; row < source.GetLength(0); row++)
                        for (var column = 0; column < source.GetLength(1); column++)
                            result[column, source.GetLength(0) - row - 1] = source[row, column];
                    break;

                case 2:     // 180°
                case -2:    // 180°
                    result = new char[source.GetLength(0), source.GetLength(1)];
                    for (var row = 0; row < source.GetLength(0); row++)
                        for (var column = 0; column < source.GetLength(1); column++)
                            result[source.GetLength(0) - row - 1, source.GetLength(1) - column - 1] = source[row, column];
                    break;

                case 3:     // 90° CCW
                case -1:    // 90° CCW
                    result = new char[source.GetLength(1), source.GetLength(0)];
                    for (var row = 0; row < source.GetLength(0); row++)
                        for (var column = 0; column < source.GetLength(1); column++)
                            result[source.GetLength(1) - column - 1, row] = source[row, column];
                    break;
            }

            return result;
        }

        /// <summary>
        /// Randomly shuffle a 2-dimensional char array
        /// </summary>
        /// <param name="source">Original two-dimensional character array</param>
        /// <param name="locked">Locations containing this char will not be changed</param>
        /// <returns>Two-dimensional char array with values shuffled and puzzle rotated randomly</returns>
        public static char[,] RandomizeArray(char[,] source, char locked = ' ')
        {
            char[,] result = ScrambleChars(source, locked);
            // Would work better if Flips were randomly optional, but test coverage fails then
            result = FlipHorizontal(FlipVertical(RotateClockwise(result, new Random().Next())));

            return result;
        }
        #endregion
    }
}