using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    //An object that represents the board of the game displayed on screen
    internal class BoardGrid
    {

        int m_width;
        int m_height;
        int m_NoOfRows;
        int m_NoOfCols;

        const int DEFAULT_X_OFFSET = 500;
        const int DEFAULT_Y_OFFSET = 100;
        const int DEFAULT_NO_ROWS = 8;
        const int DEFAULT_NO_COLS = 4;
        const int DEFAULT_WIDTH = 60;
        const int DEFAULT_HEIGHT = 60;

        public BoardGrid()
        {
            m_NoOfRows = DEFAULT_NO_ROWS;
            m_NoOfCols = DEFAULT_NO_COLS;
            m_width = DEFAULT_WIDTH;
            m_height = DEFAULT_HEIGHT;
        }

        public void DrawGrid(Graphics gfx)
        {
            Graphics boardLayout = gfx;
            Pen layoutPen = new Pen(Color.Black);

            int X = DEFAULT_X_OFFSET;
            int Y = DEFAULT_Y_OFFSET;

            // Draw horizontal line for matrix
            for (int i = 0; i <= m_NoOfRows; i++)
            {
                boardLayout.DrawLine(layoutPen, X, Y, X + this.m_width * this.m_NoOfCols, Y);
                Y = Y + m_height;
            }

            //reset the matrix x and y to default values
            X = DEFAULT_X_OFFSET;
            Y = DEFAULT_Y_OFFSET;

            // Draw vertical line for matrix
            for (int j = 0; j <= m_NoOfCols; j++)
            {
                boardLayout.DrawLine(layoutPen, X, Y, X, Y + this.m_height * this.m_NoOfRows);
                X = X + m_width;
            }
        }
    }
}
