using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hexes.UI
{
    public static class ActiveHexUIElements
    {
        //public static Dictionary<string, UIDrawable> AvailibleUIElements = new Dictionary<string, UIDrawable>();
        public static Dictionary<string,UIGridBag> AvailibleUIElements = new Dictionary<string, UIGridBag>();
    }

    public static class TextUIStatics
    {
        public enum ActorStats
        {
            CurrentHP,
            TotalAP,
            Movment,
            Attack,
            Defense,
            Rotate,
        }
    }
    public static class UIGridBagLocationCordinates
    {
        public static Vector2 Left =  new Vector2(5, 40);
        //public static Vector2 Right = new Vector2(5, 40);
        //public static Vector2 Bottom = new Vector2(5, 40);
        //public static Vector2 Top = new Vector2(5, 40);
    }

    public static class UIGridBagLocations
    {
        public static string Left = "Left";
        public static string Right = "Right";
        public static string Top = "Top";
        public static string Bottom = "Bottom";
    }

}
