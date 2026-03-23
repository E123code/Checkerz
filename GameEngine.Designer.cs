namespace CheckerZ
{
    partial class GameEngine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameEngine));
            this.RightButton = new System.Windows.Forms.Button();
            this.LeftButton = new System.Windows.Forms.Button();
            this.ReverseRightButton = new System.Windows.Forms.Button();
            this.ReverseLeftButton = new System.Windows.Forms.Button();
            this.countdownTimer = new System.Windows.Forms.Timer(this.components);
            this.timerlabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.GameIcon = new System.Windows.Forms.PictureBox();
            this.DrawingMode = new System.Windows.Forms.ToolStripButton();
            this.ClearDrawings = new System.Windows.Forms.ToolStripButton();
            this.Draw = new System.Windows.Forms.ToolStripButton();
            this.clear = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.DrawOnScreen = new System.Windows.Forms.ToolStripButton();
            this.ClearDraws = new System.Windows.Forms.ToolStripButton();
            this.RunReplay = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.GameIcon)).BeginInit();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // RightButton
            // 
            this.RightButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.RightButton.Location = new System.Drawing.Point(1042, 254);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(85, 64);
            this.RightButton.TabIndex = 0;
            this.RightButton.Text = "Right";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.Click += new System.EventHandler(this.RightButtonClick);
            // 
            // LeftButton
            // 
            this.LeftButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LeftButton.Location = new System.Drawing.Point(878, 254);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(82, 64);
            this.LeftButton.TabIndex = 1;
            this.LeftButton.TabStop = false;
            this.LeftButton.Text = "Left";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.Click += new System.EventHandler(this.LeftButtonClick);
            // 
            // ReverseRightButton
            // 
            this.ReverseRightButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ReverseRightButton.Location = new System.Drawing.Point(1042, 384);
            this.ReverseRightButton.Name = "ReverseRightButton";
            this.ReverseRightButton.Size = new System.Drawing.Size(85, 69);
            this.ReverseRightButton.TabIndex = 2;
            this.ReverseRightButton.Text = "ReverseRight";
            this.ReverseRightButton.UseVisualStyleBackColor = true;
            this.ReverseRightButton.Click += new System.EventHandler(this.ReverseRightClick);
            // 
            // ReverseLeftButton
            // 
            this.ReverseLeftButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ReverseLeftButton.Location = new System.Drawing.Point(878, 384);
            this.ReverseLeftButton.Name = "ReverseLeftButton";
            this.ReverseLeftButton.Size = new System.Drawing.Size(82, 69);
            this.ReverseLeftButton.TabIndex = 3;
            this.ReverseLeftButton.Text = "ReverseLeft";
            this.ReverseLeftButton.UseVisualStyleBackColor = true;
            this.ReverseLeftButton.Click += new System.EventHandler(this.ReverseLeftClick);
            // 
            // countdownTimer
            // 
            this.countdownTimer.Interval = 1000;
            this.countdownTimer.Tick += new System.EventHandler(this.countdownTimer_Tick);
            // 
            // timerlabel
            // 
            this.timerlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timerlabel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.timerlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.timerlabel.Location = new System.Drawing.Point(140, 254);
            this.timerlabel.Name = "timerlabel";
            this.timerlabel.Size = new System.Drawing.Size(121, 91);
            this.timerlabel.TabIndex = 4;
            this.timerlabel.Text = "10";
            this.timerlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "2",
            "5",
            "15"});
            this.comboBox1.Location = new System.Drawing.Point(140, 167);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.Text = "10";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // GameIcon
            // 
            this.GameIcon.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GameIcon.Image = global::CheckerZ.Properties.Resources.CheckerZ;
            this.GameIcon.Location = new System.Drawing.Point(127, 398);
            this.GameIcon.Name = "GameIcon";
            this.GameIcon.Size = new System.Drawing.Size(146, 150);
            this.GameIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.GameIcon.TabIndex = 7;
            this.GameIcon.TabStop = false;
            this.GameIcon.Click += new System.EventHandler(this.startgame_Click);
            // 
            // DrawingMode
            // 
            this.DrawingMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DrawingMode.Image = ((System.Drawing.Image)(resources.GetObject("DrawingMode.Image")));
            this.DrawingMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawingMode.Name = "DrawingMode";
            this.DrawingMode.Size = new System.Drawing.Size(86, 22);
            this.DrawingMode.Text = "DrawingMode";
            this.DrawingMode.ToolTipText = "Enable/Disable Draw Mode";
            // 
            // ClearDrawings
            // 
            this.ClearDrawings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ClearDrawings.Image = ((System.Drawing.Image)(resources.GetObject("ClearDrawings.Image")));
            this.ClearDrawings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearDrawings.Name = "ClearDrawings";
            this.ClearDrawings.Size = new System.Drawing.Size(87, 22);
            this.ClearDrawings.Text = "ClearDrawings";
            this.ClearDrawings.ToolTipText = "Clear Drawings on screen";
            // 
            // Draw
            // 
            this.Draw.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Draw.Image = ((System.Drawing.Image)(resources.GetObject("Draw.Image")));
            this.Draw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Draw.Name = "Draw";
            this.Draw.Size = new System.Drawing.Size(38, 22);
            this.Draw.Text = "Draw";
            // 
            // clear
            // 
            this.clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clear.Image = ((System.Drawing.Image)(resources.GetObject("clear.Image")));
            this.clear.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(87, 22);
            this.clear.Text = "ClearDrawings";
            // 
            // ToolStrip
            // 
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DrawOnScreen,
            this.ClearDraws,
            this.RunReplay});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(1264, 25);
            this.ToolStrip.TabIndex = 8;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // DrawOnScreen
            // 
            this.DrawOnScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DrawOnScreen.Image = ((System.Drawing.Image)(resources.GetObject("DrawOnScreen.Image")));
            this.DrawOnScreen.ImageTransparentColor = System.Drawing.Color.Linen;
            this.DrawOnScreen.Name = "DrawOnScreen";
            this.DrawOnScreen.Size = new System.Drawing.Size(69, 22);
            this.DrawOnScreen.Text = "DrawMode";
            this.DrawOnScreen.Click += new System.EventHandler(this.DrawOnScreen_Click);
            // 
            // ClearDraws
            // 
            this.ClearDraws.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ClearDraws.Image = ((System.Drawing.Image)(resources.GetObject("ClearDraws.Image")));
            this.ClearDraws.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearDraws.Name = "ClearDraws";
            this.ClearDraws.RightToLeftAutoMirrorImage = true;
            this.ClearDraws.Size = new System.Drawing.Size(70, 22);
            this.ClearDraws.Text = "ClearDraws";
            this.ClearDraws.ToolTipText = "ClearDraws";
            this.ClearDraws.Click += new System.EventHandler(this.ClearDraws_Click);
            // 
            // RunReplay
            // 
            this.RunReplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RunReplay.Image = ((System.Drawing.Image)(resources.GetObject("RunReplay.Image")));
            this.RunReplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunReplay.Name = "RunReplay";
            this.RunReplay.Size = new System.Drawing.Size(67, 22);
            this.RunReplay.Text = "RunReplay";
            this.RunReplay.Click += new System.EventHandler(this.RunReplay_Click);
            // 
            // GameEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.GameIcon);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.timerlabel);
            this.Controls.Add(this.ReverseLeftButton);
            this.Controls.Add(this.ReverseRightButton);
            this.Controls.Add(this.LeftButton);
            this.Controls.Add(this.RightButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameEngine";
            this.ShowIcon = false;
            this.Text = "CheckerZ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameEngine_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Matrix_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameEngine_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameEngine_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.GameIcon)).EndInit();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.Button LeftButton;
        private System.Windows.Forms.Button ReverseRightButton;
        private System.Windows.Forms.Button ReverseLeftButton;
        private System.Windows.Forms.Timer countdownTimer;
        private System.Windows.Forms.Label timerlabel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Timer animationTimer;
        private System.Windows.Forms.PictureBox GameIcon;
        private System.Windows.Forms.ToolStripButton DrawingMode;
        private System.Windows.Forms.ToolStripButton ClearDrawings;
        private System.Windows.Forms.ToolStripButton Draw;
        private System.Windows.Forms.ToolStripButton clear;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripButton DrawOnScreen;
        private System.Windows.Forms.ToolStripButton ClearDraws;
        private System.Windows.Forms.ToolStripButton RunReplay;
    }
}

