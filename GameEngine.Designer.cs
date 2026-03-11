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
            this.MoveRight = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.GameIcon)).BeginInit();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MoveRight
            // 
            this.MoveRight.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MoveRight.Location = new System.Drawing.Point(1042, 254);
            this.MoveRight.Name = "MoveRight";
            this.MoveRight.Size = new System.Drawing.Size(85, 64);
            this.MoveRight.TabIndex = 0;
            this.MoveRight.Text = "Right";
            this.MoveRight.UseVisualStyleBackColor = true;
            this.MoveRight.Click += new System.EventHandler(this.RightButtonClick);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.button2.Location = new System.Drawing.Point(878, 254);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 64);
            this.button2.TabIndex = 1;
            this.button2.TabStop = false;
            this.button2.Text = "Left";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.LeftButtonClick);
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.button3.Location = new System.Drawing.Point(1042, 384);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 69);
            this.button3.TabIndex = 2;
            this.button3.Text = "ReverseRight";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.ReverseRightClick);
            // 
            // button4
            // 
            this.button4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.button4.Location = new System.Drawing.Point(878, 384);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 69);
            this.button4.TabIndex = 3;
            this.button4.Text = "ReverseLeft";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ReverseLeftClick);
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
            this.comboBox1.UseWaitCursor = true;
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
            this.ClearDraws});
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
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.MoveRight);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameEngine";
            this.ShowIcon = false;
            this.Text = "CheckerZ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameEngine_FormClosing);
            this.Load += new System.EventHandler(this.GameEngine_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Matrix_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameEngine_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameEngine_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameEngine_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.GameIcon)).EndInit();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MoveRight;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
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
    }
}

