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
        List<HexPoint> AllInMoveRange(HexGrid.HexGrid hexGrid);

        Boolean CanMoveTo(HexPoint moveTo, HexGrid.HexGrid hexGrid);

        List<HexPoint> CanSee(HexPoint startLocLeft, HexPoint startLocRight, HexGrid.HexGrid hexGrid);

        void MoveTo(HexPoint moveTo, HexGrid.HexGrid hexGrid);

    }
}
