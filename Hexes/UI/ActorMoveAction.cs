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
        public static Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }
        public HexPoint HexPointTo { get; set; }


        public ActorMoveAction(BasicActor actor, HexPoint hexPointTo, HexGrid.HexGrid hexGrid)
        {
            ElementName = "ActorMoveActions";
            //maaaybe pass as param
            //not centerpoint, upper left corner
            //StartV = new Vector2(5, 40);
            HexPointTo = hexPointTo;
            HexGrid = hexGrid;
            Actor = actor;

        }
        public override void Draw()
        {
            base.Draw(Texture);
        }
        //public delegate void CustomEventHandler(object sender, ActorMoveActionEvent a);
        public override void OnClick()
        {
            var eventSend = new ActorMoveActionEvent(HexPointTo,HexGrid);
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

