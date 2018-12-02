using Hexes.Geometry;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    interface IDrawable
    {
        void Draw();

        void Draw(FloatPoint center);
    }

    public abstract class Drawable 
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public readonly static SpriteBatch Sb = Game1.SpriteBatch;

    }
}
