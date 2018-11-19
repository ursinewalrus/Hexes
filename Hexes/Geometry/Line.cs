using Microsoft.Xna.Framework;
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
        private SpriteBatch Sb;
        private Color LineColor; 
        private int WidthMultiplier;

        private Vector2 StartV;
        private Vector2 EndV;

        private Game Game;

        Texture2D texture;

        public Line(SpriteBatch spriteBatch, int startX, int startY, int endX, int endY, int wM, Color color, Game game)
        {
            Sb = spriteBatch;
            WidthMultiplier = wM;

            StartV = new Vector2(startX, startY);
            EndV = new Vector2(endX, endY);

            Game = game;

            texture = new Texture2D(game.GraphicsDevice, 1, 1);
            LineColor = color;
            texture.SetData<Color>(new Color[] { LineColor });

            Sb.Begin();
            DrawLine();
            Sb.End();
        }

        //https://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders
        private void DrawLine()
        {
            Vector2 edge = EndV - StartV;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            Sb.Draw(texture,
                    new Rectangle(
                        (int)StartV.X,
                        (int)StartV.Y,
                        (int)edge.Length(),
                        WidthMultiplier
                    ),
                    null,
                    LineColor,
                    0,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    0
                );

        }
    }
}
