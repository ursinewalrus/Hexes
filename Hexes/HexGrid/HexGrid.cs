using Hexes.Geometry;
using Microsoft.Xna.Framework.Graphics;
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
        int BaseYOffset;
        int BaseXOffset;
        Texture2D Texture;

        public Hex[,] HexStorage;
        //Axial coordinate rectangle
        //https://www.redblobgames.com/grids/hexagons/#map-storage
        public HexGrid(int r, int q, Texture2D texture, int baseXOffset = 0 , int baseYOffset = 0)
        {
            Rows = r;
            Cols = q;
            Hex.gameWidth = GameWidth;
            HexStorage = new Hex[Rows, Cols];
            Texture = texture;
            FillGrid();
            BaseXOffset = baseXOffset;
            BaseYOffset = baseYOffset;
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
                    Hex hex = new Hex(hexR, hexQ, Texture);
                    HexStorage[hexR, hexQ - -1*(hexR / 2)] = hex;
                }
            }
        }
        public void Draw()
        {
            foreach(Hex hex in HexStorage)
            {
                hex.Draw();
            }
        }

    }
}
