using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    //class that contains the required data for the game to operate:
    // includes the board with the actual pieces on the 8x4 matrix.
    // includes both lists of player and computer locations for efficant usage of piece locations in game logic
    internal class GameData
    {

        public Piece[,] Board = {{null,new Piece(0,1,false),null,new Piece(0,3,false)},
                          {new Piece(1,0,false),null,new Piece(1,2,false),null},
                          {null,null,null,null},
                          {null,null,null,null},
                          {null,null,null,null},
                          {null,null,null,null},
                          {null,new Piece(6,1,true),null,new Piece(6,3,true)},
                          {new Piece(7,0,true),null,new Piece(7,2,true),null}};

        public List<BoardLocation> computerLocations = new List<BoardLocation>() { new BoardLocation(0, 1), new BoardLocation(0, 3), new BoardLocation(1, 0), new BoardLocation(1, 2) };

        public List<BoardLocation> playerLocations = new List<BoardLocation>() { new BoardLocation(6, 1), new BoardLocation(6, 3), new BoardLocation(7, 0), new BoardLocation(7, 2) };
    }
}
