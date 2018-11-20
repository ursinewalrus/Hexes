using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public class Hex
    {
        private int R;
        private int Q;
        private int S;
        public List<Point> HexCorners;
        public static int Size = 20;
        private int BaseXOffSet = 100;
        private int BaseYOffSet = 100;


        public static int gameWidth { get; set; }

        public static readonly double[] Corners = {
                3.0 / 2.0, 0.0, //f0
                Math.Sqrt(3.0) / 2.0, //f1
                Math.Sqrt(3.0), //f2
                2.0 / 3.0, //f3
                0.0, //b0
                -1.0 / 3.0, //b1
                Math.Sqrt(3.0) / 3.0, //b2
                0.0 //b3
            };

        public Hex(int r, int q)
        {
            R = r;
            Q = q;
            S = -Q - R;
            if (R + Q + S != 0)
            {
                throw new ArgumentException(String.Format("Invalid Hex Cordinates {0} {1} {2}", Q, R, S), "original");
            }
            GetCorners();
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

        //https://www.redblobgames.com/grids/hexagons/#hex-to-pixel
        //https://www.redblobgames.com/grids/hexagons/#coordinates
        //odd-q
        public void GetCorners()
        {
            var corners = new List<Point>();

            Point center = HexToPixel();
            //var t = new Texture2D(Sb.GraphicsDevice, 10, 10);

            //Sb.Begin();
            //Sb.Draw(t, new Vector2(center.X, center.Y), Color.Black);
            //Sb.End();

            //https://www.redblobgames.com/grids/hexagons/implementation.html#hex-geometry
            //2.3Drawing a hex
            for (var i = 0; i < 6; i++)
            {
                Point offset = HexCornerOffset(i);
                ;
                corners.Add(
                        new Point(
                            offset.X + center.X + BaseXOffSet, //+ (Q * Size), //center x
                            offset.Y + center.Y + BaseYOffSet //+ ((R * -Size) + (Q % 2 == 0? R : -R))//center y
                        )
                    );
            }
            HexCorners = corners;
            //int xCor = multiplier * (3 / 2 * Q);
            //int YCor = (int)(multiplier * (Math.Sqrt(3) / 2 * Q + (Math.Sqrt(3) * R)));
        }

        public Point HexCornerOffset(int corner)
        {
            double angle = 2.0 * Math.PI * (0.0 + corner) / 6;
            return new Point((int)(Size * Math.Cos(angle)),(int) (Size * Math.Sin(angle)));
        }

        public Point HexToPixel()
        {
            int x = (int)((Corners[0] + Q) + (Corners[1] * R)) * Size;
            int y = (int)((Corners[2] + Q) + (Corners[3] * R)) * Size;
            return new Point(x, y);

        }

        public void DrawEdges()
        {
            var lastPoint = HexCorners[0];
            for (var i=1; i<6; i++)
            {
                new Line(lastPoint.X, lastPoint.Y, HexCorners[i].X, HexCorners[i].Y, 2, Color.Black);
                lastPoint = HexCorners[i];
            }
            var line = new Line(HexCorners[0].X, HexCorners[0].Y, HexCorners[5].X, HexCorners[5].Y, 2, Color.Black);

        }
        //public List<Hex> GetNeighbors()
        //{

        //}
    }
}
