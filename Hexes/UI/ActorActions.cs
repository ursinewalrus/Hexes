using EmptyKeys.UserInterface.Mvvm;
using Hexes.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.UI
{
    public class ActorActions : Drawable
    {
        public Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public Vector2 StartV { get; set; }
        public BasicActor Actor { get; set; }

        public float Size = 25;


        public class ActorACtionsUIViewModel : ViewModelBase
        {

        }


        //public ActorActions(BasicActor actor)
        //{
        //    Texture = new Texture2D(Sb.GraphicsDevice, 25, 25);
        //    StartV = new Vector2(5, 25);
        //    Actor = actor;
        //}
        //public void DrawActorActions()
        //{
        //    //outline draw, txt draw for actions which should be sprite, and draw over maybe, Z index? doable?
        //    Sb.Draw(Texture,
        //        StartV,
        //        LineColor
        //        );
        //}
    }
}
