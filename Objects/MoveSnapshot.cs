using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    // Object that represents a snapshot of a game that will be displayed during a replay
    internal class MoveSnapshot
    {
        public int GameID { get; set; }
        public int MoveNumber {  get; set; }
        public List<BoardLocation> PlayerLocations {  get; set; }
        public List<BoardLocation> ComputerLocations { get; set; }
        public int StartRow {  get; set; }
        public int StartCol { get; set; }
        public int TargetRow { get; set; }
        public int TargetCol {  get; set; }

        public MoveSnapshot(int gameID, int moveNumber, List<BoardLocation> playerLocations, List<BoardLocation> computerLocations, int startRow, int startCol, int targetRow, int targetCol)
        {
            MoveNumber = moveNumber;
            GameID = gameID;
            PlayerLocations = playerLocations;
            ComputerLocations = computerLocations;

            StartRow = startRow;
            StartCol = startCol;

            TargetRow = targetRow;
            TargetCol = targetCol;
        }

        //Setting the snapshot after move is completed
        public void UpdateSnapshot(List<BoardLocation> playerLocations, List<BoardLocation> computerLocations, int startRow, int startCol, int targetRow, int targetCol)
        {
            MoveNumber++;
            PlayerLocations = playerLocations;
            ComputerLocations = computerLocations;
            StartRow = startRow;
            StartCol = startCol;
            TargetRow = targetRow;
            TargetCol = targetCol;
        }
    }
}
