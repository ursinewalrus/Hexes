using Hexes.Geometry;
using Microsoft.Xna.Framework;
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
        public float HexXSize;
        public float HexYSize;
        Texture2D Texture;

        public Hex[,] HexStorage;
        //Axial coordinate rectangle
        //https://www.redblobgames.com/grids/hexagons/#map-storage
        public HexGrid(int r, int q, float hexXSize, float hexYSize, Texture2D texture = null, int baseXOffset = 0 , int baseYOffset = 0)
        {
            Rows = r;
            Cols = q;
            HexXSize = hexXSize;
            HexYSize = hexYSize;
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
                    Hex hex = new Hex(hexR, hexQ, HexXSize, HexYSize, Texture);
                    HexStorage[hexR, hexQ - -1*(hexR / 2)] = hex;
                }
            }
        }

        public void SelectedHex(Vector2 screenCordinates)
        {
            var Q = Math.Round(((Math.Sqrt(3) / 3 * screenCordinates.X) - (1.0f/3.0f * screenCordinates.Y)) / HexXSize);
            var R = Math.Round((2.0f/3.0f * screenCordinates.Y) / HexYSize);
            //convert to cube cordinates
           

            ;
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
