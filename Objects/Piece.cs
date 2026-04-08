using System.Drawing;
using System.Threading.Tasks;
namespace CheckerZ
{
    // A class that represents a piece on the board
    internal class Piece
    {
        // Size of the piece in the ui
        public const int SIZE = 30;
        // Distance between squares the piece will travel
        public const int MOVEOFFSET = 60;

        public const int INITIALX = 515;

        public const int INITIALY = 115;

        public int RowIndex {  get; set; }

        public int ColIndex {  get; set; }

        public int X {get; set;}

        public int Y {get; set;}

        //A flag to determine which side the piece belongs
        public bool IsPlayer { get; set; }

        public Color PieceColor { get; set; }

        public Piece(int rowIndex,int colIndex, bool isPlayer)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;

            X = INITIALX + colIndex*MOVEOFFSET;
            Y = INITIALY + rowIndex* MOVEOFFSET;

            IsPlayer = isPlayer;


            if (IsPlayer)
            {
                PieceColor = Color.Blue;
            }
            else
            {
                PieceColor = Color.Red;
            }
        }

        //Draw function on the board of the game
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
