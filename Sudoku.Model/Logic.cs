/********************************************************************
 * Soduko game-solving logic methods
 * Jeff Straw | Northwestern Michigan College 
 * 02/23/2015: prototype
 * 03/01/2015: added ElsewhereInGroup
 * 03/06/2015 (approx): added IntersectingExclusions
 * 03/10/2015: added Difficulty persistence
 *******************************************************************/
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
                        result.Add(new Move(row, column, cell.RemainingPossibilities[0], MoveType.Solution));
                }

            // Save complexity measure
            if (result.Count > 0)
                game.Difficulty = (int)Math.Max(game.Difficulty, 1);

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
                            result.Add(new Move(row, column, uniqueValue, MoveType.Solution));
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
                            result.Add(new Move(row, column, uniqueValue, MoveType.Solution));
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
                            result.Add(new Move(location.Row, location.Column, uniqueValue, MoveType.Solution));
            }

            // Save complexity measure
            if (result.Count > 0)
                game.Difficulty = (int)Math.Max(game.Difficulty, 2);

            // Remove duplicate Moves
            return result.Distinct().ToList();
        }
        #endregion

        #region [ ElsewhereInGroup ]
        /// <summary>
        /// Find all Moves that must be excluded due to requirement in other Cells in same row/column/region
        /// </summary>
        /// <remarks>Should be extended beyond pairs - don't know how far
        /// Example: 12/24/14 trio, 3 MUST be in second row in region since 3 CAN'T be in 
        /// second row in any other region</remarks>
        /// <param name="game">Two-dimensional array representing a Sudoku puzzle</param>
        /// <returns>List of Moves that should be excluded / disqualified during this pass</returns>
        public static List<Move> ElsewhereInGroup(Game game)
        {
            var result = new List<Move>();

            // Find subsets of Cells within rows that potentially define exclusions
            for (var row = 0; row < game.SideLength; row++)
                result.AddRange(FindExclusionsInGroup(game.CellsByRow(row).Where(x => !x.IsSolved)));

            // Find subsets of Cells within columns that potentially define exclusions
            for (var column = 0; column < game.SideLength; column++)
                result.AddRange(FindExclusionsInGroup(game.CellsByColumn(column).Where(x => !x.IsSolved)));

            // Find subsets of Cells within regions that potentially define exclusions
            for (var region = 0; region < game.SideLength; region++)
                result.AddRange(FindExclusionsInGroup(game.CellsByRegion(region).Where(x => !x.IsSolved)));

            // Save complexity measure
            if (result.Count > 0)
                game.Difficulty = (int)Math.Max(game.Difficulty, 3);

            // Remove duplicate Moves
            return result.Distinct().OrderBy(r => r.Row).ThenBy(c => c.Column).ThenBy(v => v.Value).ToList();
        }

        /// <summary>
        /// Exclude possibilities due to singles, pairs, trios, quads, quints, ... within same set of Cells
        /// </summary>
        /// <param name="candidates">Set of Cells to investigate</param>
        /// <returns>List of disqualified Moves</returns>
        private static List<Move> FindExclusionsInGroup(IEnumerable<Cell> candidates)
        {
            var result = new List<Move>();

            for (var setSize = 1; setSize < candidates.Count(); setSize++)
            {
                // Evaluate all combinations of Cells within set
                foreach (var combination in Util.Combinations<Cell>(candidates, setSize))
                {
                    var uniqueValues = combination
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    // If total count of values within combination == setSize, exclude those values from other Cells
                    // (if less rather than equal, then the puzzle isn't solvable...)
                    if (combination.Count() == uniqueValues.Count())
                    {
                        // Process all "other" cells in same set
                        foreach (var disqualifier in candidates.Except(combination))
                            // Process all disqualified values
                            foreach (var disqualVal in uniqueValues)
                                // Work with those not yet disqualified
                                if (disqualifier.RemainingPossibilities.Contains(disqualVal))
                                    result.Add(new Move(disqualifier.Row, disqualifier.Column, disqualVal, MoveType.Disqualification));
                    }
                }
            }
            return result;
        }
        #endregion

        #region [ IntersectingExclusions ]
        /// <summary>
        /// Find all Moves that must be excluded due to intersection constraints between regions and rows/columns 
        /// </summary>
        /// <param name="game">Two-dimensional array representing a Sudoku puzzle</param>
        /// <returns>List of Moves that should be excluded / disqualified during this pass</returns>
        public static List<Move> IntersectingExclusions(Game game)
        {
            var result = new List<Move>();

            // Each region (is this specific to regions - or is a more general version available for all combos?)
            for (var region = 0; region < game.SideLength; region++)
            {
                // Each intersection of row and region
                foreach (var row in game.RowsIntersectingRegion(region))
                {
                    // Any unsolved values required here, not allowed in other regions in this row?
                    var valuesHere = game.RowRegionIntersection(row, region)
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    var valuesElsewhere = game.RowExcludeRegion(row, region)
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    var valuesRequiredHere = valuesHere.Except(valuesElsewhere);

                    // Disqualify non-intersection locations for values not allowed in other region in this row
                    foreach (var potentialDisqualifyCell in game.RegionExcludeRow(row, region)
                        .Where(c => !c.IsSolved))
                        foreach (var value in valuesRequiredHere)
                            if (potentialDisqualifyCell.RemainingPossibilities.Contains(value))
                                result.Add(new Move(potentialDisqualifyCell.Row, potentialDisqualifyCell.Column, value, MoveType.Disqualification));

                    // Any unsolved values required here, not allowed in other rows in this region?
                    valuesElsewhere = game.RegionExcludeRow(row, region)
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    valuesRequiredHere = valuesHere.Except(valuesElsewhere);

                    // Disqualify non-intersection locations for values not allowed in other rows in this region
                    foreach (var potentialDisqualifyCell in game.RowExcludeRegion(row, region)
                        .Where(c => !c.IsSolved))
                        foreach (var value in valuesRequiredHere)
                            if (potentialDisqualifyCell.RemainingPossibilities.Contains(value))
                                result.Add(new Move(potentialDisqualifyCell.Row, potentialDisqualifyCell.Column, value, MoveType.Disqualification));
                }

                // Each intersection of column and region
                foreach (var column in game.ColumnsIntersectingRegion(region))
                {
                    // Any unsolved values required here, not allowed in other regions in this column?
                    var valuesHere = game.ColumnRegionIntersection(column, region)
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    var valuesElsewhere = game.ColumnExcludeRegion(column, region)
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    var valuesRequiredHere = valuesHere.Except(valuesElsewhere);

                    // Disqualify non-intersection locations for values not allowed in other region in this column
                    foreach (var potentialDisqualifyCell in game.RegionExcludeColumn(column, region)
                        .Where(c => !c.IsSolved))
                        foreach (var value in valuesRequiredHere)
                            if (potentialDisqualifyCell.RemainingPossibilities.Contains(value))
                                result.Add(new Move(potentialDisqualifyCell.Row, potentialDisqualifyCell.Column, value, MoveType.Disqualification));

                    // Any unsolved values required here, not allowed in other columns in this region?
                    valuesElsewhere = game.RegionExcludeColumn(column, region)
                        .SelectMany(c => c.RemainingPossibilities)
                        .Distinct();
                    valuesRequiredHere = valuesHere.Except(valuesElsewhere);

                    // Disqualify non-intersection locations for values not allowed in other columns in this region
                    foreach (var potentialDisqualifyCell in game.ColumnExcludeRegion(column, region)
                        .Where(c => !c.IsSolved))
                        foreach (var value in valuesRequiredHere)
                            if (potentialDisqualifyCell.RemainingPossibilities.Contains(value))
                                result.Add(new Move(potentialDisqualifyCell.Row, potentialDisqualifyCell.Column, value, MoveType.Disqualification));
                }
            }


            // Save complexity measure
            if (result.Count > 0)
                game.Difficulty = (int)Math.Max(game.Difficulty, 4);

            // Remove duplicate Moves
            return result.Distinct().OrderBy(r => r.Row).ThenBy(c => c.Column).ThenBy(v => v.Value).ToList();
        } 
        #endregion
        
        #region [ SolveRecursively ]
        /// <summary>
        /// Attempt to use all algorithms in parameter process recursively to solve a Sudoku
        /// </summary>
        /// <param name="process">Set of steps in a procedure</param>
        /// <param name="currentStep">Algorithmic step</param>
        /// <param name="game">Sudoku game object</param>
        public static void SolveRecursively(List<Func<Game, List<Move>>> process, int currentStep, Game game)
        {
            var situationBefore = game.RemainingPossibilities;

            // End recursion when done...
            if (currentStep == process.Count)
                return;

            // Implement changes found in this pass
            Logic.UpdateCellValues(process[currentStep], game);

            // Did this pass improve the puzzle's solution?
            if (game.RemainingPossibilities < situationBefore)
            {
                game.StepCount++;
                SolveRecursively(process, 0, game);
            }

            // No improvement - try a more complex algorithm
            SolveRecursively(process, currentStep + 1, game);
        }
        #endregion

        #region [ UpdateCellValues ]
        /// <summary>
        /// Attempt a solving step that might identify solved or disqualified Cells
        /// </summary>
        /// <param name="nextStep">Algorithmic step</param>
        /// <param name="game">Sudoku game object</param>
        public static void UpdateCellValues(Func<Game, List<Move>> nextStep, Game game)
        {
            // What can be done next?
            var whatToDo = nextStep(game);

            if (whatToDo.Count > 0)
            {
                // Implement changes
                foreach (var move in whatToDo)
                    if (move.Kind == MoveType.Solution)
                        game.SetCell(move);
                    else if (move.Kind == MoveType.Disqualification)
                        game.Cells[move.Row, move.Column].Disqualify(move.Value);
            }
        } 
        #endregion
    }
}