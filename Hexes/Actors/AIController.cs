using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hexes.Geometry;
using Microsoft.Xna.Framework;

namespace Hexes.Actors
{
    public class AIController
    {
        private readonly HexGrid.HexGrid HexGrid;
        private readonly Random Chaos = new Random();
        //ms
        public int MoveDelay = 500;
        public AIController(HexGrid.HexGrid hexgrid)
        {
            HexGrid = hexgrid;
        }


        public void Wander(BasicActor actor)
        {
            var canMoveTo = actor.MoveableInMoveRange(HexGrid);
            var chooseMoveTo = Chaos.Next(canMoveTo.Count());
            var newLoc = canMoveTo[chooseMoveTo];
            var moveDiff = new HexPoint(newLoc.R - actor.Location.R, newLoc.Q - actor.Location.Q);
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
            //comments for "base" case
            //maybe look at ratios for more fine grained turning -> other things should determine rot later though
            //dont be lazy just draw it out :TODO
            if (dir.Q > 0 && dir.R < 0)//dir.Q == 1 && dir.R == -1
            {
                return 0;
            }
            else if (dir.Q > 0 && dir.R == 0)//dir.Q == 1 && dir.R == 0
            {
                return 1;
            }
            else if (dir.Q == 0 && dir.R > 0)//dir.Q == 0 && dir.R == 1
            {
                return 2;
            }
            else if (dir.Q < 0 && dir.R > 0)//dir.Q == -1 && dir.R == 1
            {
                return 3;
            }
            else if (dir.Q < 0 && dir.R == 0)//dir.Q == -1 && dir.R == 0
            {
                return 4;
            }
            else if(dir.Q == 0 && dir.R < 0)//if (dir.Q == 0 && dir.R == -1)
            {
                return 5;
            }
            else
            {
                if(dir.Q > 0 && dir.R > 0)
                {
                    if (dir.Q > dir.R)
                        return 1;
                    return 2;
                }
                if (dir.Q < 0 && dir.R < 0)
                {
                    if (dir.R > dir.Q)
                        return 4;
                    return 5;
                }
            }
            //should never happpen
            return new Random().Next(6);

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
