using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    internal class TurnSnapshot
    {

        public List<BoardLocation> PlayerPieces {  get; set; }
        public List<BoardLocation> ComputerPieces { get; set; }

        public Piece MovedPiece { get; set; }

        public BoardLocation MoveLocation { get; set; }



        public TurnSnapshot(List<BoardLocation> playerPieces, List<BoardLocation> computerPieces, Piece movedPiece,int moveRow,int moveCol)
        {

            PlayerPieces = playerPieces;
            ComputerPieces = computerPieces;
            MovedPiece = movedPiece;
            MoveLocation = new BoardLocation(moveRow, moveCol);
        }
    }
}
