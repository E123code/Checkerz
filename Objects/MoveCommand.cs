using CheckerZ.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ.Objects
{
    internal class MoveCommand
    {
        public GameAction Action { get; set; }

        // The server points to the exact piece using Grid Math! No pixels!
        public int StartRow { get; set; }
        public int StartCol { get; set; }

        // Where it lands
        public int TargetRow { get; set; }
        public int TargetCol { get; set; }
    }
}
