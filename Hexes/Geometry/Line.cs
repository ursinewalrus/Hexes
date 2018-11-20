﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Geometry
{
    public class Line
    {
        public static SpriteBatch Sb { get; set; }
        private Color LineColor; 
        private int WidthMultiplier;

        private Vector2 StartV;
        private Vector2 EndV;

        Texture2D texture;

        public Line(int startX, int startY, int endX, int endY, int wM, Color color)
        {
            WidthMultiplier = wM;

            StartV = new Vector2(startX, startY);
            EndV = new Vector2(endX, endY);

            texture = new Texture2D(Sb.GraphicsDevice, 1, 1);
            LineColor = color;
            texture.SetData<Color>(new Color[] { LineColor });

            Sb.Begin();
            DrawLine();
            Sb.End();
        }

        //https://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders
        //https://gamedev.stackexchange.com/questions/26013/drawing-a-texture-line-between-two-vectors-in-xna-wp7
        private void DrawLine()
        {
            Vector2 edge = EndV - StartV;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            Sb.Draw(texture,
                StartV,
                null,
                LineColor,
                (float)Math.Atan2(EndV.Y - StartV.Y, EndV.X - StartV.X),
                new Vector2(0f, (float)texture.Height / 2),
                new Vector2(Vector2.Distance(StartV, EndV), WidthMultiplier), //float for width
                SpriteEffects.None, 
                0f
            );
        }
    }
}
