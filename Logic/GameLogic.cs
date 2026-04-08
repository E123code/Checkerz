namespace CheckerZ
{
    internal class GameLogic
    {
        // Class to handle the logical operations of the game
        private readonly GameData gameData;

        // We pass constants here, or you can put them in GameData
        const int ROWNUMBER = 8;
        const int COLNNUMBER = 4;

        public GameLogic(GameData data)
        {
            gameData = data;
        }

        // check if the player won the game
        public bool CheckWin(out string outcome,out string endCondition)
        {
            //check if player captured all computer pieces
            if (gameData.computerLocations.Count == 0)
            {
                outcome = GameOutcome.Win.ToString();
                endCondition = EndCondition.CapturedAllComputerPieces.ToString();
                return true;
            }
            //check if player piece reached the top of the board
            for (int i = 0; i < COLNNUMBER; i++)
            {
                if (gameData.Board[0, i] != null && gameData.Board[0, i].IsPlayer)
                {
                    outcome = GameOutcome.Win.ToString();
                    endCondition = EndCondition.PlayerReachedTop.ToString();
                    return true;
                }
            }
            outcome = null;
            endCondition = null;
            return false;
        }
        // check if the player lost the game
        public bool CheckLose(out string outcome, out string endCondition)
        {
            //check if computer captured all player pieces
            if (gameData.playerLocations.Count == 0)
            {
                outcome = GameOutcome.Loss.ToString();
                endCondition = EndCondition.CapturedAllPlayerPieces.ToString();
                return true;
            }

            //check if computer piece reached the bottom of the board
            for (int i = 0; i < COLNNUMBER; i++)
            {
                if (gameData.Board[7, i] != null && !gameData.Board[7, i].IsPlayer)
                {
                    outcome = GameOutcome.Loss.ToString();
                    endCondition = EndCondition.ComputerReachedBottom.ToString();
                    return true;
                }
            }
            // check if player is blocked from any possible move
            if (!CheckPossibleMovesForPlayer())
            {
                outcome = GameOutcome.Loss.ToString();
                endCondition = EndCondition.PlayerBlocked.ToString();
                return true;
            }

            outcome = null;
            endCondition = null;
            return false;
        }
        // Checks all possible moves for player to see if the game can continue and if not the player loses
        public bool CheckPossibleMovesForPlayer()
        {
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                int targetRow = gameData.playerLocations[i].Row;
                int targetCol = gameData.playerLocations[i].Col;
                Piece targetPiece = gameData.Board[targetRow, targetCol];

                // check if capture upright is possible
                if (targetRow - 2 >= 0 && targetCol + 2 < COLNNUMBER)
                {
                    Piece midPiece = gameData.Board[targetRow - 1, targetCol + 1];
                    if (midPiece != null && !midPiece.IsPlayer && gameData.Board[targetRow - 2, targetCol + 2] == null)
                        return true;
                }

                // check if capture upleft is possible
                if (targetRow - 2 >= 0 && targetCol - 2 >= 0)
                {
                    Piece midPiece = gameData.Board[targetRow - 1, targetCol - 1];
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
                if (!gameData.playerLocations[i].isReversed && targetRow + 1 < ROWNUMBER && targetCol + 1 < COLNNUMBER && gameData.Board[targetRow + 1, targetCol + 1] == null)
                    return true;

                // check if move downleft is possible
                if (!gameData.playerLocations[i].isReversed && targetRow + 1 < ROWNUMBER && targetCol - 1 >= 0 && gameData.Board[targetRow + 1, targetCol - 1] == null)
                    return true;
            }
            return false;
        }
    }
}
