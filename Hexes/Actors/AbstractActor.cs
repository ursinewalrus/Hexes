using Hexes.Geometry;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Actors
{
    public abstract class AbstractActor : IMovable, IActor
    {
        public HexPoint Location;
        public Boolean Controllable;
        public int MoveDistance;
        public Texture2D Texture;

        public abstract void LoadAttributes(Dictionary<string, Dictionary<string, string>> attributes);
        public virtual List<HexPoint> AllInMoveRange(HexPoint location)
        {
            // have grid calculate distance
            return new List<HexPoint>();
        }

        public virtual HexPoint MoveTo(HexPoint moveFrom, HexPoint moveTo)
        {
            return new HexPoint(0,0);
        }

        public virtual Boolean CanMoveTo(HexPoint moveFrom, HexPoint moveTo)
        {
            return false;
        }

        public virtual List<HexPoint> CanSee(HexPoint location)
        {
            return new List<HexPoint>();
        }
    }
}
