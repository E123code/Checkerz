using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    //Abstract class that is the basis for both player and computer controllers
    abstract class Controller
    {
        protected readonly GameData gameData;
        protected readonly GameLogic gameLogic;

        protected const int ROWNUMBER = 8;
        protected const int COLNNUMBER = 4;

        public Controller(GameData data, GameLogic logic)
        {
            gameData = data;
            gameLogic = logic;
        }

    }
}
