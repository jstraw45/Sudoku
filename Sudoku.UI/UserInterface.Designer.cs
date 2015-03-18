namespace Sudoku.UI
{
    partial class UserInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterface));
            this.pnlPuzzle = new System.Windows.Forms.Panel();
            this.chkNotes = new System.Windows.Forms.CheckBox();
            this.chkHints = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblFeedback = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbAuto = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbLoners = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbUnique = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbElsewhereInGroup = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbIntersectionExclusions = new System.Windows.Forms.ToolStripSplitButton();
            this.tmrClock = new System.Windows.Forms.Timer(this.components);
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPuzzle
            // 
            this.pnlPuzzle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPuzzle.Location = new System.Drawing.Point(12, 12);
            this.pnlPuzzle.Name = "pnlPuzzle";
            this.pnlPuzzle.Size = new System.Drawing.Size(400, 400);
            this.pnlPuzzle.TabIndex = 0;
            this.pnlPuzzle.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPuzzle_Paint);
            this.pnlPuzzle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlPuzzle_MouseClick);
            this.pnlPuzzle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlPuzzle_MouseMove);
            this.pnlPuzzle.Resize += new System.EventHandler(this.pnlPuzzle_Resize);
            // 
            // chkNotes
            // 
            this.chkNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkNotes.AutoSize = true;
            this.chkNotes.Checked = true;
            this.chkNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotes.Location = new System.Drawing.Point(12, 418);
            this.chkNotes.Name = "chkNotes";
            this.chkNotes.Size = new System.Drawing.Size(84, 17);
            this.chkNotes.TabIndex = 2;
            this.chkNotes.Text = "Show Notes";
            this.chkNotes.UseVisualStyleBackColor = true;
            this.chkNotes.CheckedChanged += new System.EventHandler(this.chkNotes_CheckedChanged);
            // 
            // chkHints
            // 
            this.chkHints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHints.AutoSize = true;
            this.chkHints.Location = new System.Drawing.Point(102, 418);
            this.chkHints.Name = "chkHints";
            this.chkHints.Size = new System.Drawing.Size(80, 17);
            this.chkHints.TabIndex = 3;
            this.chkHints.Text = "Show Hints";
            this.chkHints.UseVisualStyleBackColor = true;
            this.chkHints.CheckedChanged += new System.EventHandler(this.chkHints_CheckedChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFeedback,
            this.lblTime,
            this.tsbAuto,
            this.tsbLoners,
            this.tsbUnique,
            this.tsbElsewhereInGroup,
            this.tsbIntersectionExclusions});
            this.statusStrip.Location = new System.Drawing.Point(0, 446);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(422, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblFeedback
            // 
            this.lblFeedback.Name = "lblFeedback";
            this.lblFeedback.Size = new System.Drawing.Size(81, 17);
            this.lblFeedback.Text = "M of N solved";
            // 
            // lblTime
            // 
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(43, 17);
            this.lblTime.Text = "0:00:00";
            // 
            // tsbAuto
            // 
            this.tsbAuto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAuto.Image = ((System.Drawing.Image)(resources.GetObject("tsbAuto.Image")));
            this.tsbAuto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAuto.Name = "tsbAuto";
            this.tsbAuto.Size = new System.Drawing.Size(31, 20);
            this.tsbAuto.Text = "A";
            this.tsbAuto.ToolTipText = "Auto-solve";
            this.tsbAuto.ButtonClick += new System.EventHandler(this.tsbAuto_ButtonClick);
            // 
            // tsbLoners
            // 
            this.tsbLoners.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoners.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoners.Image")));
            this.tsbLoners.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoners.Name = "tsbLoners";
            this.tsbLoners.Size = new System.Drawing.Size(26, 20);
            this.tsbLoners.Text = "L";
            this.tsbLoners.ToolTipText = "Lone value for location";
            this.tsbLoners.Click += new System.EventHandler(this.tsbLoners_Click);
            // 
            // tsbUnique
            // 
            this.tsbUnique.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbUnique.Image = ((System.Drawing.Image)(resources.GetObject("tsbUnique.Image")));
            this.tsbUnique.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnique.Name = "tsbUnique";
            this.tsbUnique.Size = new System.Drawing.Size(31, 20);
            this.tsbUnique.Text = "U";
            this.tsbUnique.ToolTipText = "Unique location for value";
            this.tsbUnique.ButtonClick += new System.EventHandler(this.tsbUnique_ButtonClick);
            // 
            // tsbElsewhereInGroup
            // 
            this.tsbElsewhereInGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbElsewhereInGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsbElsewhereInGroup.Image")));
            this.tsbElsewhereInGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbElsewhereInGroup.Name = "tsbElsewhereInGroup";
            this.tsbElsewhereInGroup.Size = new System.Drawing.Size(29, 20);
            this.tsbElsewhereInGroup.Text = "E";
            this.tsbElsewhereInGroup.ToolTipText = "Exclusions";
            this.tsbElsewhereInGroup.ButtonClick += new System.EventHandler(this.tsbElsewhereInGroup_ButtonClick);
            // 
            // tsbIntersectionExclusions
            // 
            this.tsbIntersectionExclusions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbIntersectionExclusions.Image = ((System.Drawing.Image)(resources.GetObject("tsbIntersectionExclusions.Image")));
            this.tsbIntersectionExclusions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbIntersectionExclusions.Name = "tsbIntersectionExclusions";
            this.tsbIntersectionExclusions.Size = new System.Drawing.Size(26, 20);
            this.tsbIntersectionExclusions.Text = "I";
            this.tsbIntersectionExclusions.ToolTipText = "Intersections";
            this.tsbIntersectionExclusions.ButtonClick += new System.EventHandler(this.tsbIntersectionExclusions_ButtonClick);
            // 
            // tmrClock
            // 
            this.tmrClock.Enabled = true;
            this.tmrClock.Interval = 1000;
            this.tmrClock.Tick += new System.EventHandler(this.tmrClock_Tick);
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 468);
            this.Controls.Add(this.chkNotes);
            this.Controls.Add(this.chkHints);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlPuzzle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(420, 500);
            this.Name = "UserInterface";
            this.Text = "Strawbuilt Sudoku";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserInterface_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlPuzzle;
        private System.Windows.Forms.CheckBox chkNotes;
        private System.Windows.Forms.CheckBox chkHints;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblFeedback;
        private System.Windows.Forms.ToolStripStatusLabel lblTime;
        private System.Windows.Forms.ToolStripSplitButton tsbAuto;
        private System.Windows.Forms.ToolStripDropDownButton tsbLoners;
        private System.Windows.Forms.ToolStripSplitButton tsbElsewhereInGroup;
        private System.Windows.Forms.ToolStripSplitButton tsbIntersectionExclusions;
        private System.Windows.Forms.ToolStripSplitButton tsbUnique;
        private System.Windows.Forms.Timer tmrClock;
    }
}