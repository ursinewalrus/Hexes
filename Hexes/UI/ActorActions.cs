using Hexes.Actors;
using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.UI
{
    public class ActorActions : UIDrawable, IDrawable, IUIActions
    {
        public static string ElementName = "ActorActions";
        public Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }


        public ActorActions(BasicActor actor, Vector2 size)
        {
            Texture = actor.Texture;
            //maaaybe pass as param
            StartV = new Vector2(5, 25);
            Size = size;
            Actor = actor;
        }
        public void Draw()
        {
            //outline draw, txt draw for actions which should be sprite, and draw over maybe, Z index? doable?
            //Sb.Draw(Texture,
            //     Vector2.Transform(StartV, Matrix.Invert(Camera.Transform)),
            //    Color.Black
            //    );
            //Sb.Draw(Texture,
            //       destinationRectangle: new Rectangle(5,25,30,50),
            //       color: Color.White
            //   );
            var v2 = Vector2.Transform(new Vector2(30f, 40f), Matrix.Invert(Camera.Transform));
            Actor.Draw(new FloatPoint(v2.X, v2.Y), Size);
            Sb.DrawString(Font, "MOVE", Vector2.Transform(StartV, Matrix.Invert(Camera.Transform)), Color.Black);

            // Vector2.Transform(new Vector2(5,5), Matrix.Invert(Camera.Transform))
        }

      

        public override void OnClick()
        {
            ;
            //
        }



        public  void Draw(FloatPoint location)
        {
            //
        }

        public void Draw(FloatPoint center, Vector2 size)
        {
            //
        }
    }
}
