using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexes.Utilities
{
    public class Debugger : Drawable
    {
        //set in draw each loop -> probably bad :TODO
        public Debugger()
        {
        }

        //dont actually be writing these to the screen, needs to be an object so it actually persists
        public static void Log(string logMsg)
        {
            //Vector2.Transform(mouseInfo.MouseCords, Matrix.Invert(GameCamera.Transform));

            Sb.DrawString(Font, logMsg, Vector2.Transform(new Vector2(5,5), Matrix.Invert(Camera.Transform)), Color.Black );
        }
        public static void Log(string logMsg, Vector2 cord)
        {
            Sb.DrawString(Font, logMsg, cord, Color.Black);
        }

        public static void Log(string logMsg, Vector2 cord, SpriteFont font)
        {
            Sb.DrawString(font, logMsg, cord, Color.Black);
        }

    }
}
