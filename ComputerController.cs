using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    internal class ComputerController : Controller

    {
        private Random rng = new Random();

        public ComputerController(GameData data, GameLogic logic) : base(data, logic) { }

        public void Shuffle<T>(List<T> list) {
            int n = list.Count;
            while (n > 1)
            {
                // [1,2,3,4]
                // [2,1,4,3]
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // Notice the 3 'out' parameters added to the signature
        public bool ExecuteComputerMove(out Piece movedPiece,out int startX,out int startY)
        {
            movedPiece = null;
            startX = 0;
            startY = 0;

            if (gameData.computerLocations.Count > 1)
            {
                Shuffle(gameData.computerLocations);
            }

            // 1. Trying to capture a player piece
            for (int i = 0; i < gameData.computerLocations.Count; i++)
            {
                int targetRow = gameData.computerLocations[i].Row;
                int targetCol = gameData.computerLocations[i].Col;
                Piece targetPiece = gameData.Board[targetRow, targetCol];

                if (targetPiece != null)
                {
                    // Capture the coordinates BEFORE the logic moves it
                    int tempX = targetPiece.X;
                    int tempY = targetPiece.Y;

                    if (TryCaptureDownRight(i, targetRow, targetCol, targetPiece) ||
                        TryCaptureDownLeft(i, targetRow, targetCol, targetPiece))
                    {
                        // We got a successful capture! Report back to the Engine.
                        movedPiece = targetPiece;
                        startX = tempX;
                        startY = tempY;
                        return true;
                    }
                }
            }

            // 2. Trying to move a piece normally
            for (int i = 0; i < gameData.computerLocations.Count; i++)
            {
                int targetRow = gameData.computerLocations[i].Row;
                int targetCol = gameData.computerLocations[i].Col;
                Piece targetPiece = gameData.Board[targetRow, targetCol];

                if (targetPiece != null)
                {
                    // Capture coordinates BEFORE moving
                    int tempX = targetPiece.X;
                    int tempY = targetPiece.Y;

                    if (TryMoveDownRight(gameData.computerLocations, i, targetRow, targetCol, targetPiece) ||
                        TryMoveDownLeft(gameData.computerLocations, i, targetRow, targetCol, targetPiece))
                    {
                        movedPiece = targetPiece;
                        startX = tempX;
                        startY = tempY;
                        return true;
                    }

                    if (!targetPiece.Reversed &&
                       (TryMoveUpRight(gameData.computerLocations, i, targetRow, targetCol, targetPiece) ||
                        TryMoveUpLeft(gameData.computerLocations, i, targetRow, targetCol, targetPiece)))
                    {
                        movedPiece = targetPiece;
                        startX = tempX;
                        startY = tempY;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryCaptureDownRight(int locationIndex, int targetRow, int targetCol, Piece targetPiece) {
            if (targetRow + 2 < ROWNUMBER && targetCol + 2 < COLNNUMBER)
            {
                Piece midPiece = gameData.Board[targetRow + 1, targetCol + 1];
                // Is there a player piece in the way and is the landing spot empty?
                if (midPiece != null && midPiece.IsPlayer && gameData.Board[targetRow + 2, targetCol + 2] == null)
                {
                    targetPiece.MoveDownRight(true);
                    targetPiece.RowIndex += 2;
                    targetPiece.ColIndex += 2;

                    gameData.Board[targetRow + 2, targetCol + 2] = targetPiece;
                    gameData.Board[targetRow + 1, targetCol + 1] = null;
                    gameData.Board[targetRow, targetCol] = null;

                    gameData.computerLocations[locationIndex].Row = targetRow + 2;
                    gameData.computerLocations[locationIndex].Col = targetCol + 2;
                    for (int i = 0; i < gameData.playerLocations.Count; i++)
                    {
                        if (gameData.playerLocations[i].Row == midPiece.RowIndex && gameData.playerLocations[i].Col == midPiece.ColIndex)
                        {
                            gameData.playerLocations.RemoveAt(i);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        private bool TryCaptureDownLeft(int locationIndex, int targetRow, int targetCol, Piece targetPiece) {
            if (targetRow + 2 < ROWNUMBER && targetCol - 2 >= 0)
            {
                Piece midPiece = gameData.Board[targetRow + 1, targetCol - 1];
                if (midPiece != null && midPiece.IsPlayer && gameData.Board[targetRow + 2, targetCol - 2] == null)
                {
                    targetPiece.MoveDownLeft(true);
                    targetPiece.RowIndex += 2;
                    targetPiece.ColIndex -= 2;

                    gameData.Board[targetRow + 2, targetCol - 2] = targetPiece;
                    gameData.Board[targetRow + 1, targetCol - 1] = null;
                    gameData.Board[targetRow, targetCol] = null;

                    gameData.computerLocations[locationIndex].Row = targetRow + 2;
                    gameData.computerLocations[locationIndex].Col = targetCol - 2;
                    for (int i = 0; i < gameData.playerLocations.Count; i++)
                    {
                        if (gameData.playerLocations[i].Row == midPiece.RowIndex && gameData.playerLocations[i].Col == midPiece.ColIndex)
                        {
                            gameData.playerLocations.RemoveAt(i);
                        }
                    }

                    return true;
                }
            }
            return false;
        }
    }
}
