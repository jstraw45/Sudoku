/*************************************************************************
 * Transformation and scaling utilities for Sudoku puzzle user interface
 * Jeff Straw | Northwestern Michigan College 
 * 02/22/2015: initial release
 ************************************************************************/
using System;
using System.Drawing;
using Utility;

namespace Sudoku.UI.Utility
{
    /// <summary>
    /// Represent coordinates in multiple views
    /// </summary>
    public class Transform
    {
        #region [ Business rules ]
        // Aesthetic and usability specifications
        public int HintZoneUnknown = -1;                // Flag to declare that no hint zone is applicable
        public static int PuzzlePadding = 1;            // Minimum gap around game inside panel (with fixed aspect ratio)
        private static float hintZoneThreshold = 0.4f;  // Proximity to destination center required for identification

        // Shared calculated values used for scaling and translating
        private int gridUnitsPerCell;                   // 1 for each hint spot, one for one of two borders
        private static int numberGridUnits;             // 1 for left/top edge, add cells 
        #endregion

        #region [ Fields ]
        private int gameBreadth;                        // Number of Cells in each row, column, and region
        private int regionBreadth;                      // Linear cells in region or linear hints in cell
        private int gameSize;                           // Physical extent of puzzle grid, in pixels
        private int puzzleLeftMargin;                   // Distance from left edge of container to top-left corner of puzzle zone
        private int puzzleRightmost;                    // Distance from left edge of container to top-right corner of puzzle zone
        private int puzzleTopMargin;                    // Distance from top edge of container to top-left corner of puzzle zone
        private int puzzleBottommost;                   // Distance from top edge of container to bottom-left corner of puzzle zone

        private float pixelsPerGridUnit;                // Physical span of logical grid (which includes 1 border and hints)
        private float PixelsPerCell;                    // Physical span of puzzle cell 
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// Represent coordinates in multiple views, translating between coordinate systems
        /// </summary>
        /// <param name="puzzleSize">Number of Cells in each row, column, and region</param>
        /// <param name="puzzleDimensions">Pixel-based physical size of the puzzle as displayed</param>
        public Transform(int puzzleSize, Size puzzleDimensions)
        {
            // Configure fields, saving state
            gameBreadth = puzzleSize;         // 9 for a standard 9 x 9 puzzle
            gameSize = puzzleDimensions.Height < puzzleDimensions.Width
                ? puzzleDimensions.Height
                : puzzleDimensions.Width;               // Maximum size of minimum dimension: fixed aspect ratio
            puzzleLeftMargin = (puzzleDimensions.Width - gameSize) / 2 + PuzzlePadding;
            puzzleRightmost = puzzleLeftMargin + (gameSize - 2 * PuzzlePadding);
            puzzleTopMargin = (puzzleDimensions.Height - gameSize) / 2 + PuzzlePadding;
            puzzleBottommost = puzzleTopMargin + (gameSize - 2 * PuzzlePadding);

            // Calculate shared scale and transform factors
            Util.IsPerfectSquare(gameBreadth,           // Find square root (check for and resolve bad data elsewhere)
                out regionBreadth);                     // 3 linear cells each direction in a 9 x 9 puzzle

            // Values relating to logical resolution of the puzzle
            gridUnitsPerCell = regionBreadth + 1;       // 1 for each hint spot, one for one of two borders
            numberGridUnits = 1 + gameBreadth * gridUnitsPerCell;
            PixelsPerCell = (float)(gameSize - 2 * PuzzlePadding) / gameBreadth;    // Same for x and y
            pixelsPerGridUnit = PixelsPerCell / gridUnitsPerCell;
        } 
        #endregion

        #region [ Translate game coordinates to physical coordinates ]
        /// <summary>
        /// Find scaled and translated physical location for plotting game location
        /// </summary>
        /// <param name="gameLocation">Representation of row, column, and hintZone</param>
        /// <returns>Physical coordinates of location</returns>
        public Point PhysicalFromGame(Triad gameLocation)
        {
            if (gameLocation.HintZone == HintZoneUnknown)
                throw new ArgumentException("Hint zone unknown; cannot plot that point");

            var cellHoriz = puzzleLeftMargin + PuzzlePadding + gameLocation.Column * PixelsPerCell;
            var cellVert = puzzleTopMargin + PuzzlePadding + gameLocation.Row * PixelsPerCell;
            var hintHorizOffset = (gameLocation.HintZone % regionBreadth + 1) * pixelsPerGridUnit;
            var hintVertOffset = (gameLocation.HintZone / regionBreadth + 1) * pixelsPerGridUnit;

            return new Point(Util.IntFromFP(cellHoriz + hintHorizOffset),
                Util.IntFromFP(cellVert + hintVertOffset));
        }

        /// <summary>
        /// Find scaled and translated physical location for plotting game location
        /// </summary>
        /// <param name="gameLocation">Representation of row and column</param>
        /// <returns>Physical coordinates of beginning edge of Cell location</returns>
        public Point PhysicalFromGame(Duad gameLocation)
        {
            return new Point(
                puzzleLeftMargin + Util.IntFromFP(gameLocation.Column * PixelsPerCell),
                puzzleTopMargin + Util.IntFromFP(gameLocation.Row * PixelsPerCell));
        }

        /// <summary>
        /// Find scaled and translated physical center of a specified cell
        /// </summary>
        /// <param name="gridLocation">Point of interest in grid coordinates</param>
        /// <returns>Physical location of center of cell</returns>
        public Point PhysicalCellCenterFromGame(Duad gameLocation)
        {
            return new Point(
                puzzleLeftMargin + Util.IntFromFP(PixelsPerCell * (gameLocation.Column + 0.5f)),
                puzzleTopMargin + Util.IntFromFP(PixelsPerCell * (gameLocation.Row + 0.5F)));
        }
        #endregion

        #region [ Translate physical coordinates to game coordinates ]
        /// <summary>
        /// Find "array" subscripts corresponding to a location
        /// </summary>
        /// <param name="physical">Physical coordinates of location</param>
        /// <returns>Game-based coordinates of location</returns>
        public Triad GameFromPhysical(Point physical)
        {
            var result = new Triad();

            // Calculate row and column in floating-point (interim data)
            var row = (physical.Y - puzzleTopMargin - PuzzlePadding) / PixelsPerCell;
            var column = (physical.X - puzzleLeftMargin - PuzzlePadding) / PixelsPerCell;

            // Game Row and Column are truncated versions, excluding the last pixel
            // on the right and on the bottom and forcing non-negative
            result.Row = (int)(Math.Max(Math.Min(row, gameBreadth - 1), 0));
            result.Column = (int)(Math.Max(Math.Min(column, gameBreadth - 1), 0));

            // Default value for HintZone is out-of-range, fixed later when applicable
            result.HintZone = HintZoneUnknown;

            // Inside Cell - find floating-point representation of hint zone
            var hintVertOffset = (row - result.Row) * gridUnitsPerCell;
            var hintHorizOffset = (column - result.Column) * gridUnitsPerCell;
           
            // Nearest hint zone, integral representation. Subtract one due to offset within Cell
            var potentialVertHintZone = Util.IntFromFP(hintVertOffset);
            var potentialHorizHintZone = Util.IntFromFP(hintHorizOffset);

            // Close enough to be within hint zone (while not at leading left/top edge)?
            if (OnGame(physical)
                && Math.Abs(hintVertOffset - potentialVertHintZone) < hintZoneThreshold
                && Math.Abs(hintHorizOffset - potentialHorizHintZone) < hintZoneThreshold
                && potentialVertHintZone % gridUnitsPerCell != 0 && potentialHorizHintZone % gridUnitsPerCell != 0
                && potentialVertHintZone >= 1 && potentialHorizHintZone >= 1)
            {
                result.HintZone = (potentialVertHintZone - 1) * regionBreadth + (potentialHorizHintZone - 1);
            }
            return result;
        }

        /// <summary>
        /// Is physical point within bounds of the game grid?
        /// </summary>
        /// <param name="physical">Physical coordinates of location</param>
        /// <returns>true if the point is within game boundaries</returns>
        public bool OnGame(Point physical)
        {
            return (physical.X >= puzzleLeftMargin && physical.X <= puzzleRightmost
                && physical.Y >= puzzleTopMargin && physical.Y <= puzzleBottommost);
        }
        #endregion
    }
}