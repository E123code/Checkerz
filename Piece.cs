using System.Drawing;
using System.Threading.Tasks;
namespace CheckerZ
{
    internal class Piece
    {
        public const int SIZE = 30;

        public const int MOVEOFFSET = 60;

        public const int INITIALX = 515;

        public const int INITIALY = 115;


        public int RowIndex {  get; set; }

        public int ColIndex {  get; set; }

        public int X {get; set;}

        public int Y {get; set;}

        public bool IsPlayer { get; set; }

        public bool Reversed { get; set; }

        public Color PieceColor { get; set; }

        public Piece(int rowIndex,int colIndex, bool isPlayer, bool reversed)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;

            X = INITIALX + colIndex*MOVEOFFSET;
            Y = INITIALY + rowIndex*MOVEOFFSET;

            IsPlayer = isPlayer;
            Reversed = reversed;

            if (IsPlayer)
            {
                PieceColor = Color.Blue;
            }
            else
            {
                PieceColor = Color.Red;
            }
        }

        public void MoveUpRight(bool capture)
        {
            //for (int i = 0; i < MOVEOFFSET; i++)
            //{
            //    this.X += 1;
            //    this.Y -= 1;
            //}
            if (capture)
            {
                this.X += MOVEOFFSET * 2;
                this.Y -= MOVEOFFSET * 2;
            }
            else
            {
                this.X += MOVEOFFSET;
                this.Y -= MOVEOFFSET;
            }
        }

        public void MoveUpLeft(bool capture)
        {
            //for (int i = 0; i < MOVEOFFSET; i++)
            //{
            //    this.X -= 1;
            //    this.Y -= 1;
            //}
            if (capture)
            {
                this.X -= MOVEOFFSET * 2;
                this.Y -= MOVEOFFSET * 2;
            }
            else
            {
                this.X -= MOVEOFFSET;
                this.Y -= MOVEOFFSET;
            }
        }


        public void MoveDownRight(bool capture)
        {
            //for(int i = 0; i < MOVEOFFSET; i++)
            //{
            //    this.X += 1;
            //    this.Y += 1;
            //}
            if (capture)
            {
                this.X += MOVEOFFSET * 2;
                this.Y += MOVEOFFSET * 2;
            }
            else
            {
                this.X += MOVEOFFSET;
                this.Y += MOVEOFFSET;
            }
        }

        public void MoveDownLeft(bool capture)
        {
            //for (int i = 0; i < MOVEOFFSET; i++)
            //{
            //    this.X -= 1;
            //    this.Y += 1;
            //}

            if (capture)
            {
                this.X -= MOVEOFFSET * 2;
                this.Y += MOVEOFFSET * 2;
            }
            else
            {
                this.X -= MOVEOFFSET;
                this.Y += MOVEOFFSET;
            }
        }


        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(PieceColor))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(brush, X, Y, SIZE, SIZE);
                g.DrawEllipse(Pens.Black, X, Y, SIZE, SIZE);
            }
        }


    }
}
