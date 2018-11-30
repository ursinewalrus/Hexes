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
    }

    public abstract class Drawable 
    {
        public static GraphicsDevice GraphicsDevice { get; set; }

    }
}
