using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    //Enum describing end conditions(Win and loss conditions)
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
