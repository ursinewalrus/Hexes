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
    //TODO : Make their draw methods come from the same place
    public class ActorRotateClockWise : UIDrawable, IUIActions
    {
        public static Texture2D Texture { get; set; }
        public BasicActor Actor { get; set; }
        public HexPoint HexPoint { get; set; }

        public ActorRotateClockWise(BasicActor actor)
        {
            ElementName = "ActorRotateClockWiseActions";
            //maaaybe pass as param
            StartV = new Vector2(5, 70);
            Size = new Vector2(100, 100);
            Actor = actor;
        }

        public override void Draw()
        {
            base.Draw(Texture);
        }

        public override void OnClick()
        {
            var eventSend = new ActorRotateActionEvent(true);
            eventSend.RotateAction += Actor.Rotate;
            eventSend.OnRotateAction();
        }
    }

    public class ActorRotateCounterClockWise : UIDrawable, IUIActions
    {
        public static Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }
        public HexPoint HexPoint { get; set; }

        public ActorRotateCounterClockWise(BasicActor actor)
        {

            ElementName = "ActorRotateCounterClockWiseActions";
            //maaaybe pass as param
            StartV = new Vector2(5, 120);
            Size = new Vector2(100, 100);
            Actor = actor;
        }

        //i dont want to write this every time, find a way to not do this :TODO
        public override void Draw()
        {
            base.Draw(Texture);
        }

        public override void OnClick()
        {
            var eventSend = new ActorRotateActionEvent(false);
            eventSend.RotateAction += Actor.Rotate;
            eventSend.OnRotateAction();
        }
    }


    public class ActorRotateActionEvent : EventArgs
    {
        public event EventHandler<ActorRotateActionEvent> RotateAction;

        public bool ClockWise { get; set; }

        public ActorRotateActionEvent(bool clockWise)
        {
            ClockWise = clockWise;
            //:TODO re do actors can see
        }
        // :TODO probaly abstractable
        public void OnRotateAction()
        {
            var handler = RotateAction;
            if (handler != null)
            {
                handler(this, new ActorRotateActionEvent(ClockWise));
            }
        }
    }
}
