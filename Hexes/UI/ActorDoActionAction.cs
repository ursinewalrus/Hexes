using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Actors;
using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexes.UI
{
    public class ActorDoActionAction : UIDrawable, IUIActions
    {
        public Texture2D Texture { get; set; }
        public Color LineColor { get; set; }
        public BasicActor Actor { get; set; }

        public ActorDoActionAction(BasicActor actor, HexGrid.HexGrid hexGrid)
        {
            //:TODO generalize more or make more options, not sufficient
            ElementName = "DoAction";
            //make some custom one :TODO
            Texture = actor.Texture;
            HexGrid = hexGrid;
            Actor = actor;
        }

        public override void OnClick()
        {
            var eventSend = new ActorDoActionActionEvent("BasicAttackQue",HexGrid);
            eventSend.ActionAction += Actor.DoAction;
            eventSend.OnActionAction();
        }
    }

    public class ActorDoActionActionEvent : EventArgs
    {
        public event EventHandler<ActorDoActionActionEvent> ActionAction;
        private string Action { get; set; }
        public HexGrid.HexGrid HexGrid { get; set; }

        public ActorDoActionActionEvent(string action, HexGrid.HexGrid hexGrid)
        {
            Action = action;
            HexGrid = hexGrid;
        }
        public void OnActionAction()
        {
            var handler = ActionAction;

            if (handler != null)
            {
                handler(this, new ActorDoActionActionEvent(Action, HexGrid));
            }
        }

    }
}
