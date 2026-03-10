using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
   abstract class Controller
    {
        protected readonly GameData gameData;
        protected readonly GameLogic gameLogic;

        protected const int ROWNUMBER = 8;
        protected const int COLNNUMBER = 4;

        public Controller(GameData data, GameLogic logic)
        {
            gameData = data;
            gameLogic = logic;
        }

        // Paste your exact shared move methods here:
        public bool TryMoveDownRight(List<BoardLocation> locationsList, int locationIndex, int targetRow, int targetCol, Piece targetPiece) {
            if (targetRow + 1 < ROWNUMBER && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow + 1, targetCol + 1] == null)
            {
                targetPiece.MoveDownRight(false);
                targetPiece.RowIndex++;
                targetPiece.ColIndex++;
                if (locationsList == gameData.playerLocations)
                    targetPiece.Reversed = true;
                locationsList[locationIndex].Row++;
                locationsList[locationIndex].Col++;
                gameData.Board[targetRow + 1, targetCol + 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;
        }
        public bool TryMoveDownLeft(List<BoardLocation> locationsList, int locationIndex, int targetRow, int targetCol, Piece targetPiece) {
            if (targetRow + 1 < ROWNUMBER && targetCol - 1 >= 0 && gameData.Board[targetRow + 1, targetCol - 1] == null)
            {
                targetPiece.MoveDownLeft(false);
                targetPiece.RowIndex++;
                targetPiece.ColIndex--;
                if (locationsList == gameData.playerLocations)
                    targetPiece.Reversed = true;
                locationsList[locationIndex].Row++;
                locationsList[locationIndex].Col--;
                gameData.Board[targetRow + 1, targetCol - 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;

        }
        public bool TryMoveUpRight(List<BoardLocation> locationsList, int locationIndex, int targetRow, int targetCol, Piece targetPiece) { /* ... */
            if (targetRow - 1 >= 0 && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow - 1, targetCol + 1] == null)
            {
                targetPiece.MoveUpRight(false);
                targetPiece.RowIndex--;
                targetPiece.ColIndex++;
                if (locationsList == gameData.computerLocations)
                    targetPiece.Reversed = true;
                locationsList[locationIndex].Row--;
                locationsList[locationIndex].Col++;
                gameData.Board[targetRow - 1, targetCol + 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;
        }
        public bool TryMoveUpLeft(List<BoardLocation> locationsList, int locationIndex, int targetRow, int targetCol, Piece targetPiece) { /* ... */
            if (targetRow - 1 >= 0 && targetCol - 1 >= 0 && gameData.Board[targetRow - 1, targetCol - 1] == null)
            {
                targetPiece.MoveUpLeft(false);
                targetPiece.RowIndex--;
                targetPiece.ColIndex--;
                if (locationsList == gameData.computerLocations)
                    targetPiece.Reversed = true;
                locationsList[locationIndex].Row--;
                locationsList[locationIndex].Col--;
                gameData.Board[targetRow - 1, targetCol - 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;
        }

    }
}
