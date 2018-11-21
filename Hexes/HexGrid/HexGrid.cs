using Hexes.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public class HexGrid : IDrawable
    {
        public int Rows;
        public int Cols;
        public int GameWidth;

        public Hex[,] HexStorage;
        //Axial coordinate rectangle
        //https://www.redblobgames.com/grids/hexagons/#map-storage
        public HexGrid(int r, int q)
        {
            Rows = r;
            Cols = q;
            Hex.gameWidth = GameWidth;
            HexStorage = new Hex[Rows, Cols];
            FillGrid();
            ;
        }

        private void FillGrid()
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var q = 0; q < Cols; q++)
                {
                    int hexR = r;
                    int hexQ = q - (r / 2);
                    Hex hex = new Hex(hexR, hexQ);
                    HexStorage[hexR, hexQ - -1*(hexR / 2)] = hex;
                }
            }
        }

    }
}
