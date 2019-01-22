using System;
using System.Collections.Generic;
using System.IO;
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
        Dictionary<string, string> ActionArgs { get; set; }
        public ActorDoActionAction(BasicActor actor, HexGrid.HexGrid hexGrid, string actionName, Dictionary<string, string> actionArgs )
        {

            ElementName = actionName;
            //make some custom one :TODO
            string assetPath = @"Modules\" + actionArgs["moduleName"] + @"\" + actionArgs["texture"];
            FileStream fs = new FileStream(assetPath, FileMode.Open);
            Texture = Texture2D.FromStream(GraphicsDevice, fs);
            HexGrid = hexGrid;
            Actor = actor;
            ActionArgs = actionArgs;
        }

        public override void OnClick()
        {
            var eventSend = new ActorDoActionActionEvent("BasicAttackQue",HexGrid, ActionArgs);
            eventSend.ActionAction += Actor.DoAction;
            eventSend.OnActionAction();
        }

        public override void Draw()
        {
            base.Draw(Texture);
        }
    }

    public class ActorDoActionActionEvent : EventArgs
    {
        public event EventHandler<ActorDoActionActionEvent> ActionAction;
        private string Action { get; set; }
        public HexGrid.HexGrid HexGrid { get; set; }

        Dictionary<string, string> ActionArgs { get; set; }
        public ActorDoActionActionEvent(string action, HexGrid.HexGrid hexGrid, Dictionary<string, string> actionArgs )
        {
            Action = action;
            HexGrid = hexGrid;
            ActionArgs = actionArgs;
        }
        public void OnActionAction()
        {
            var handler = ActionAction;

            if (handler != null)
            {
                handler(this, new ActorDoActionActionEvent(Action, HexGrid, ActionArgs));
            }
        }

    }
}
