using System;
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

        public override bool Equals(object obj)
        {
            var otherPoint = obj as HexPoint;
            if (otherPoint == null)
                return false;

            return (otherPoint.Q == Q && otherPoint.R == R);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash*16777619 ^ R.GetHashCode();
                hash = hash * 16777619 ^ Q.GetHashCode();
                return hash;
            }
        }

    }
}
