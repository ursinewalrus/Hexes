using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public class Hex : Drawable
    {
        #region properties
        public string Name;

        private int R;
        private int Q;
        private int S;

        bool BlocksMovment;
        bool BlocksVision;

        public List<FloatPoint> HexCorners;
        public static float SizeX = 100;
        public static float SizeY = 100;
        //y tho -> should not need these
        public int ResizeLeft;
        public int ResizeRight;
        public int ResizeTop;
        public int ResizeBottom;

        //set from game1 perhaps
        private int BaseXOffSet;
        private int BaseYOffSet;

        public FloatPoint Center;
        private List<Line> Edges = new List<Line>();
        private Texture2D Texture;
        public readonly static SpriteBatch Sb = Game1.SpriteBatch;
        FloatPoint TopLeftOfSprite;

        private static float _sizeW;
        private static float SizeW
        {
            set
            {
                SizeW = (float)Math.Sqrt(3) * SizeX;
            }
            get
            {
                return _sizeW;
            }
        }

        private static float _sizeH;
        private static float SizeH
        {
            set
            {
                SizeH = 2.0f * SizeY;
            }
            get
            {
                return _sizeH;
            }
        }

        public static double[] HexOrientation { get; set; }


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
        //lazy, move
        string ModuleName;
        #endregion
        //public Hex(int r, int q, float sizeX = 0, float sizeY = 0, Texture2D texture = null, int baseXOffset = 0, int baseYOffset = 0)
        public Hex(int r, int q, string name, Dictionary<string, string> hexData, string moduleName)
        {
            HexOrientation = PointyCorners;
            Name = name;
            ModuleName = moduleName;
            R = r;
            Q = q;
            S = -Q - R;
            if (R + Q + S != 0)
            {
                throw new ArgumentException(String.Format("Invalid Hex Cordinates {0} {1} {2}", Q, R, S), "original");
            }
            AsignTileData(hexData);
            GetCorners();
            MakeEdges();
        }

        private void AsignTileData(Dictionary<string, string> hexData)
        {
            ResizeLeft = Convert.ToInt32(hexData["topLeftX"].Trim());
            ResizeRight = Convert.ToInt32(hexData["bottomrightX"].Trim());
            ResizeTop = Convert.ToInt32(hexData["topLeftY"].Trim());
            ResizeBottom = Convert.ToInt32(hexData["bottomrightY"].Trim());
            BlocksMovment = Convert.ToBoolean(hexData["blocksMovment"].Trim());
            BlocksVision = Convert.ToBoolean(hexData["blocksVision"].Trim());
            //bump this up maybe, so only need to load once for each indev sprite
            string assetPath = @"Modules\" + ModuleName + @"\" + hexData["texture"];
            FileStream fs = new FileStream(assetPath, FileMode.Open);
            Texture = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Dispose();
        }
        //https://www.redblobgames.com/grids/hexagons/#hex-to-pixel
        //https://www.redblobgames.com/grids/hexagons/#coordinates
        //odd-q
        public void GetCorners()
        {
            var corners = new List<FloatPoint>();

            Center = CenterHexToPixel();
            TopLeftOfSprite = TopLeftRectangleHexToPicture();

            //https://www.redblobgames.com/grids/hexagons/implementation.html#hex-geometry
            for (var i = 0; i < 6; i++)
            {
                FloatPoint offset = HexCornerOffset(i);
                ;
                corners.Add(
                        new FloatPoint(
                            offset.X + Center.X + BaseXOffSet, //+ (Q * Size), //center x
                            offset.Y + Center.Y + BaseYOffSet //+ ((R * -Size) + (Q % 2 == 0? R : -R))//center y
                        )
                    );
            }
            //corners.Add(
            //    new FloatPoint(
            //        BaseXOffSet + Center.X,
            //        BaseYOffSet + Center.Y
            //        )
            //    );
            HexCorners = corners;
            //int xCor = multiplier * (3 / 2 * Q);
            //int YCor = (int)(multiplier * (Math.Sqrt(3) / 2 * Q + (Math.Sqrt(3) * R)));
        }

        public FloatPoint HexCornerOffset(int corner)
        {
            double angle = 2.0 * Math.PI * (HexOrientation[8]+ corner) / 6;
            return new FloatPoint((float)(SizeX * Math.Cos(angle)),(float)(SizeY * Math.Sin(angle)));
        }

        //there is definitally some extra stuff around this
        public FloatPoint TopLeftRectangleHexToPicture()
        {
            FloatPoint topLeft = new FloatPoint(Center.X, Center.Y);
            topLeft.Y -= SizeY;
            topLeft.X -= (_sizeW / 2);
            return topLeft;
        }

        public FloatPoint CenterHexToPixel()
        {
            float x = (float)(HexOrientation[0] * Q + HexOrientation[1] * R) * SizeX;
            float y = (float)(HexOrientation[2] * Q + HexOrientation[3] * R) * SizeY;
            return new FloatPoint(x, y);

        }

        public void MakeEdges()
        {
            var lastPoint = HexCorners[0];
            for (var i = 1; i < 6; i++)
            {
                Edges.Add(new Line(lastPoint.X, lastPoint.Y, HexCorners[i].X, HexCorners[i].Y, 2, Color.LightGray));
                lastPoint = HexCorners[i];
            }
            var line = new Line(HexCorners[0].X, HexCorners[0].Y, HexCorners[5].X, HexCorners[5].Y, 2, Color.LightGray);
            Edges.Add(line);

        }

        public override void Draw()
        {
            //https://www.codeproject.com/Articles/1119973/Part-I-Creating-a-Digital-Hexagonal-Tile-Map
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

            Sb.Draw(texture: Texture,
                    destinationRectangle: new Rectangle((int)TopLeftOfSprite.X - (int)SizeX, (int)TopLeftOfSprite.Y, (int)SizeX * 2, (int)SizeY * 2),
                    sourceRectangle: new Rectangle(ResizeLeft, 0,ResizeRight, (int)SizeY),
                    color: Color.White
                    //scale: new Vector2(9000,0.5f),
                    //effects: SpriteEffects.None,
                    //layerDepth: 0.0f
                    );
            foreach (Line line in Edges)
            {
                line.Draw();
            }

        }
        //public List<Hex> GetNeighbors()
        //{

        //}
    }
}
