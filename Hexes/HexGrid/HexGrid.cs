using Hexes.Actors;
using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.UI;

namespace Hexes.HexGrid
{
    public class HexGrid : IDrawable
    {
        #region properties
        public string MapName;

        //tile: {datakey:dataval}
        private Dictionary<string, Dictionary<string, string>> TileData = new Dictionary<string, Dictionary<string, string>>();
        //x-y: {tile}
        private Dictionary<HexPoint, string> SpecificTilePlacements = new Dictionary<HexPoint, string>();
        private Dictionary<HexPoint, string> SpecificActorPlacements = new Dictionary<HexPoint, string>();
        //{tile: odds}
        //private Dictionary<string, int> TileOdds = new Dictionary<string, int>();
        //private int TotalTileOdds = 0;
        private string DefaultTile;
        public int Rows;
        public int Cols;
        public float HexXSize = 100;
        public float HexYSize = 100;
        private Color RadiusColor;

        public Dictionary<HexPoint, Hex> HexStorage = new  Dictionary<HexPoint, Hex>();
        public List <BasicActor> ActorStorage = new List<BasicActor>();
        #endregion
        #region  selectable properties

        public KeyValuePair<HexPoint, Hex> ActiveHex;
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
                int r = Convert.ToInt32(placementParams[0].Trim(new[] { ' ', ')', '(' }));
                int q = Convert.ToInt32(placementParams[1].Trim());
                string type = placementParams[2].Trim(new[] { ' ', ')', '(' });
                SpecificTilePlacements[new HexPoint(r,q)] = type;
            });
            //set specific placements for actors
            mapData["specificActorPlacements"].Split(new string[] { "),(" }, StringSplitOptions.None).ToList().ForEach((t) =>
            {
                List<string> placementParams = t.Split(',').ToList();
                int r = Convert.ToInt32(placementParams[0].Trim(new[] { ' ', ')', '(' }));
                int q = Convert.ToInt32(placementParams[1].Trim());
                string type = placementParams[2].Trim(new[] { ' ', ')', '(' });
                string controllStatus = placementParams[3].Trim(new[] { ' ', ')', '(' });
                string rotation = placementParams[4].Trim(new[] { ' ', ')', '(' });
                SpecificActorPlacements[new HexPoint(r, q)] = type + "-" + controllStatus + '-' +  rotation;
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
                    var tilePlacementKeyExists = SpecificTilePlacements.Where(p => p.Key.Equals(new HexPoint(hexR, hexQ))).ToList();
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
                    var actorPlacementKeyExists = SpecificActorPlacements.Where(p => p.Key.Equals(new HexPoint(hexR, hexQ))).ToList();
                    if (actorPlacementKeyExists.Any())
                    {
                        var placementKey = actorPlacementKeyExists[0];
                        string actorType = placementKey.Value.Split('-')[0];
                        bool controllable = placementKey.Value.Split('-')[1] == "PC" ? true : false;
                        int rotation = Convert.ToInt32(placementKey.Value.Split('-')[2]);
                        BasicActor actor = new BasicActor(new HexPoint(hexR, hexQ), actorType, actorData[actorType], rotation, controllable, moduleName);
                        ActorStorage.Add(actor);
                    }
                }
            }
        }

        public HexPoint SelectedHex(Vector2 screenCordinates)
        {
            var R = (int)Math.Round((2.0f / 3.0f * screenCordinates.Y) / HexYSize);
            var Q = (int)Math.Round(((Math.Sqrt(3) / 3 * screenCordinates.X) - (1.0f/3.0f * screenCordinates.Y)) / HexXSize);
            var possiblePoint = new HexPoint(R,Q);
            var inGrid = HexStorage.Any(h => h.Key.Equals(possiblePoint));
            if (!inGrid)
                return null;
            return possiblePoint;
        }

        public void Draw()
        {
            foreach(HexPoint hex in HexStorage.Keys)
            {
                HexStorage[hex].Draw();
            }
            foreach (BasicActor actor in ActorStorage)
            {
                var onHex = HexStorage.Where(h => h.Key.Equals(new HexPoint(actor.Location.R, actor.Location.Q))).FirstOrDefault();
                if(onHex.Key != null)
                    actor.Draw(onHex.Value.Center);
            }
        }

        public List<HexPoint> AllInRadiusOf(HexPoint hexPoint, int radius)
        {
            //probs exclude self
            var inRadius = new List<HexPoint>();
            var hexPointCubeThirdPoint = -hexPoint.R - hexPoint.Q;
            //function cube_distance(a, b):
            foreach (var storedHex in HexStorage)
            {
                HexPoint point = storedHex.Key;
                var hexThirdPoint = -point.R - point.Q;
                if ((Math.Abs(point.R - hexPoint.R) + Math.Abs(point.Q - hexPoint.Q) +
                     Math.Abs(hexPointCubeThirdPoint - hexThirdPoint))/2 <= radius)
                {
                    inRadius.Add(storedHex.Key);
                }
            }
            return inRadius;
        }

        public void HighlightHexes(List<HexPoint> hexPoints, Boolean hightlight)
        {
            foreach(var hexPoint in hexPoints)
            {
                HexStorage[hexPoint].Highlighted = hightlight;
            }
        }
        public void UnHighlightAll()
        {
            foreach(var hex in HexStorage)
            {
                hex.Value.Highlighted = false;
            }
        }
        public Dictionary<HexPoint, Hex> GetNeighbors(HexPoint hexPoint)
        {
            var neighboors = new Dictionary<HexPoint, Hex>();
            var possibleNeighbors = new List<Vector2>()
            {
                new Vector2(+1, 0), new Vector2(+1, -1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(-1, +1), new Vector2(0, +1)
            };
            foreach (var v in possibleNeighbors)
            {
                var possibleNeighbor = new HexPoint(hexPoint.R + (int)v.X, hexPoint.Q + (int)v.Y);
                var inGrid = HexStorage.FirstOrDefault(h => h.Key.R == possibleNeighbor.R && h.Key.Q == possibleNeighbor.Q);
                //null check right?
                if (inGrid.Key != null)
                    neighboors[inGrid.Key] = inGrid.Value;
            }
            return neighboors;

        }
        //https://www.redblobgames.com/grids/hexagons/#line-drawing
       
        public static List<HexPoint> LineBetweenTwoPoints(HexPoint p1, HexPoint p2)
        {
            //totally0
            int distance = HexDistance(p1, p2);
            var results = new List<HexPoint>();
            //var p1z = -p1.Q - p1.R;
            //var p2z = -p2.Q - p2.R;

            for (var i = 0; i <= distance; i++)
            {
                var x = p1.Q + (p2.Q - p1.Q) * (1.0f/distance * i);
                var y = p1.R + (p2.R - p1.R) * (1.0f / distance * i);
                //var z = Math.Round(p1z + (p2z + p1z) * (1.0f / distance * i));
                var rounded = RoundCubeCordinates(x, y);
                results.Add(new HexPoint((int)rounded.Y, (int)rounded.X));
                ;
            }
            return results;
        }


        public static Vector2 RoundCubeCordinates(float x, float y)
        {
            //Q or R??
            var z = - y - x;
            var rx = (float)Math.Round(x);
            var ry = (float)Math.Round(y);
            var rz = (float)Math.Round(z);

            var xDif = Math.Abs(rx - x);
            var yDif = Math.Abs(ry - y);
            var zDif = Math.Abs(rz - z);

            if (xDif > yDif && xDif > zDif)
                rx = -ry - rz;
            else if (yDif > zDif)
                ry = -rx - rz;
            return new Vector2(rx, ry);
        }
        /*
         * 
         * function cube_round(cube):
    var rx = round(cube.x)
    var ry = round(cube.y)
    var rz = round(cube.z)

    var x_diff = abs(rx - cube.x)
    var y_diff = abs(ry - cube.y)
    var z_diff = abs(rz - cube.z)

    if x_diff > y_diff and x_diff > z_diff:
        rx = -ry-rz
    else if y_diff > z_diff:
        ry = -rx-rz
    else:
        rz = -rx-ry

    return Cube(rx, ry, rz)

            function lerp(a, b, t): # for floats
    return a + (b - a) * t

function cube_lerp(a, b, t): # for hexes
    return Cube(lerp(a.x, b.x, t), 
                lerp(a.y, b.y, t),
                lerp(a.z, b.z, t))

function cube_linedraw(a, b):
    var N = cube_distance(a, b)
    var results = []
    for each 0 ≤ i ≤ N:
        results.append(cube_round(cube_lerp(a, b, 1.0/N * i)))
    return results
         */
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
        //doesnt really need the stuff in the abstract
        public void Draw(FloatPoint center)
        {
            throw new NotImplementedException();
        }

        public void Draw(FloatPoint center, Vector2 size)
        {
            throw new NotImplementedException();
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