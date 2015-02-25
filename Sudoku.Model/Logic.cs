/****************************************************************
 * Soduko game-solving logic methods
 * Jeff Straw | Northwestern Michigan College 
 * 02/23/2015: prototype
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Sudoku.Model
{
    /// <summary>
    /// Soduko game-solving logic methods
    /// </summary>
    /// <remarks>May want to exclude from Unit testing - no independent way to verify?</remarks>
    // [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Logic
    {
        #region [ Loners ]
        /// <summary>
        /// Find all unsolved Cells where all but one possibilities have been disqualified
        /// </summary>
        /// <param name="game">Two-dimensional array representing a Sudoku puzzle</param>
        /// <returns>List of Moves that should be taken during this pass</returns>
        public static List<Move> Loners(Game game)
        {
            var result = new List<Move>();
            for (var row = 0; row < game.SideLength; row++)
                for (var column = 0; column < game.SideLength; column++)
                {
                    var cell = game.Cells[row, column];
                    // Unsolved; only one possibility left
                    if (!cell.IsSolved && cell.RemainingPossibilities.Count == 1)
                        result.Add(new Move(row, column, cell.RemainingPossibilities[0]));
                }

            return result;
        } 
        #endregion

        #region [ Unique ]
        /// <summary>
        /// Find all unsolved Cells where only one Cell in a row/column/region can provide a specific value
        /// </summary>
        /// <param name="game">Two-dimensional array representing a Sudoku puzzle</param>
        /// <returns>List of Moves that should be taken during this pass</returns>
        public static List<Move> Unique(Game game)
        {
            var result = new List<Move>();
            var validAll = new List<char>();                    // All potential valid values
            for (var c = game.Cells[0, 0].MinValue; c <= game.Cells[0, 0].MaxValue; c++)
                validAll.Add(c);

            // Find Cells within rows that uniquely represent individual values
            for (var row = 0; row < game.SideLength; row++)
            {
                // Exclude solved values
                var valid = validAll.Select(c => c).ToList();   // Clone the list generated earlier
                foreach (var cell in game.CellsByRow(row))
                    if (cell.IsSolved)
                        valid.Remove(cell.CurrentValue);

                // Count quantities of each value
                var counts = new Dictionary<char, int>();
                foreach (var cell in game.CellsByRow(row))
                    if (!cell.IsSolved)
                        foreach (var val in cell.RemainingPossibilities)
                            if (counts.Keys.Contains(val))
                                counts[val]++;
                            else
                                counts[val] = 1;

                // Identify quantities of one
                foreach (var uniqueValue in counts.Where(c => c.Value == 1).Select(c => c.Key).ToList())
                    // Create move for each of these locations / values - must find grid location now
                    for (var column = 0; column < game.SideLength; column++)
                        if (game.Cells[row, column].RemainingPossibilities.Contains(uniqueValue))
                            result.Add(new Move(row, column, uniqueValue));
            }

            // Find Cells within columns that uniquely represent individual values
            for (var column = 0; column < game.SideLength; column++)
            {
                // Exclude solved values
                var valid = validAll.Select(c => c).ToList();   // Clone the list generated earlier
                foreach (var cell in game.CellsByColumn(column))
                    if (cell.IsSolved)
                        valid.Remove(cell.CurrentValue);

                // Count quantities of each value
                var counts = new Dictionary<char, int>();
                foreach (var cell in game.CellsByColumn(column))
                    if (!cell.IsSolved)
                        foreach (var val in cell.RemainingPossibilities)
                            if (counts.Keys.Contains(val))
                                counts[val]++;
                            else
                                counts[val] = 1;

                // Identify quantities of one
                foreach (var uniqueValue in counts.Where(c => c.Value == 1).Select(c => c.Key).ToList())
                    // Create move for each of these locations / values - must find grid location now
                    for (var row = 0; row < game.SideLength; row++)
                        if (game.Cells[row, column].RemainingPossibilities.Contains(uniqueValue))
                            result.Add(new Move(row, column, uniqueValue));
            }

            // Find Cells within regions that uniquely represent individual values
            for (var region = 0; region < game.SideLength; region++)
            {
                // Exclude solved values
                var valid = validAll.Select(c => c).ToList();   // Clone the list generated earlier
                foreach (var cell in game.CellsByRegion(region))
                    if (cell.IsSolved)
                        valid.Remove(cell.CurrentValue);

                // Count quantities of each value
                var counts = new Dictionary<char, int>();
                foreach (var cell in game.CellsByRegion(region))
                    if (!cell.IsSolved)
                        foreach (var val in cell.RemainingPossibilities)
                            if (counts.Keys.Contains(val))
                                counts[val]++;
                            else
                                counts[val] = 1;

                // Identify quantities of one
                foreach (var uniqueValue in counts.Where(c => c.Value == 1).Select(c => c.Key).ToList())
                    // Create move for each of these locations / values - must find grid location now
                    foreach (var location in game.LocationsInRegion(region))
                        if (game.Cells[location.Row, location.Column].RemainingPossibilities.Contains(uniqueValue))
                            result.Add(new Move(location.Row, location.Column, uniqueValue));
            }

            // Remove duplicate Moves
            return result.Distinct().ToList();
        } 
        #endregion
    }
}