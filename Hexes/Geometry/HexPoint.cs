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

        /// <summary>
        /// Return format cords "R-Q"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return R + ":" + Q;
        }


        /// <summary>
        /// Converts string to hexpoint
        /// </summary>
        /// <param name="hexString">"R-Q" format</param>
        /// <returns></returns>
        public static HexPoint StringToHexPoint(string hexString)
        {
            var cords = hexString.Split(':');
            if (cords.Length != 2)
            {
                throw new Exception("Wrong string format");
            }
            return new HexPoint(Int32.Parse(cords[0]), Int32.Parse(cords[1]));
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
