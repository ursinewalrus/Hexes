using Hexes.Actors;
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
        #region properties
        public string MapName;

        //tile: {datakey:dataval}
        private Dictionary<string, Dictionary<string, string>> TileData = new Dictionary<string, Dictionary<string, string>>();
        //x-y: {tile}
        private Dictionary<string, string> SpecificTilePlacements = new Dictionary<string, string>();
        //{tile: odds}
        //private Dictionary<string, int> TileOdds = new Dictionary<string, int>();
        //private int TotalTileOdds = 0;
        private string DefaultTile;
        public int Rows;
        public int Cols;
        public float HexXSize = 100;
        public float HexYSize = 100;
       

        public Hex[,] HexStorage;
        public Dictionary<HexPoint,AbstractActor> ActorStorage;
        #endregion
        //Axial coordinate rectangle
        //https://www.redblobgames.com/grids/hexagons/#map-storage
        public HexGrid(Dictionary<string, Dictionary<string, string>> mapData, Dictionary<string, Dictionary<string, string>> tileData, string moduleName)
        {
            TileData = tileData;
            MapName = mapData.First().Key;

            AsignMapData(mapData[MapName]);
            FillGrid(tileData, moduleName);
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
            mapData["specificTilePlacements"].Split(new string[] { "),(" }, StringSplitOptions.None).ToList().ForEach( (t) =>
            {
                List<string> placementParams = t.Split(',').ToList();
                string x = placementParams[0].Trim(new[] { ' ', ')', '(' });
                string y = placementParams[1].Trim();
                string type = placementParams[2].Trim(new[] { ' ', ')', '(' });
                SpecificTilePlacements[x + '-' + y] = type;
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
                    if (SpecificTilePlacements.ContainsKey(placementKey))
                    {
                        Hex hex = new Hex(new HexPoint(hexR,hexQ), placementKey, TileData[SpecificTilePlacements[placementKey]],moduleName);
                        HexStorage[hexR, hexQ - -1 * (hexR / 2)] = hex;
                    }
                    else
                    {
                        Hex hex = new Hex(new HexPoint(hexR, hexQ), DefaultTile, TileData[DefaultTile], moduleName);
                        HexStorage[hexR, hexQ - -1 * (hexR / 2)] = hex;
                    }
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


        public List<HexPoint> InRadiusOf(HexPoint hexPoint)
        {
            return new List<HexPoint>();
        }

        #region static methods
        public static int HexDistance(HexPoint hex1, HexPoint hex2)
        {
            //https://www.redblobgames.com/grids/hexagons/#range
            //get cube cord third value
            int cubeValHex1 = -hex1.Q - hex1.R;
            int cubeValHex2 = -hex2.Q - hex2.R;
            //return max(abs(a.x - b.x), abs(a.y - b.y), abs(a.z - b.z))
            return Math.Max(Math.Max(Math.Abs(hex1.Q - hex2.Q), Math.Abs(hex1.R - hex2.R)), Math.Abs(cubeValHex1 - cubeValHex2));
        }
        public static Point ConvertFromHexToGridCords(HexPoint hexPoint)
        {
            return new Point(hexPoint.R, hexPoint.Q - -1 * (hexPoint.R / 2));
        }
        #endregion
    }
}
/*
 * function cube_to_axial(cube):
    var q = cube.x
    var r = cube.z
    return Hex(q, r)

function axial_to_cube(hex):
    var x = hex.q
    var z = hex.r
    var y = -x-z
    return Cube(x, y, z)
    */