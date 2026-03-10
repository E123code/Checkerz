namespace CheckerZ
{
    internal class GameLogic
    {
        private readonly GameData gameData;

        // We pass constants here, or you can put them in GameData
        const int ROWNUMBER = 8;
        const int COLNNUMBER = 4;

        public GameLogic(GameData data)
        {
            gameData = data;
        }

        // check if the player won the game
        public bool CheckWin()
        {
            //
            if (gameData.computerLocations.Count == 0) return true;

            for (int i = 0; i < COLNNUMBER; i++)
            {
                if (gameData.Board[0, i] != null && gameData.Board[0, i].IsPlayer)
                    return true;
            }
            return false;
        }

        public bool CheckLose()
        {
            if (gameData.playerLocations.Count == 0) return true;

            for (int i = 0; i < COLNNUMBER; i++)
            {
                if (gameData.Board[7, i] != null && !gameData.Board[7, i].IsPlayer)
                    return true;
            }

            if (!CheckPossibleMovesForPlayer()) return true;

            return false;
        }

        public bool CheckPossibleMovesForPlayer()
        {
            // ... (Paste your exact CheckPossibleMovesForPlayer logic here) ...
            // It uses gameData perfectly, so no changes are needed to the inner loop!
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                int targetRow = gameData.playerLocations[i].Row;
                int targetCol = gameData.playerLocations[i].Col;
                Piece targetPiece = gameData.Board[targetRow, targetCol];

                // check if capture upright is possible
                if (targetRow - 2 >= 0 && targetCol + 2 < COLNNUMBER)
                {
                    Piece midPiece = gameData.Board[targetRow - 1, targetCol + 1];
                    // Is there a player piece in the way and is the landing spot empty?
                    if (midPiece != null && !midPiece.IsPlayer && gameData.Board[targetRow - 2, targetCol + 2] == null)
                        return true;
                }

                // check if capture upleft is possible
                if (targetRow - 2 >= 0 && targetCol - 2 >= 0)
                {
                    Piece midPiece = gameData.Board[targetRow - 1, targetCol - 1];
                    // Is there a player piece in the way and is the landing spot empty?
                    if (midPiece != null && !midPiece.IsPlayer && gameData.Board[targetRow - 2, targetCol - 2] == null)
                        return true;
                }

                // check if move upright is possible
                if (targetRow - 1 >= 0 && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow - 1, targetCol + 1] == null)
                    return true;

                // check if move upleft is possible
                if (targetRow - 1 >= 0 && targetCol - 1 >= 0 && gameData.Board[targetRow - 1, targetCol - 1] == null)
                    return true;

                // check if move downright is possible
                if (!targetPiece.Reversed && targetRow + 1 < ROWNUMBER && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow + 1, targetCol + 1] == null)
                    return true;

                // check if move downleft is possible
                if (!targetPiece.Reversed && targetRow + 1 < ROWNUMBER && targetCol - 1 >= 0 && gameData.Board[targetRow + 1, targetCol - 1] == null)
                    return true;
            }
            return false;
        }
    }
}
