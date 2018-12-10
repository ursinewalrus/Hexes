using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Geometry
{
    public class Line: Drawable, IDrawable
    {
        private Color LineColor; 
        private float WidthMultiplier;

        private Vector2 StartV;
        private Vector2 EndV;
        private double Length;

        Texture2D Texture;

        public Line(float startX, float startY, float endX, float endY, float wM, Color color)
        {
            WidthMultiplier = wM;

            StartV = new Vector2(startX, startY);
            EndV = new Vector2(endX, endY);

            Texture = new Texture2D(Sb.GraphicsDevice, 1, 1);
            LineColor = color;
            Texture.SetData<Color>(new Color[] { LineColor });

            Length = Math.Sqrt(Math.Pow(EndV.X - StartV.X, 2) + Math.Pow(EndV.Y - StartV.Y, 2));
        }

        //https://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders
        //https://gamedev.stackexchange.com/questions/26013/drawing-a-texture-line-between-two-vectors-in-xna-wp7
        public void Draw()
        {
            Vector2 edge = EndV - StartV;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            Sb.Draw(Texture,
                StartV,
                null,
                LineColor,
                (float)Math.Atan2(EndV.Y - StartV.Y, EndV.X - StartV.X),
                new Vector2(0f, (float)Texture.Height / 2),
                new Vector2(Vector2.Distance(StartV, EndV), WidthMultiplier), //float for width
                SpriteEffects.None, 
                0f
            );
        }

        public void Draw(FloatPoint center)
        {
        }
    }
}
