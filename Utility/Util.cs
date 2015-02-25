/******************************************************************************
 * Simple sharable static calculations
 * Jeff Straw | Northwestern Michigan College
 * 02/20/2015: Initial release
 *****************************************************************************/
using System;
using System.Drawing;
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
    }
}