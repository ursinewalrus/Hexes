using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexes.UI
{
    public class TextUIElement: UIDrawable, IUIActions
    {
        public String UIText { get; set; }
        public SpriteFont UIFont { get; set; }
        public int UITextSize { get; set; }
        public Color UIFontColor { get; set; }
        public BasicActor Actor { get; set; }
        public TextUIStatics.ActorStats ActorStatType { get; set; }
        //:TODO should have a texture property
        public TextUIElement(BasicActor actor, TextUIStatics.ActorStats actorStatType, string defaultText = null)
        {
            Actor = actor;
            ActorStatType = actorStatType;
            UIText = defaultText ?? "";
        }
        public override void OnClick()
        {
            //tooltip probably
            //throw new NotImplementedException();
        }

        public override void Draw()
        {
            //should have some kind of update function to account for changing state
            UIText = UpdateText();
            base.Draw(UIText,Font,(int)Size.X,Color.Black);
        }
        //https://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp
        public string UpdateText()
        {
            switch (ActorStatType)
            {
                case TextUIStatics.ActorStats.Attack:
                    return "Attack Ap: " + Actor.ActiveTurnState[APUseType.Attack].ToString();
                case TextUIStatics.ActorStats.CurrentHP:
                    return $"HP: {Actor.HP}/{Actor.HP - Actor.DamageTaken}";
                case TextUIStatics.ActorStats.Defense:
                    return "Defense AP: " + Actor.ActiveTurnState[APUseType.Defend].ToString();
                case TextUIStatics.ActorStats.Movment:
                    return "Moves Left " + Actor.ActiveTurnState[APUseType.Movement].ToString();
                case TextUIStatics.ActorStats.Rotate:
                    return "Rotations Left " + Actor.ActiveTurnState[APUseType.Rotation].ToString();
                case TextUIStatics.ActorStats.TotalAP:
                    return "Total AP Left " + Actor.ActiveTurnState[APUseType.TotalAp].ToString();
                default:
                    return UIText;
            }
        }
    }
}
