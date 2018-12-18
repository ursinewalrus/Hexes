using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Geometry
{
    public class Hex : Drawable
    {
        #region properties
        public string Name;

        HexPoint HexPoint;

        public bool BlocksMovment;
        public bool BlocksVision;
        public Color Color = Color.White;
        public bool Highlighted { get; set; }
        public static Texture2D SelectedTexture {get; set;}

        public List<FloatPoint> HexCorners;
        public static float SizeX = 100;
        public static float SizeY = 100;
        //y tho -> should not need these
        public int ResizeLeft;
        public int ResizeRight;
        public int ResizeTop;
        public int ResizeBottom;

        public FloatPoint Center;
        private List<Line> Edges = new List<Line>();
        private Texture2D Texture;
        public static Texture2D HighlightedTexture;
        public bool Hovered = false;


        public static double[] HexOrientation = 
            {
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
       
        //lazy, move
        string ModuleName;
        #endregion

        #region set up hex properties
        public Hex(HexPoint hexPoint, string name, Dictionary<string, string> hexData, string moduleName)
        {
            Name = name;
            Highlighted = false;
            ModuleName = moduleName;
            //make these use HexPoint for consistency
            HexPoint = hexPoint;
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
        #endregion 

        #region create hex geometry
        //https://www.redblobgames.com/grids/hexagons/#hex-to-pixel
        //https://www.redblobgames.com/grids/hexagons/#coordinates
        //odd-q
        public void GetCorners()
        {
            var corners = new List<FloatPoint>();

            Center = CenterHexToPixel();

            //https://www.redblobgames.com/grids/hexagons/implementation.html#hex-geometry
            for (var i = 0; i < 6; i++)
            {
                FloatPoint offset = HexCornerOffset(i);
                ;
                corners.Add(
                        new FloatPoint(
                            offset.X + Center.X, 
                            offset.Y + Center.Y 
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
        }

        public FloatPoint HexCornerOffset(int corner)
        {
            double angle = 2.0 * Math.PI * (HexOrientation[8]+ corner) / 6;
            return new FloatPoint((float)(SizeX * Math.Cos(angle)),(float)(SizeY * Math.Sin(angle)));
        }

        public FloatPoint CenterHexToPixel()
        {
            float x = (float)(HexOrientation[0] * HexPoint.Q + HexOrientation[1] * HexPoint.R) * SizeX;
            float y = (float)(HexOrientation[2] * HexPoint.Q + HexOrientation[3] * HexPoint.R) * SizeY;
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
        #endregion

        #region draw hex texture+geometry
        public void Draw()
        {
            //https://www.codeproject.com/Articles/1119973/Part-I-Creating-a-Digital-Hexagonal-Tile-Map
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

            Sb.Draw(texture: Texture,
                    destinationRectangle: new Rectangle((int)Center.X, (int)Center.Y, (int)SizeX * 2, (int)SizeY * 2),
                    //this is inconsistent -> maybe not anymore
                    sourceRectangle: new Rectangle(ResizeLeft, 0,ResizeRight, (int)SizeY),
                    color: Color,
                    origin: new Vector2(SizeX / 2, SizeY / 2)
                    //scale: new Vector2(9000,0.5f),
                    //effects: SpriteEffects.None,
                    //layerDepth: 0.0f
                    );
            if (Highlighted)
            {
                Sb.Draw(texture: HighlightedTexture,
                    destinationRectangle: new Rectangle((int)Center.X, (int)Center.Y, (int)SizeX * 2, (int)SizeY * 2),
                    //this is inconsistent -> maybe not anymore
                    sourceRectangle: new Rectangle(ResizeLeft, 0, ResizeRight, (int)SizeY),
                    color: Color,
                    origin: new Vector2(SizeX / 2, SizeY / 2)
                    //scale: new Vector2(9000,0.5f),
                    //effects: SpriteEffects.None,
                    //layerDepth: 0.0f
                    );
            }
            foreach (Line line in Edges)
            {
                //if (Hovered)
                //{
                //    line.Draw(Color.Yellow);
                //}
                //else
                //{
                //    line.Draw(Color.Black);
                //}
                line.Draw();
            }
        }

        #endregion

        public static FloatPoint CenterHexToPixel(HexPoint hex)
        {
            float x = (float)(HexOrientation[0] * hex.Q + HexOrientation[1] * hex.R) * SizeX;
            float y = (float)(HexOrientation[2] * hex.Q + HexOrientation[3] * hex.R) * SizeY;
            return new FloatPoint(x, y);
        }

        public static bool Equals(Hex a, Hex b)
        {
            if (a == null || b == null)
                return false;
            return a.HexPoint.R == b.HexPoint.R && a.HexPoint.Q == b.HexPoint.Q;
        }
    }
}
