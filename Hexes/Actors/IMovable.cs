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
        List<HexPoint> AllInMoveRange(HexPoint moveFrom, HexGrid.HexGrid hexGrid);

        Boolean CanMoveTo(HexPoint moveFrom, HexPoint moveTo, HexGrid.HexGrid hexGrid);

        List<HexPoint> CanSee(HexPoint location, HexGrid.HexGrid hexGrid);

        void MoveTo(HexPoint moveFrom, HexPoint moveTo, HexGrid.HexGrid hexGrid);

    }
}
