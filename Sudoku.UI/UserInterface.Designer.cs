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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblFeedback = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnLoners = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnUnique = new System.Windows.Forms.ToolStripSplitButton();
            this.chkHints = new System.Windows.Forms.CheckBox();
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
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFeedback,
            this.lblTime,
            this.btnLoners,
            this.btnUnique});
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
            this.lblTime.Size = new System.Drawing.Size(28, 17);
            this.lblTime.Text = "0:00";
            // 
            // btnLoners
            // 
            this.btnLoners.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLoners.Image = ((System.Drawing.Image)(resources.GetObject("btnLoners.Image")));
            this.btnLoners.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoners.Name = "btnLoners";
            this.btnLoners.Size = new System.Drawing.Size(26, 20);
            this.btnLoners.Text = "L";
            this.btnLoners.ToolTipText = "Lone value for location";
            this.btnLoners.Click += new System.EventHandler(this.btnLoners_Click);
            // 
            // btnUnique
            // 
            this.btnUnique.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUnique.Image = ((System.Drawing.Image)(resources.GetObject("btnUnique.Image")));
            this.btnUnique.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnique.Name = "btnUnique";
            this.btnUnique.Size = new System.Drawing.Size(31, 20);
            this.btnUnique.Text = "U";
            this.btnUnique.ToolTipText = "Unique location for value";
            this.btnUnique.ButtonClick += new System.EventHandler(this.btnUnique_ButtonClick);
            // 
            // chkHints
            // 
            this.chkHints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHints.AutoSize = true;
            this.chkHints.Location = new System.Drawing.Point(12, 422);
            this.chkHints.Name = "chkHints";
            this.chkHints.Size = new System.Drawing.Size(80, 17);
            this.chkHints.TabIndex = 3;
            this.chkHints.Text = "Show Hints";
            this.chkHints.UseVisualStyleBackColor = true;
            this.chkHints.CheckedChanged += new System.EventHandler(this.chkHints_CheckedChanged);
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
            this.Controls.Add(this.chkHints);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlPuzzle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(280, 350);
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
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblFeedback;
        private System.Windows.Forms.CheckBox chkHints;
        private System.Windows.Forms.ToolStripDropDownButton btnLoners;
        private System.Windows.Forms.ToolStripSplitButton btnUnique;
        private System.Windows.Forms.ToolStripStatusLabel lblTime;
        private System.Windows.Forms.Timer tmrClock;
    }
}