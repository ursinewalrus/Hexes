using Hexes.Geometry;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Control;
using Microsoft.Xna.Framework;
using Hexes.UI;

namespace Hexes
{
    public interface IDrawable
    {
        void Draw();

        void Draw(FloatPoint center);

        void Draw(FloatPoint center, Vector2 size);
    }

    //hold texture?
    public abstract class Drawable : IDrawable
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static SpriteBatch Sb { get; set; }
        public static SpriteFont Font { get; set; }
        public static Camera Camera { get; set; }

        public virtual void Draw()
        {
            //
        }
        public virtual void Draw(FloatPoint center)
        {
            //
        }
        public virtual void Draw(FloatPoint center, Vector2 size)
        {
            //
        }

    }

    public abstract class UIDrawable : Drawable
    {
        public Vector2 StartV { get; set; }
        public Vector2 Size;
        public abstract void OnClick();
        


    }
}
