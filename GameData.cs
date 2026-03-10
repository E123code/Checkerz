using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    internal class GameData
    {
        public Piece[,] Board = {{null,new Piece(0,1,false,false),null,new Piece(0,3,false,false)},
                          {new Piece(1,0,false,false),null,new Piece(1,2,false,false),null},
                          {null,null,null,null},
                          {null,null,null,null},
                          {null,null,null,null},
                          {null,null,null,null},
                          {null,new Piece(6,1,true,false),null,new Piece(6,3,true,false)},
                          {new Piece(7,0,true,false),null,new Piece(7,2,true,false),null}};

        public List<BoardLocation> computerLocations = new List<BoardLocation>() { new BoardLocation(0, 1), new BoardLocation(0, 3), new BoardLocation(1, 0), new BoardLocation(1, 2) };

        public List<BoardLocation> playerLocations = new List<BoardLocation>() { new BoardLocation(6, 1), new BoardLocation(6, 3), new BoardLocation(7, 0), new BoardLocation(7, 2) };
    }
}
