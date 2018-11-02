using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public class HexGrid
    {
        public int Rows;
        public int Cols;
        public Hex[] HexArray;

        public Hex[,] HexStorage;
        public HexGrid(int r, int q)
        {
            this.Rows = r;
            this.Cols = q;

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
