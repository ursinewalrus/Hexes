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
        public string MapName;

        //tile: {datakey:dataval}
        private Dictionary<string, Dictionary<string, string>> TileData = new Dictionary<string, Dictionary<string, string>>();
        //x-y: {tile}
        private Dictionary<string, string> SpecificPlacements = new Dictionary<string, string>();
        //{tile: odds}
        //private Dictionary<string, int> TileOdds = new Dictionary<string, int>();
        //private int TotalTileOdds = 0;
        private string DefaultTile;
        public int Rows;
        public int Cols;
        public float HexXSize = 100;
        public float HexYSize = 100;
       

        public Hex[,] HexStorage;
        //Axial coordinate rectangle
        //https://www.redblobgames.com/grids/hexagons/#map-storage
       // public HexGrid(int r, int q, float hexXSize, float hexYSize, Texture2D texture = null, int baseXOffset = 0 , int baseYOffset = 0)
        public HexGrid(Dictionary<string, Dictionary<string, string>> mapData, Dictionary<string, Dictionary<string, string>> tileData, string moduleName)
        {
            //FileStream fs = new FileStream(@"Content/greenhex.png", FileMode.Open);
            //Texture2D background1 = Texture2D.FromStream(GraphicsDevice, fs);
            //fs.Dispose();

            TileData = tileData;
            MapName = mapData.First().Key;

            AsignMapData(mapData[MapName]);
            FillGrid(tileData, moduleName);

            //AsignTileData();
            //Rows = r;
            //Cols = q;
            //HexXSize = hexXSize;
            //HexYSize = hexYSize;
            //HexStorage = new Hex[Rows, Cols];
            //Texture = texture;
            //BaseXOffset = baseXOffset;
            //BaseYOffset = baseYOffset;
            ;
        }

        public void AsignMapData(Dictionary<string, string> mapData)
        {
            Rows = Convert.ToInt32(mapData["xSize"].Trim());
            Cols = Convert.ToInt32(mapData["ySize"].Trim());
            HexStorage = new Hex[Rows, Cols];
            //set the relative odds for each tile and update the total odds
            //mapData["tileTypes"].Split(',').ToList().ForEach( (t) =>
            //{
            //    string tileType = t.Split(':')[0];
            //    int odds = Convert.ToInt32(t.Split(':')[1]);
            //    TotalTileOdds =+ odds;
            //    TileOdds[tileType] = odds;
            //});
            //set specific placements for tiles
            mapData["specificPlacements"].Split(new string[] { "),(" }, StringSplitOptions.None).ToList().ForEach( (t) =>
            {
                List<string> placementParams = t.Split(',').ToList();
                string x = placementParams[0].Trim(new[] { ' ', ')', '(' });
                string y = placementParams[1].Trim();
                string type = placementParams[2].Trim(new[] { ' ', ')', '(' });
                SpecificPlacements[x + '-' + y] = type;
            });
            DefaultTile = mapData["defaultTile"];

        }

        private void FillGrid(Dictionary<string, Dictionary<string, string>> tileData, string moduleName)
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var q = 0; q < Cols; q++)
                {
                    int hexR = r;
                    int hexQ = q - (r / 2);
                    string placementKey = hexR.ToString() + '-' + hexQ.ToString();
                    if (SpecificPlacements.ContainsKey(placementKey))
                    {
                        Hex hex = new Hex(hexR, hexQ, placementKey, TileData[SpecificPlacements[placementKey]],moduleName);
                        HexStorage[hexR, hexQ - -1 * (hexR / 2)] = hex;
                    }
                    else
                    {
                        Hex hex = new Hex(hexR, hexQ, DefaultTile, TileData[DefaultTile], moduleName);
                        HexStorage[hexR, hexQ - -1 * (hexR / 2)] = hex;
                    }
                    //Hex hex = new Hex(hexR, hexQ, HexXSize, HexYSize, Texture);
                    //HexStorage[hexR, hexQ - -1*(hexR / 2)] = hex;
                }
            }
        }

        public void SelectedHex(Vector2 screenCordinates)
        {
            var Q = Math.Round(((Math.Sqrt(3) / 3 * screenCordinates.X) - (1.0f/3.0f * screenCordinates.Y)) / HexXSize);
            var R = Math.Round((2.0f/3.0f * screenCordinates.Y) / HexYSize);
            //convert to cube cordinates
            //return new Point(Q,R)

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
