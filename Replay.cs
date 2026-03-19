using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    internal class Replay
    {
        public int TurnCounter {  get; set; }
        public DateTime DatePlayed { get; set; } = DateTime.Now;

        // The win/loss data you requested
        public GameOutcome Outcome { get; set; }
        public EndCondition EndCondition { get; set; }

        // The history of the game
        public List<TurnSnapshot> History { get; set; } = new List<TurnSnapshot>();

        public void RecordCurrentState(List<BoardLocation> playerLocations, List<BoardLocation> computerLocations ,Piece movedPiece, int moveRow, int moveCol)
        {
            History.Add(new TurnSnapshot(playerLocations, computerLocations, movedPiece, moveRow, moveCol));
        }
    }
}
