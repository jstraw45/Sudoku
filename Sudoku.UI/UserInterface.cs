/****************************************************************
 * Sudoku puzzle user interface
 * Jeff Straw | Northwestern Michigan College
 * Icon from http://findicons.com/icon/267689/3x3_grid
 * 02/06/2015: initial release
 * 02/15/2015: added GDI+ grid
 * 02/17/2015: refactored methods
 * 02/22/2015: trapped (and logged) key press and mouse click
 * 02/23/2015: added stack push/pop
 * 03/01/2015: added Exclusion
 * 03/06/2015: updated feedback to show remaining hints
 * 03/08/2015: added Intersections
 * 03/10/2015: added Autosolve
 ***************************************************************/
using Sudoku.Model;
using Sudoku.UI.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utility;
// Original "using" directives that are currently unused:
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Sudoku.UI
{
    /// <summary>
    /// WinForms GUI for Sudoku game
    /// </summary>
    public partial class UserInterface : Form
    {
        #region [ Fields and business rules ]
        // Graphics specifications
        private static Color backgroundColor = Color.White;
        private static Color gridColor = Color.Black;

        Pen thickPen = new Pen(gridColor, 2.0f);        // Used in region outlines
        Pen thinPen = new Pen(gridColor, 1.0f);         // Used in cell outlines

        Pen thickTestPen = new Pen(Color.FromArgb(255, 255, 051, 051), 2.0f);
        Pen thinTestPen = new Pen(Color.FromArgb(255, 255, 051, 051), 1.0f);

        Brush backgroundBrush = new SolidBrush(backgroundColor);
        Brush hintBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));       // Light gray
        Brush noteBrush = new SolidBrush(Color.FromArgb(255, 36, 111, 159));        // Deep blue-gray
        Brush knownBrush = new SolidBrush(gridColor);
        Brush solveBrush = new SolidBrush(Color.FromArgb(255, 048, 173, 035));      // Green

        StringFormat stringFormat = new StringFormat(); // Used to configure text in graphics
        Font hintFont = new Font("Consolas", 10f);      // Small font for hints
        Font puzzleFont = new Font("Consolas", 18f);    // Large font for static and solved cells

        // Shared calculated values used for scaling and translating
        private int sqrtGameSize;                       // Linear cells in region or linear hints in cell
        private int logicalCellSize;                    // 1 for each hint spot, one for one of two borders
        private static int logicalUnitCount;            // 1 for left/top edge, add cells

        // Shared puzzle and drawing objects
        private Game game;                              // Sudoku game object
        private Stack<Game> gameHistory;                // For undo/redo

        private Triad mouseLocation;                    // Most recent row/column/hint zone

        // Shared resources
        Random rand = new Random();
        Transform trans;
        DateTime launch;
        #endregion

        #region [ Constructors ]
        /// <summary>
        /// WinForms GUI for Sudoku game
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();

            game = PuzzleFactory.GetSudoku();           // Instantiate puzzle
            trans = new Transform(game.SideLength,      // Scaled and translated utility methods to match current dimensions
                new Size(pnlPuzzle.Width, pnlPuzzle.Height));
            gameHistory = new Stack<Game>();            // Prepare for undo history
            launch = DateTime.Now;

            // Configure shared calculated values used for scaling and translating
            sqrtGameSize = (int)Math.Round(Math.Sqrt(game.SideLength), 0);
            logicalCellSize = sqrtGameSize + 1;
            logicalUnitCount = 1 + game.SideLength * logicalCellSize;

            //SetStyle(ControlStyles.ResizeRedraw, true); // Redraw form when resized
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }
        #endregion

        #region [ Graphic event-handler methods ]
        /// <summary>
        /// Redraw panel whenever it is resized
        /// </summary>
        /// <param name="sender">Container that needs to be redrawn</param>
        /// <param name="e">Event arguments object</param>
        private void pnlPuzzle_Resize(object sender, EventArgs e)
        {
            trans = new Transform(game.SideLength,      // Scaled and translated utility methods to match current dimensions
                new Size(pnlPuzzle.Width, pnlPuzzle.Height));
            (sender as Control).Invalidate();
        }

        /// <summary>
        /// Actions to perform when the pnlPuzzle Panel needs to be refreshed visibly
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="eventObject">Object encapsulating paint event properties and methods</param>
        private void pnlPuzzle_Paint(object sender, PaintEventArgs eventObject)
        {
            var graphicsObject = eventObject.Graphics;      // Graphics object for all drawing

            // Fill background - do this before grid lines
            var topLeft = trans.PhysicalFromGame(new Duad(0, 0));
            var bottomRight = trans.PhysicalFromGame(new Duad(game.SideLength, game.SideLength));
            graphicsObject.FillRectangle(backgroundBrush,
                topLeft.X,
                topLeft.Y,
                bottomRight.X - topLeft.X,
                bottomRight.Y - topLeft.Y);

            // Draw cell borders, highlighting regions (including puzzle borders)
            for (var x = 0; x <= game.SideLength; x++)
                graphicsObject.DrawLine(x % sqrtGameSize == 0 ? thickPen : thinPen,
                    trans.PhysicalFromGame(new Duad(x, 0)),
                    trans.PhysicalFromGame(new Duad(x, game.SideLength)));
            for (var y = 0; y <= game.SideLength; y++)
                graphicsObject.DrawLine(y % sqrtGameSize == 0 ? thickPen : thinPen,
                    trans.PhysicalFromGame(new Duad(0, y)),
                    trans.PhysicalFromGame(new Duad(game.SideLength, y)));

            // Draw contents of game as it is now known
            for (var row = 0; row < game.SideLength; row++)
                for (var column = 0; column < game.SideLength; column++)
                {
                    var thisCell = game.Cells[row, column];
                    if (thisCell.IsSolved)
                    {
                        var solvedCellLocation = trans.PhysicalCellCenterFromGame(new Duad(row, column));
                        graphicsObject.DrawString(thisCell.CurrentValue.ToString(),
                            puzzleFont,
                            thisCell.Status == CellStatus.Known ? knownBrush : solveBrush,
                            solvedCellLocation.X,
                            solvedCellLocation.Y,
                            stringFormat);
                    }
                    else if (chkHints.Checked)  // Show hints too, when desired
                    {
                        for (var hint = 0; hint < game.SideLength; hint++)
                        {
                            var destination = trans.PhysicalFromGame(new Triad(row, column, hint));
                            graphicsObject.DrawString(thisCell.Domain[hint].ToString(),
                                hintFont, hintBrush, destination.X, destination.Y, stringFormat);
                        }
                    }
                    else if (chkNotes.Checked)  // Show user notes, when desired
                    {
                        for (var note = 0; note < game.SideLength; note++)
                        {
                            var destination = trans.PhysicalFromGame(new Triad(row, column, note));
                            graphicsObject.DrawString(thisCell.ScratchPad[note].ToString(),
                                hintFont, noteBrush, destination.X, destination.Y, stringFormat);
                        }
                    }
                }

            // Update user feedback
            lblFeedback.Text = chkHints.Checked
                ? String.Format("{0:d} of {1:d} solved, {2:d} hints, {3:d} steps; level {4:d}",
                    game.SolvedCells, game.SideLength * game.SideLength, game.RemainingPossibilities, game.StepCount, game.Difficulty)
                : String.Format("{0:d} of {1:d} solved, {2:d} steps; level {3:d}",
                    game.SolvedCells, game.SideLength * game.SideLength, game.StepCount, game.Difficulty);
            lblTime.Text = String.Format(@"{0:h\:mm\:ss}", DateTime.Now - launch);

            // Stop the clock if done
            if (game.IsSolved)
                tmrClock.Enabled = false;
        }

        /// <summary>
        /// Respond to user-triggered puzzle actions
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void pnlPuzzle_MouseClick(object sender, MouseEventArgs e)
        {
            // Identify game-based location of the mouse pointer
            var here = new Point(e.X, e.Y);
            Cell currentCell = game.Cells[mouseLocation.Row, mouseLocation.Column];

            // Is the mouse over a hint zone in the game area?
            if (trans.OnGame(here) && mouseLocation.HintZone != trans.HintZoneUnknown)
            {
                if (chkNotes.Checked)           // In notes mode - toggle visibility of note
                {
                    if (currentCell.ScratchPad[mouseLocation.HintZone] == Cell.valueWhenUnsolved)
                        currentCell.AddScratch(mouseLocation.HintZone);
                    else
                        currentCell.RemoveScratch(mouseLocation.HintZone);

                    RefreshPuzzle();
                }
                else if (chkHints.Checked)      // In visible hints mode - disqualify (or potentially requalify) value
                {
                    // Identify the char that represents the specific HintZone within the cell
                    char potentialSolution = currentCell.ValueAtIndex(mouseLocation.HintZone);

                    // Is the specified point still a valid solution (i.e., has it not yet been disqualified)?
                    if (currentCell.IsPotentialSolution(potentialSolution))
                    {
                        // Save a snapshot of the game as it currently stands
                        gameHistory.Push(game.DeepClone);

                        // Declare the option unavailable
                        currentCell.Disqualify(potentialSolution);
                        RefreshPuzzle();
                    }
                }
            }
        }

        /// <summary>
        /// Save current mouse position
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void pnlPuzzle_MouseMove(object sender, MouseEventArgs e)
        {
            // Identify game-based location of the mouse pointer
            var here = new Point(e.X, e.Y);

            // Save location for later keyboard interaction, but only if mouse is over the game
            if (trans.OnGame(here))
                mouseLocation = trans.GameFromPhysical(here);
        }

        /// <summary>
        /// Respond when user clicks any key
        /// </summary>
        /// <param name="sender">The form itself</param>
        /// <param name="e">Details about the key that was pressed</param>
        private void UserInterface_KeyDown(object sender, KeyEventArgs e)
        {
            // Did user ask for undo with Ctrl+Z?
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    if (gameHistory.Count > 0)
                    {
                        game = gameHistory.Pop();                       // Pull most-recently saved game off the stack
                        if (!game.IsSolved)                             // Restart the clock in case Game is no longer done
                            tmrClock.Enabled = true;
                        RefreshPuzzle();
                    }
                    else
                    {
                        Beep();                                         // Inform user of end of stack
                        game.Difficulty = 0;
                        game.StepCount = 0;
                        RefreshPuzzle();
                    }
                }
            }
            else
            {
                var pressed = e.KeyCode.ToString().ToUpper();           // Handle upper- and lower-case similarly
                var charPressed = pressed[pressed.Length - 1];          // char representation, including both numerics

                var cellToCheck = game.Cells[mouseLocation.Row, mouseLocation.Column];
                if (!cellToCheck.IsSolved && cellToCheck.RemainingPossibilities.Contains(charPressed))
                {
                    // Save a snapshot of the game as it currently stands
                    gameHistory.Push(game.DeepClone);

                    var move = new Move(mouseLocation.Row, mouseLocation.Column, charPressed, MoveType.Solution);
                    game.SetCell(move);
                    RefreshPuzzle();
                }
                else
                {
                    Beep();
                }
            }
        }

        /// <summary>
        /// Change status of hint display
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void chkNotes_CheckedChanged(object sender, EventArgs e)
        {
            // System hints mode disables user notes mode
            if (chkNotes.Checked)
                chkHints.Checked = false;

            RefreshPuzzle();
        }

        /// <summary>
        /// Change status of hint display
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void chkHints_CheckedChanged(object sender, EventArgs e)
        {
            // User notes mode disables hints mode
            if (chkHints.Checked)
                chkNotes.Checked = false;

            RefreshPuzzle();
        }

        /// <summary>
        /// Update the elapsed time clock
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void tmrClock_Tick(object sender, EventArgs e)
        {
            lblTime.Text = String.Format(@"{0:h\:mm\:ss}", DateTime.Now - launch);
        }
        #endregion

        #region [ Puzzle-solving event-handler methods ]
        /// <summary>
        /// Solve game automatically - as far as is currently possible
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void tsbAuto_ButtonClick(object sender, EventArgs e)
        {
            // Anything left to do?
            if (game.IsSolved) { Hand(); return; }

            // Set of algorithm methods, ordered from simplest to most complex
            var process = new List<Func<Game, List<Move>>>();
            process.Add(Logic.Loners);
            process.Add(Logic.Unique);
            process.Add(Logic.ElsewhereInGroup);
            process.Add(Logic.IntersectingExclusions);

            // Save a snapshot of the game as it currently stands
            gameHistory.Push(game.DeepClone);

            Logic.SolveRecursively(process, 0, game);
            RefreshPuzzle();
        }

        /// <summary>
        /// Solve loners - those unsolved Cells that have only one remaining possibility
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void tsbLoners_Click(object sender, EventArgs e)
        {
            // Anything left to do?
            if (game.IsSolved) { Hand(); return; }

            // Save a snapshot of the game as it currently stands
            gameHistory.Push(game.DeepClone);
            game.StepCount++;

            Logic.UpdateCellValues(Logic.Loners, game);

            RefreshPuzzle();
        }

        /// <summary>
        /// Solve uniques - where only one Cell in a row/column/region can provide a specific value
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void tsbUnique_ButtonClick(object sender, EventArgs e)
        {
            // Anything left to do?
            if (game.IsSolved) { Hand(); return; }

            // Save a snapshot of the game as it currently stands
            gameHistory.Push(game.DeepClone);
            game.StepCount++;

            Logic.UpdateCellValues(Logic.Unique, game);
            RefreshPuzzle();
        }

        /// <summary>
        /// Find all Moves that must be excluded due to requirement in other Cells in same row/column/region
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void tsbElsewhereInGroup_ButtonClick(object sender, EventArgs e)
        {
            // Anything left to do?
            if (game.IsSolved) { Hand(); return; }

            // Save a snapshot of the game as it currently stands
            gameHistory.Push(game.DeepClone);
            game.StepCount++;

            Logic.UpdateCellValues(Logic.ElsewhereInGroup, game);
            RefreshPuzzle();
        }

        /// <summary>
        /// Find all Moves that must be excluded due to intersection constraints between regions and rows/columns 
        /// </summary>
        /// <param name="sender">Object that triggered the event handler</param>
        /// <param name="e">Object encapsulating event properties and methods</param>
        private void tsbIntersectionExclusions_ButtonClick(object sender, EventArgs e)
        {
            // Anything left to do?
            if (game.IsSolved) { Hand(); return; }

            // Save a snapshot of the game as it currently stands
            gameHistory.Push(game.DeepClone);
            game.StepCount++;

            Logic.UpdateCellValues(Logic.IntersectingExclusions, game);
            RefreshPuzzle();
        }

        #endregion

        #region [ Helper methods ]
        /// <summary>
        /// Audio response to user
        /// </summary>
        private void Beep()
        {
            System.Media.SystemSounds.Beep.Play();                      // Hand appears to have a different default sound
        }

        /// <summary>
        /// Audio response to user
        /// </summary>
        private void Hand()
        {
            System.Media.SystemSounds.Hand.Play();
        }

        /// <summary>
        /// Re-draw the puzzle in its current state
        /// </summary>
        private void RefreshPuzzle()
        {
            pnlPuzzle.Invalidate();                                     // Request repainting
        }
        #endregion
    }
}