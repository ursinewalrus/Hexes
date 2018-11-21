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
    public class Hex : IDrawable
    {
        private int R;
        private int Q;
        private int S;
        public List<FloatPoint> HexCorners;
        public static int Size = 30;
        private int BaseXOffSet = 100;
        private int BaseYOffSet = 100;
        FloatPoint Center;
        public static double[] HexOrientation { get; set; }

        public static int gameWidth { get; set; }

        public static readonly double[] PointyCorners = {
                Math.Sqrt(3.0), //f0
                Math.Sqrt(3.0) / 2.0, //f1
                0.0, //f2
                3.0 / 2.0, //f3
                Math.Sqrt(3.0) / 3.0, //b0
                -1.0 / 3.0, //b1
                0.0, //b2
                2.0 / 3.0, //b3
                0.5 //start angle
            };
        public static readonly double[] FlatCorners = {
                3.0 / 2.0, //f0
                0.0, //f1
                Math.Sqrt(3.0) / 2.0, //f2
                Math.Sqrt(3.0), //f3
                2.0 / 3.0, //b0
                0.0, //b1
                -1.0 / 3.0, //b2
                Math.Sqrt(3.0) / 3.0, //b3
                0.0 //start angle
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
            var corners = new List<FloatPoint>();

            FloatPoint center = HexToPixel();
            Center = center;
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
                        new FloatPoint(
                            offset.X + center.X + BaseXOffSet, //+ (Q * Size), //center x
                            offset.Y + center.Y + BaseYOffSet //+ ((R * -Size) + (Q % 2 == 0? R : -R))//center y
                        )
                    );
            }
            corners.Add(
                new FloatPoint(
                    BaseXOffSet + center.X,
                    BaseYOffSet + center.Y
                    )
                );
            HexCorners = corners;
            //int xCor = multiplier * (3 / 2 * Q);
            //int YCor = (int)(multiplier * (Math.Sqrt(3) / 2 * Q + (Math.Sqrt(3) * R)));
        }

        public Point HexCornerOffset(int corner)
        {
            double angle = 2.0 * Math.PI * (HexOrientation[8]+ corner) / 6;
            return new Point((int)(Size * Math.Cos(angle)),(int) (Size * Math.Sin(angle)));
        }

        public FloatPoint HexToPixel()
        {
            float x = (float)(HexOrientation[0] * Q + HexOrientation[1] * R) * Size;
            float y = (float)(HexOrientation[2] * Q + HexOrientation[3] * R) * Size;
            return new FloatPoint(x, y);

        }

        public void DrawEdges()
        {
            var lastPoint = HexCorners[0];
            for (var i = 1; i < 6; i++)
            {
                new Line(lastPoint.X, lastPoint.Y, HexCorners[i].X, HexCorners[i].Y, 1, Color.Black);
                lastPoint = HexCorners[i];
            }
            var line = new Line(HexCorners[0].X, HexCorners[0].Y, HexCorners[5].X, HexCorners[5].Y, 1, Color.Black);
            var dot = new Line(HexCorners[6].X, HexCorners[6].Y, HexCorners[6].X+1, HexCorners[6].Y+1, 1, Color.Black);

        }
        //public List<Hex> GetNeighbors()
        //{

        //}
    }
}
