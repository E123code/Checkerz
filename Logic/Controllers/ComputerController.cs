using CheckerZ.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    // A child of controller that handles the moves of the computer(in response for the request from the server)
    internal class ComputerController : Controller

    {

        public ComputerController(GameData data, GameLogic logic) : base(data, logic) { }

        //Handles the next move dependent on selected action of the servers
        public bool ComputerMove(MoveCommand moveCommand, out Piece movedPiece, out int startRow, out int startCol)
        {
            movedPiece = null;
            startRow = 0;
            startCol = 0;

            if (moveCommand != null)
            {
                Piece targetPiece = gameData.Board[moveCommand.StartRow, moveCommand.StartCol];
                startRow = moveCommand.StartRow; startCol = moveCommand.StartCol;
                switch (moveCommand.Action)
                {
                    case Enums.GameAction.CaptureRight:
                        Capture(targetPiece, moveCommand, moveCommand.StartRow + 1, moveCommand.StartCol + 1);
                        break;

                    case Enums.GameAction.CaptureLeft:
                        Capture(targetPiece, moveCommand, moveCommand.StartRow + 1, moveCommand.StartCol - 1);
                        break;
                    default:
                        Move(targetPiece, moveCommand);
                        break;
                }
                movedPiece = targetPiece;
                return true;
            }
            return false;
        }
        // Handles the captures of the computer
        public void Capture(Piece targetPiece, MoveCommand moveCommand, int midRow, int midCol)
        {
            targetPiece.RowIndex = moveCommand.TargetRow; targetPiece.ColIndex = moveCommand.TargetCol;
            gameData.Board[moveCommand.TargetRow, moveCommand.TargetCol] = targetPiece;
            gameData.Board[midRow, midCol] = null;
            gameData.Board[moveCommand.StartRow, moveCommand.StartCol] = null;

            //search for the target piece in computer locations and updates the list
            for (int i = 0; i < gameData.computerLocations.Count; i++)
            {
                if (gameData.computerLocations[i].Row == moveCommand.StartRow && gameData.computerLocations[i].Col == moveCommand.StartCol)
                {
                    gameData.computerLocations[i].Row = targetPiece.RowIndex;
                    gameData.computerLocations[i].Col = targetPiece.ColIndex;
                }
            }

            //searches for captured piece in player locations and removes the player locations
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == midRow && gameData.playerLocations[i].Col == midCol)
                {
                    gameData.playerLocations.RemoveAt(i);
                }
            }
        }
        // Handles piece movements of the computer(except for the captures)
        public void Move(Piece targetPiece, MoveCommand moveCommand)
        {
            targetPiece.RowIndex = moveCommand.TargetRow; targetPiece.ColIndex = moveCommand.TargetCol;
            int pieceIndex = 0;
            //search for the target piece in computer locations and updates the list
            for (int i = 0; i < gameData.computerLocations.Count; i++)
            {
                if (gameData.computerLocations[i].Row == moveCommand.StartRow && gameData.computerLocations[i].Col == moveCommand.StartCol)
                {
                    gameData.computerLocations[i].Row = targetPiece.RowIndex;
                    gameData.computerLocations[i].Col = targetPiece.ColIndex;
                    pieceIndex = i;
                }
            }
            if ((moveCommand.Action == Enums.GameAction.UpRight
                || moveCommand.Action == Enums.GameAction.UpLeft)
                && !gameData.computerLocations[pieceIndex].isReversed)
                gameData.computerLocations[pieceIndex].isReversed = true;
            gameData.Board[moveCommand.TargetRow, moveCommand.TargetCol] = targetPiece;
            gameData.Board[moveCommand.StartRow, moveCommand.StartCol] = null;

        }
    }
}
