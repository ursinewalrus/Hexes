using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Geometry;
using Microsoft.Xna.Framework;

namespace Hexes.Actors
{
    public class AIController
    {
        private readonly HexGrid.HexGrid HexGrid;
        private readonly Random Chaos = new Random();
        public AIController(HexGrid.HexGrid hexgrid)
        {
            HexGrid = hexgrid;
        }


        public void Wander(BasicActor actor)
        {
            var canMoveTo = actor.MoveableInMoveRange(HexGrid);
            var chooseMoveTo = Chaos.Next(canMoveTo.Count());
            var newLoc = canMoveTo[chooseMoveTo];
            var moveDiff = new HexPoint(actor.Location.R - newLoc.R, actor.Location.Q - newLoc.Q);
            var rot = RotateToDirectionMoved(moveDiff);
            actor.Rotate(rot);
            actor.MoveTo(newLoc, HexGrid);
        }

        public HexPoint MoveToNearestSeenFoe(BasicActor actor)
        {
            var canSee = actor.CanSee(HexGrid);
            return new HexPoint(0,0);   
        }
        public int RotateToDirectionMoved(HexPoint dir)
        {
            if (dir.Q == 1 && dir.R == -1)
            {
                return 0;
            }
            else if (dir.Q == 1 && dir.R == 0)
            {
                return 1;
            }
            else if (dir.Q == 0 && dir.R == 1)
            {
                return 2;
            }
            else if (dir.Q == -1 && dir.R == 1)
            {
                return 3;
            }
            else if (dir.Q == -1 && dir.R == 0)
            {
                return 4;
            }
            else //if (dir.Q == 0 && dir.R == -1)
            {
                return 5;
            }

        }
    }
    //eg
    public enum AIActionTypes
    {
        Wander,
        Rotate,
        MoveToNearestEnemy
    }
}
