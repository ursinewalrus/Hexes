using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.UI;
using Hexes.Utilities;

namespace Hexes.Actors
{
    public class BasicActor : Drawable, IMovable, IActor
    {
        #region properites and setup
        public string Name;
        public HexPoint Location { get; set; }
        public Boolean Controllable;
        public int MoveDistance;
        public int SightRange;
        public Texture2D Texture;
        public int HP { get; set; }
        public string ModuleName;
        public static float SizeX;
        public static float SizeY;
        public int Rotation;
        public List<HexPoint> HexesCanSee = new List<HexPoint>();

        public static Dictionary<int, List<Vector2>> FoVArms = new Dictionary<int, List<Vector2>>()
        {
            //rotation, <left arm, right arm>
            {0, new List<Vector2>(){new Vector2(-1,0),new Vector2(0,1)}},
            {1, new List<Vector2>(){new Vector2(-1,1),new Vector2(1,0)}}, //rotation 1
            {2, new List<Vector2>(){new Vector2(0,1),new Vector2(1,-1)}}, // 3 o clock
            {3, new List<Vector2>(){new Vector2(1,0),new Vector2(0,-1)}},
            {4, new List<Vector2>(){new Vector2(1,-1),new Vector2(-1,0)}},
            {5, new List<Vector2>(){new Vector2(0,-1),new Vector2(-1,1)}}
        };

        public BasicActor(HexPoint location, string name, Dictionary<string, string> actorData, int rotation, bool PC, string moduleName)
        {
            Name = name;
            Location = location;
            Controllable = PC;
            Rotation = rotation;
            ModuleName = moduleName;
            AsignActorData(actorData);
        }
        public void AsignActorData(Dictionary<string, string> actorData)
        {
            MoveDistance = Convert.ToInt32(actorData["moveDistance"]);
            HP = Convert.ToInt32(actorData["defaultHP"]);
            string assetPath = @"Modules\" + ModuleName + @"\" + actorData["texture"];
            FileStream fs = new FileStream(assetPath, FileMode.Open);
            Texture = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Dispose();
            SizeX = Convert.ToInt32(actorData["bottomrightX"]);
            SizeY = Convert.ToInt32(actorData["bottomrightY"]);
            SightRange = 8;
        }
        #endregion

        #region move related
        public List<HexPoint> AllInMoveRange(HexGrid.HexGrid hexGrid)
        {
            //https://www.redblobgames.com/grids/hexagons/#range
            //var possibleMoves = new List<HexPoint>();
            var visited = new List<HexPoint>();
            var toVisit = new List<HexPoint>[MoveDistance + 1];
            for (var i=0 ; i<toVisit.Count();i++)
            {
                toVisit[i] = new List<HexPoint>();
            }
            toVisit[0].Add(Location);

            for (var i = 0; i < MoveDistance; i++)
            {
                foreach (var h in toVisit[i])
                {
                    var neighbors = hexGrid.GetNeighbors(h);
                    foreach (var n in neighbors)
                    {
                        if (!n.Value.BlocksMovment && !visited.Contains(n.Key) && !hexGrid.ActorStorage.Any(a => a.Location.Equals(n.Key)))
                        {
                            visited.Add(n.Key);
                            toVisit[i + 1].Add(n.Key);
                        }
                    }
                }
            }
            return visited;
        }

        public void MoveTo(object sender, ActorMoveActionEvent eventArgs)
        {
            MoveTo(eventArgs.Location, eventArgs.HexGrid);
        }
        public void MoveTo(HexPoint moveTo, HexGrid.HexGrid hexGrid)
        {
            if (CanMoveTo(moveTo, hexGrid))
            {
                Location = moveTo;
                hexGrid.UnHighlightAll();
                hexGrid.DebugLines = new List<DebugLine>();
            }
        }

        public Boolean CanMoveTo(HexPoint moveTo, HexGrid.HexGrid hexGrid)
        {
            //split for maybe sep messages
            //also cant be straight line, need to pathfind to it
            if (AllInMoveRange(hexGrid).Contains(moveTo))
            {
                return true;
            }
            return false;
        }

        public void Rotate(object sender, ActorRotateActionEvent eventArgs)
        {
            var clockwise = eventArgs.ClockWise;
            var dir = clockwise ? 1 : -1;
            Rotation = (Rotation + dir + 6) % 6;
        }
        #endregion
        //https://www.redblobgames.com/grids/hexagons/#field-of-view
        //this could probably be redone once it starts working
        public List<HexPoint> CanSee(HexPoint startLoc, HexGrid.HexGrid hexGrid)
        {
            HexesCanSee = new List<HexPoint>();
            //invent hexes for ones that dont exist for the purpose of our calculations
            var lookDirArms = FoVArms[Rotation];

            var lArmR = startLoc.R + (int)lookDirArms[0].X * SightRange;
            var lArmQ = startLoc.Q + (int)lookDirArms[0].Y * SightRange;

            var rArmR = startLoc.R + (int)lookDirArms[1].X * SightRange;
            var rArmQ = startLoc.Q + (int)lookDirArms[1].Y * SightRange;

            var leftArmCord = new HexPoint(lArmR, lArmQ);
            var rightArmCord = new HexPoint(rArmR, rArmQ);

            var shiftRight = false;
            if (Rotation > 0 && Rotation < 4)
            {
                shiftRight = true;
            }
            var furthestEdge = HexGrid.HexGrid.LineBetweenTwoPoints(leftArmCord, rightArmCord, shiftRight);
            foreach (var hexPoint in furthestEdge)
            {
                var ray = HexGrid.HexGrid.LineBetweenTwoPoints(startLoc, hexPoint);
                //:TODO REMOVE after testing
                var startLinePointHex = hexGrid.HexStorage.First(h => h.Key.Equals(ray[0])).Value;
                var endLinePoint = hexGrid.HexStorage.First(h => h.Key.Equals(ray[0])).Value;
                //
                var debugLine = new DebugLine();

                var blocked = false;

                //should be drawing away from the start point
                foreach (var rayPoint in ray)
                {
                    if (!blocked)
                    {
                        if (hexGrid.HexStorage.ContainsKey((rayPoint)))
                        {
                            var hex = hexGrid.HexStorage.First(h => h.Key.Equals(rayPoint));
                            if (!hex.Value.BlocksVision)
                            {
                                debugLine.DebugStrings[hex.Value.Center] = hex.Key.R + " " + hex.Key.Q;
                                HexesCanSee.Add(hex.Key);
                                endLinePoint = hex.Value;
                            }
                            else
                            {
                                blocked = true;
                            }
                        }
                        else
                        {
                            blocked = true;
                        }
                    }
                }
                var debugLineLine = new Line(startLinePointHex.Center.X, startLinePointHex.Center.Y,
                    endLinePoint.Center.X, endLinePoint.Center.Y, 3f, Color.Black);
                debugLine.Line = debugLineLine;
                hexGrid.DebugLines.Add(debugLine);
            }
            return HexesCanSee;
        }


        #region drawstuff
        //maybe also a draw that just takes the R/Q cords?
        public void Draw(Vector2 center)
        {
            Sb.Draw(Texture,
                    destinationRectangle: new Rectangle((int)center.X, (int)center.Y, (int)SizeX, (int)SizeY),
                    sourceRectangle: new Rectangle(0, 0, (int)SizeX, (int)SizeY),
                    color: Color.White,
                    rotation: (MathHelper.PiOver2 / 3.0f) + (MathHelper.PiOver2 / 1.5f ) * Rotation,
                    origin: new Vector2(SizeX/2,SizeY/2)
                );
        }
        public void Draw(FloatPoint center, Vector2 size)
        {
            Sb.Draw(Texture,
                    destinationRectangle: new Rectangle((int)center.X, (int)center.Y, (int)size.X, (int)size.Y),
                    sourceRectangle: new Rectangle(0, 0, (int)SizeX, (int)SizeY),
                    color: Color.White,
                    rotation: (MathHelper.PiOver2 / 3.0f) + (MathHelper.PiOver2 / 1.5f) * Rotation,
                    origin: new Vector2(size.X / 2, size.Y / 2)
                );
        }
        #endregion
    }
}
