﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Geometry
{
    public class HexPoint
    {
        public int R { get; set; }
        public int Q { get; set; }
        
        public HexPoint(int r, int q)
        {
            R = r;
            Q = q;
        }

        //public bool Equals(HexPoint otherPoint)
        //{
        //    if (otherPoint.Equals(null))
        //        return false;
        //    if(otherPoint.Q == Q && otherPoint.R == R)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
