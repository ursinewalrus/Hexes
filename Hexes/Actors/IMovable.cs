using Hexes.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Actors
{
    interface IMovable
    {
        List<HexPoint> MoveableInMoveRange();

        Boolean CanMoveTo(HexPoint moveTo);

        List<HexPoint> CanSee(int coneArmLength);

        void MoveTo(HexPoint moveTo);

    }
}
