using Hexes.Geometry;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

        //iffy
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

    //Give this a defined draw method :TODO
    public abstract class UIDrawable : Drawable
    {
        public Vector2 StartV { get; set; }
        public Vector2 Size;
        public abstract void OnClick();
        public HexGrid.HexGrid HexGrid { get; set; }
        public string ElementName { get; protected set; }

        protected UIDrawable(HexPoint hexPoint, HexGrid.HexGrid hexGrid)
        {
            
        }

        protected UIDrawable()
        {
            
        }
        public void Draw(Texture2D texture)
        {
            var v2 = Vector2.Transform(StartV, Matrix.Invert(Camera.Transform));
            Sb.Draw(
                texture,
                    destinationRectangle: new Rectangle((int)v2.X, (int)v2.Y, (int)Size.X, (int)Size.Y),
                    sourceRectangle: new Rectangle(0, 0, 100, 100),
                    color: Color.White
                // origin: new Vector2(Size.X / 2, Size.Y / 2)
                );
        }
    }
}
