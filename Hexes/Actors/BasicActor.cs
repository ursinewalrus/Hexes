using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Actors
{
    public class BasicActor : Drawable, IMovable, IActor, IDrawable
    {
        public string Name;
        public HexPoint Location { get; set; }
        public Boolean Controllable;
        public int MoveDistance;
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
        }
        public List<HexPoint> AllInMoveRange(HexPoint location)
        {
            // have grid calculate distance
            return new List<HexPoint>();
        }

        public HexPoint MoveTo(HexPoint moveFrom, HexPoint moveTo)
        {
            return new HexPoint(0,0);
        }

        public Boolean CanMoveTo(HexPoint moveFrom, HexPoint moveTo)
        {
            return false;
        }

        public List<HexPoint> CanSee(HexPoint location)
        {
            return new List<HexPoint>();
        }

        public void Draw(FloatPoint center)
        {
            Sb.Draw(texture: Texture,
                    destinationRectangle: new Rectangle((int)center.X, (int)center.Y, (int)SizeX, (int)SizeY),
                    sourceRectangle: new Rectangle(0, 0, (int)SizeX, (int)SizeY),
                    color: Color.White,
                    rotation: (MathHelper.PiOver2 / 3.0f) + (MathHelper.PiOver2 / 1.5f ) * Rotation,
                    origin: new Vector2(SizeX/2,SizeY/2)
                );
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }
    }
}
