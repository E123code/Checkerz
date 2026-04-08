using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    // Objects that handles painting on the screen
    internal class Painter
    {
        public Bitmap Canvas;
        public Pen Pen;
        public int PenX { get; set; }
        public int PenY { get; set; }
        public bool Drawing { get; set; } = false;
        public Painter(Form f)
        {
            this.Canvas = new Bitmap(f.ClientRectangle.Width, f.ClientRectangle.Height);
        }
        public void InitializePen()
        {
            Pen = new Pen(Color.Black, 5);
            Drawing = true;
        }

        public void DrawOnScreen(Form f,MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(Canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen.StartCap = Pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            g.DrawLine(Pen, new Point(PenX, PenY), e.Location);
            PenX = e.X; PenY = e.Y;
            f.Invalidate();
        }

    }
}
