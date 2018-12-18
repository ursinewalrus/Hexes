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
    // :TODO inherit draw method
    public class ActorMoveAction : UIDrawable, IUIActions
    {
        public string ElementName = "ActorMoveActions";
        public Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }
        public HexPoint HexPoint { get; set; }
        public HexGrid.HexGrid HexGrid { get; set; }



        public ActorMoveAction(BasicActor actor, HexPoint hexPoint, HexGrid.HexGrid hexGrid)
        {
            Texture = actor.Texture;
            //maaaybe pass as param
            //not centerpoint, upper left corner
            StartV = new Vector2(5, 40);
            Size = new Vector2(100,100);
            HexPoint = hexPoint;
            HexGrid = hexGrid;
            Actor = actor;

        }
        public override void Draw()
        {
            var v2 = Vector2.Transform(StartV, Matrix.Invert(Camera.Transform));
            Sb.Draw(
                Texture,
                    destinationRectangle: new Rectangle((int)v2.X, (int)v2.Y, (int)Size.X , (int)Size.Y ),
                    sourceRectangle: new Rectangle(0, 0, 100, 100),
                    color: Color.White
                // origin: new Vector2(Size.X / 2, Size.Y / 2)
                );
            //Actor.Draw(new FloatPoint(v2.X, v2.Y), Size);
            //Sb.DrawString(Font, "MOVE", Vector2.Transform(StartV, Matrix.Invert(Camera.Transform)), Color.Black);
        }

        //public delegate void CustomEventHandler(object sender, ActorMoveActionEvent a);
        public override void OnClick()
        {
            var eventSend = new ActorMoveActionEvent(HexPoint,HexGrid);
            eventSend.MoveAction += Actor.MoveTo;
            eventSend.OnMoveAction();
            // MoveActionSent(null, new ActorMoveActionEvent(Actor));
            //
        }


    }
    public class ActorMoveActionEvent : EventArgs
    {
        public event EventHandler<ActorMoveActionEvent> MoveAction;

        public HexPoint Location { get; set; }
        public HexGrid.HexGrid HexGrid { get; set; }
        public ActorMoveActionEvent(HexPoint location, HexGrid.HexGrid hexGrid)
        {
            Location = location;
            HexGrid = hexGrid;
            //:TODO everyone's can see should be re run
        }
        // :TODO probaly abstractable
        public void OnMoveAction()
        {
            var handler = MoveAction;
            if (handler != null)
            {
                handler(this, new ActorMoveActionEvent(Location, HexGrid));
            }
        }
    }
}

