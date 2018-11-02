using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public class Hex
    {
        public int R;
        public int Q;
        public int S;

        public Hex(int r, int q)
        {
            R = r;
            Q = q;
            S = -Q - R;
            if (R + Q + S != 0)
            {
                throw new ArgumentException(String.Format("Invalid Hex Cordinates {0} {1} {2}", Q, R, S), "original");
            }
        }

        public bool IsSameHex(Hex hex)
        {
            return hex.Q == Q && hex.R == R && hex.S == S;
        }

        public Hex AddHex(Hex hex)
        {
            return new Hex(Q + hex.Q, R + hex.R);
        }

        public Hex SubtractHex(Hex hex)
        {
            return new Hex(Q - hex.Q, R + hex.R);
        }

        public int HexDistance(Hex hex)
        {
            Hex subtractedHex = SubtractHex(hex);
            return (Math.Abs(hex.Q) + Math.Abs(hex.R) + Math.Abs(hex.S)) / 2;
        }

        //public List<Hex> GetNeighbors()
        //{

        //}
    }
}
