using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ.Client_Server
{
    internal class GameStateRequest
    {
        public List<BoardLocation> PlayerLocations { get; set; }
        public List<BoardLocation> ComputerLocations { get; set; }
    }
}
