using Hexes.Actors;
using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.HexGrid
{
    public class HexGrid : IDrawable
    {
        #region properties
        public string MapName;

        //tile: {datakey:dataval}
        private Dictionary<string, Dictionary<string, string>> TileData = new Dictionary<string, Dictionary<string, string>>();
        //x-y: {tile}
        private Dictionary<Point, string> SpecificTilePlacements = new Dictionary<Point, string>();
        private Dictionary<Point, string> SpecificActorPlacements = new Dictionary<Point, string>();
        //{tile: odds}
        //private Dictionary<string, int> TileOdds = new Dictionary<string, int>();
        //private int TotalTileOdds = 0;
        private string DefaultTile;
        public int Rows;
        public int Cols;
        public float HexXSize = 100;
        public float HexYSize = 100;

        public Dictionary<HexPoint, Hex> HexStorage = new  Dictionary<HexPoint, Hex>();
        public Dictionary<HexPoint,BasicActor> ActorStorage = new Dictionary<HexPoint, BasicActor>();
        #endregion

        #region  selectable properties

        public Hex ActiveHex;
        public BasicActor ActiveActor;

        #endregion
        //Axial coordinate rectangle
        //https://www.redblobgames.com/grids/hexagons/#map-storage
        public HexGrid(Dictionary<string, Dictionary<string, string>> mapData, Dictionary<string, Dictionary<string, string>> tileData, Dictionary<string, Dictionary<string, string>> actorData, string moduleName)
        {
            TileData = tileData;
            MapName = mapData.First().Key;

            AsignMapData(mapData[MapName]);
            //AsignActorData(actorData);
            FillGrid(tileData, actorData, moduleName);
            ;
        }

        //public void AsignActorData(Dictionary<string, Dictionary<string, string>> actorData)
        //{
        //    actorData
        //}

        public void AsignMapData(Dictionary<string, string> mapData)
        {
            Rows = Convert.ToInt32(mapData["xSize"].Trim());
            Cols = Convert.ToInt32(mapData["ySize"].Trim());
            //HexStorage = new Hex[Rows, Cols];
            //set the relative odds for each tile and update the total odds
            //mapData["tileTypes"].Split(',').ToList().ForEach( (t) =>
            //{
            //    string tileType = t.Split(':')[0];
            //    int odds = Convert.ToInt32(t.Split(':')[1]);
            //    TotalTileOdds =+ odds;
            //    TileOdds[tileType] = odds;
            //});
            //set specific placements for tiles

            //could probably cut this stuff out and put it into FillGrid TODO->little redundant
            mapData["specificTilePlacements"].Split(new string[] { "),(" }, StringSplitOptions.None).ToList().ForEach( (t) =>
            {
                List<string> placementParams = t.Split(',').ToList();
                int x = Convert.ToInt32(placementParams[0].Trim(new[] { ' ', ')', '(' }));
                int y = Convert.ToInt32(placementParams[1].Trim());
                string type = placementParams[2].Trim(new[] { ' ', ')', '(' });
                SpecificTilePlacements[new Point(x,y)] = type;
            });
            //set specific placements for actors
            mapData["specificActorPlacements"].Split(new string[] { "),(" }, StringSplitOptions.None).ToList().ForEach((t) =>
            {
                List<string> placementParams = t.Split(',').ToList();
                int x = Convert.ToInt32(placementParams[0].Trim(new[] { ' ', ')', '(' }));
                int y = Convert.ToInt32(placementParams[1].Trim());
                string type = placementParams[2].Trim(new[] { ' ', ')', '(' });
                string controllStatus = placementParams[3].Trim(new[] { ' ', ')', '(' });
                string rotation = placementParams[4].Trim(new[] { ' ', ')', '(' });
                SpecificActorPlacements[new Point(x, y)] = type + "-" + controllStatus + '-' +  rotation;
            });

            DefaultTile = mapData["defaultTile"];

        }

        private void FillGrid(Dictionary<string, Dictionary<string, string>> tileData, Dictionary<string, Dictionary<string, string>> actorData, string moduleName)
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var q = 0; q < Cols; q++)
                {
                    int hexR = r;
                    int hexQ = q - (r / 2);
                    //placing tiles into drawable storage
                    var tilePlacementKeyExists = SpecificTilePlacements.Where(p => p.Key.X == hexR && p.Key.Y == hexQ).ToList();
                    if (tilePlacementKeyExists.Any())
                    {
                        var placementKey = tilePlacementKeyExists[0];
                        Hex hex = new Hex(new HexPoint(hexR,hexQ), SpecificTilePlacements[placementKey.Key], TileData[SpecificTilePlacements[placementKey.Key]],moduleName);
                        //HexStorage[hexR, hexQ - -1 * (hexR / 2)] = hex;
                        HexStorage[new HexPoint(hexR, hexQ)] = hex;

                    }
                    else
                    {
                        Hex hex = new Hex(new HexPoint(hexR, hexQ), DefaultTile, TileData[DefaultTile], moduleName);
                        //HexStorage[hexR, hexQ - -1 * (hexR / 2)] = hex;
                        HexStorage[new HexPoint(hexR, hexQ)] = hex;

                    }
                    //placing actors into actor storage
                    var actorPlacementKeyExists = SpecificActorPlacements.Where(p => p.Key.X == hexR && p.Key.Y == hexQ).ToList();
                    if (actorPlacementKeyExists.Any())
                    {
                        var placementKey = actorPlacementKeyExists[0];
                        string actorType = placementKey.Value.Split('-')[0];
                        bool controllable = placementKey.Value.Split('-')[1] == "PC" ? true : false;
                        int rotation = Convert.ToInt32(placementKey.Value.Split('-')[2]);
                        BasicActor actor = new BasicActor(new HexPoint(hexR, hexQ), actorType, actorData[actorType], rotation, controllable, moduleName);
                        ActorStorage[new HexPoint(hexR, hexQ)] = actor;
                    }
                }
            }
        }

        public HexPoint SelectedHex(Vector2 screenCordinates)
        {
            var R = (int)Math.Round(((Math.Sqrt(3) / 3 * screenCordinates.X) - (1.0f/3.0f * screenCordinates.Y)) / HexXSize);
            var Q = (int)Math.Round((2.0f/3.0f * screenCordinates.Y) / HexYSize);
            if (!(R > 0 || R < Rows || (Q < Rows / 2 && Q > -1*(Rows / 2) )))
                return null;
            return new HexPoint(Q,R);
        }

        public void Draw()
        {
            foreach(HexPoint hex in HexStorage.Keys)
            {
                HexStorage[hex].Draw();
            }
            foreach(HexPoint hex in ActorStorage.Keys)
            {
                var hexExists = HexStorage.Where(p => p.Key.R == hex.R && p.Key.Q == hex.Q).ToList();
                if(hexExists.Any())
                {
                    var drawHex = hexExists[0];
                    //send it the center of the hex it will be in to center it
                    ActorStorage[hex].Draw(drawHex.Value.Center);
                }
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
        public void Draw(FloatPoint center)
        {
        }
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