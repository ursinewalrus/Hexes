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

namespace Hexes.Actors
{
    public class BasicActor : Drawable, IMovable, IActor
    {
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
            SightRange = 10;
        }
        public List<HexPoint> AllInMoveRange(HexPoint moveFrom, HexGrid.HexGrid hexGrid)
        {
            //https://www.redblobgames.com/grids/hexagons/#range
            //var possibleMoves = new List<HexPoint>();
            var visited = new List<HexPoint>();
            var toVisit = new List<HexPoint>[MoveDistance + 1];
            for (var i=0 ; i<toVisit.Count();i++)
            {
                toVisit[i] = new List<HexPoint>();
            }
            toVisit[0].Add(moveFrom);

            for (var i = 0; i < MoveDistance; i++)
            {
                foreach (var h in toVisit[i])
                {
                    var neighbors = hexGrid.GetNeighbors(h);
                    foreach (var n in neighbors)
                    {
                        if (!n.Value.BlocksMovment && !visited.Any(v => v.Equals(n.Key)))
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
            MoveTo(eventArgs.Actor.Location, eventArgs.Location, eventArgs.HexGrid);
        }
        public void MoveTo(HexPoint moveFrom, HexPoint moveTo, HexGrid.HexGrid hexGrid)
        {
            if (CanMoveTo(moveFrom, moveTo, hexGrid))
            {
                Location = moveTo;
            }
        }

        public Boolean CanMoveTo(HexPoint moveFrom, HexPoint moveTo, HexGrid.HexGrid hexGrid)
        {
            //split for maybe sep messages
            //also cant be straight line, need to pathfind to it
            if (AllInMoveRange(moveFrom, hexGrid).Any(h => h.Equals(moveTo)))
            {
                return true;
            }
            return false;
        }

        //https://www.redblobgames.com/grids/hexagons/#field-of-view
        public List<HexPoint> CanSee(HexPoint location, HexGrid.HexGrid hexGrid)
        {
            return new List<HexPoint>();
        }

        //maybe also a draw that just takes the R/Q cords?
        public void Draw(FloatPoint center)
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
    }
}
