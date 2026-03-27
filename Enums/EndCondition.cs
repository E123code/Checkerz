using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    public enum EndCondition
    {
        // Win Conditions
        PlayerReachedTop,
        CapturedAllComputerPieces,
        ComputerBlocked,

        // Loss Conditions
        TimerRanOut,
        ComputerReachedBottom,
        CapturedAllPlayerPieces,
        PlayerBlocked
    }
}
