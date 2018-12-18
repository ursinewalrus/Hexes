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
        public string ElementName = "ActorRotateClockWiseActions";
        public static Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }
        public HexPoint HexPoint { get; set; }
        public HexGrid.HexGrid HexGrid { get; set; }

        public ActorRotateClockWise(BasicActor actor, HexPoint hexPoint, HexGrid.HexGrid hexGrid)
        {
            //maaaybe pass as param
            StartV = new Vector2(5, 70);
            Size = new Vector2(100, 100);
            HexPoint = hexPoint;
            HexGrid = hexGrid;
            Actor = actor;
        }

        public override void Draw()
        {
            var v2 = Vector2.Transform(StartV, Matrix.Invert(Camera.Transform));
            Sb.Draw(
                Texture,
                    destinationRectangle: new Rectangle((int)v2.X, (int)v2.Y, (int)Size.X, (int)Size.Y),
                    sourceRectangle: new Rectangle(0, 0, 100, 100),
                    color: Color.White
                   // origin: new Vector2(Size.X / 2, Size.Y / 2)
                );

        }
        public override void OnClick()
        {
            var eventSend = new ActorRotateActionEvent(HexGrid, true);
            eventSend.RotateAction += Actor.Rotate;
            eventSend.OnRotateAction();
        }
    }

    public class ActorRotateCounterClockWise : UIDrawable, IUIActions
    {
        public string ElementName = "ActorRotateCounterClockWiseActions";
        public static Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }
        public HexPoint HexPoint { get; set; }
        public HexGrid.HexGrid HexGrid { get; set; }

        public ActorRotateCounterClockWise(BasicActor actor, HexPoint hexPoint, HexGrid.HexGrid hexGrid)
        {
            //maaaybe pass as param
            StartV = new Vector2(5, 120);
            Size = new Vector2(100, 100);
            HexPoint = hexPoint;
            HexGrid = hexGrid;
            Actor = actor;
        }

        //i dont want to write this every time, find a way to not do this :TODO
        public override void Draw()
        {
            var v2 = Vector2.Transform(StartV, Matrix.Invert(Camera.Transform));
            Sb.Draw(
                Texture,
                    destinationRectangle: new Rectangle((int)v2.X, (int)v2.Y, (int)Size.X, (int)Size.Y),
                    sourceRectangle: new Rectangle(0, 0, 100, 100),
                    color: Color.White
                    //origin: new Vector2(Size.X / 2, Size.Y / 2)
                );

        }
        public override void OnClick()
        {
            var eventSend = new ActorRotateActionEvent(HexGrid, false);
            eventSend.RotateAction += Actor.Rotate;
            eventSend.OnRotateAction();
        }
    }


    public class ActorRotateActionEvent : EventArgs
    {
        public event EventHandler<ActorRotateActionEvent> RotateAction;

        public HexGrid.HexGrid HexGrid { get; set; }
        public BasicActor Actor { get; set; }
        public bool ClockWise { get; set; }


        public ActorRotateActionEvent(HexGrid.HexGrid hexGrid, bool clockWise)
        {
            HexGrid = hexGrid;
            ClockWise = clockWise;
            //:TODO re do actors can see
        }
        // :TODO probaly abstractable
        public void OnRotateAction()
        {
            var handler = RotateAction;
            if (handler != null)
            {
                handler(this, new ActorRotateActionEvent(HexGrid, ClockWise));
            }
        }
    }
}
