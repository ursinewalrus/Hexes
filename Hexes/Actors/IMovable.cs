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
        List<HexPoint> AllInMoveRange(HexPoint location);

        Boolean CanMoveTo(HexPoint moveFrom, HexPoint moveTo);

        List<HexPoint> CanSee(HexPoint location);

        HexPoint MoveTo(HexPoint moveFrom, HexPoint moveTo);

    }
}
