/**********************************************************************************
 * Unit tests for factory that generates Sudoku puzzles
 * Jeff Straw | Northwestern Michigan College
 * 02/19/2015: Initial release
 *********************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sudoku.Model.UnitTests
{
    /// <summary>
    /// Unit tests for Sudoku.Model.PuzzleFactory
    /// </summary>
    [TestClass()]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PuzzleFactoryUnitTests
    {
        /// <summary>
        /// Does the factory instantiate a new game correctly?
        /// </summary>
        [TestMethod()]
        public void PuzzleFactory_GetSudoku_Valid()
        {
            var puzzle = PuzzleFactory.GetSudoku();
            Assert.IsInstanceOfType(puzzle, typeof(Game));
        }
    }
}
