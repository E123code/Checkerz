using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ.Client_Server
{
    //Class to send the current game state of the board locations for the server.
    internal class GameStateRequest
    {
        public List<BoardLocation> PlayerLocations { get; set; }
        public List<BoardLocation> ComputerLocations { get; set; }
    }
}
