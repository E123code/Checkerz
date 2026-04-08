using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    // A child of controller that handles the moves of the players.

    internal class PlayerController : Controller
        //: Admin
    {
        public PlayerController(GameData data, GameLogic logic) : base(data, logic) { }

        // Handles possible options to move right
        public bool AttemptMoveRight(Piece selectedPiece, int locationIndex)
        {
            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;

            return TryCaptureUpRight(locationIndex, targetRow, targetCol, selectedPiece) || TryMoveUpRight(locationIndex, targetRow, targetCol, selectedPiece);
        }
        // Handles possible options to move left
        public bool AttemptMoveLeft(Piece selectedPiece, int locationIndex)
        {
            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;

            return TryCaptureUpLeft(locationIndex, targetRow, targetCol, selectedPiece) || TryMoveUpLeft(locationIndex, targetRow, targetCol, selectedPiece);
        }


        //reverse right
        public bool TryMoveDownRight(int locationIndex, int targetRow, int targetCol, Piece targetPiece)
        {
            if (targetRow + 1 < ROWNUMBER && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow + 1, targetCol + 1] == null)
            {
                targetPiece.RowIndex++;
                targetPiece.ColIndex++;

                gameData.playerLocations[locationIndex].isReversed = true;
                gameData.playerLocations[locationIndex].Row++;
                gameData.playerLocations[locationIndex].Col++;

                gameData.Board[targetRow + 1, targetCol + 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;
        }

        //reverse left
        public bool TryMoveDownLeft(int locationIndex, int targetRow, int targetCol, Piece targetPiece)
        {
            if (targetRow + 1 < ROWNUMBER && targetCol - 1 >= 0 && gameData.Board[targetRow + 1, targetCol - 1] == null)
            {
                targetPiece.RowIndex++;
                targetPiece.ColIndex--;

                gameData.playerLocations[locationIndex].isReversed = true;
                gameData.playerLocations[locationIndex].Row++;
                gameData.playerLocations[locationIndex].Col--;

                gameData.Board[targetRow + 1, targetCol - 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;

        }

        //right
        public bool TryMoveUpRight(int locationIndex, int targetRow, int targetCol, Piece targetPiece)
        { /* ... */
            if (targetRow - 1 >= 0 && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow - 1, targetCol + 1] == null)
            {
                targetPiece.RowIndex--;
                targetPiece.ColIndex++;

                gameData.playerLocations[locationIndex].Row--;
                gameData.playerLocations[locationIndex].Col++;

                gameData.Board[targetRow - 1, targetCol + 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;
        }

        //left
        public bool TryMoveUpLeft(int locationIndex, int targetRow, int targetCol, Piece targetPiece)
        { /* ... */
            if (targetRow - 1 >= 0 && targetCol - 1 >= 0 && gameData.Board[targetRow - 1, targetCol - 1] == null)
            {
                targetPiece.RowIndex--;
                targetPiece.ColIndex--;

                gameData.playerLocations[locationIndex].Row--;
                gameData.playerLocations[locationIndex].Col--;

                gameData.Board[targetRow - 1, targetCol - 1] = targetPiece;
                gameData.Board[targetRow, targetCol] = null;
                return true;
            }
            return false;
        }

        //player captures

        public bool TryCaptureUpRight(int locationIndex, int targetRow, int targetCol, Piece targetPiece)
        {
            if (targetRow - 2 >= 0 && targetCol + 2 < COLNNUMBER)
            {
                Piece midPiece = gameData.Board[targetRow - 1, targetCol + 1];
                // Is there a player piece in the way and is the landing spot empty?
                if (midPiece != null && !midPiece.IsPlayer && gameData.Board[targetRow - 2, targetCol + 2] == null)
                {
                    targetPiece.RowIndex -= 2;
                    targetPiece.ColIndex += 2;

                    gameData.Board[targetRow - 2, targetCol + 2] = targetPiece;
                    gameData.Board[targetRow - 1, targetCol + 1] = null;
                    gameData.Board[targetRow, targetCol] = null;

                    gameData.playerLocations[locationIndex].Row = targetRow - 2;
                    gameData.playerLocations[locationIndex].Col = targetCol + 2;
                    for (int i = 0; i < gameData.computerLocations.Count; i++)
                    {
                        if (gameData.computerLocations[i].Row == midPiece.RowIndex && gameData.computerLocations[i].Col == midPiece.ColIndex)
                        {
                            gameData.computerLocations.RemoveAt(i);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public bool TryCaptureUpLeft(int locationIndex, int targetRow, int targetCol, Piece targetPiece) {
            if (targetRow - 2 >= 0 && targetCol - 2 >= 0)
            {
                Piece midPiece = gameData.Board[targetRow - 1, targetCol - 1];
                // Is there a player piece in the way and is the landing spot empty?
                if (midPiece != null && !midPiece.IsPlayer && gameData.Board[targetRow - 2, targetCol - 2] == null)
                {
                    targetPiece.RowIndex -= 2;
                    targetPiece.ColIndex -= 2;

                    gameData.Board[targetRow - 2, targetCol - 2] = targetPiece;
                    gameData.Board[targetRow - 1, targetCol - 1] = null;
                    gameData.Board[targetRow, targetCol] = null;

                    gameData.playerLocations[locationIndex].Row = targetRow - 2;
                    gameData.playerLocations[locationIndex].Col = targetCol - 2;
                    for (int i = 0; i < gameData.computerLocations.Count; i++)
                    {
                        if (gameData.computerLocations[i].Row == midPiece.RowIndex && gameData.computerLocations[i].Col == midPiece.ColIndex)
                        {
                            gameData.computerLocations.RemoveAt(i);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }

}
