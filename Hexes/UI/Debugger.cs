﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexes.UI
{
    public class Debugger : Drawable
    {
        public SpriteBatch Sb;
        private SpriteFont Font;
        //set in draw each loop -> probably bad :TODO
        public Matrix CamLoc { get; set; }

        public Debugger(SpriteBatch sb, SpriteFont font)
        {
            Sb = sb;
            Font = font;
        }

        //dont actually be writing these to the screen, needs to be an object so it actually persists
        public void Log(string logMsg)
        {
            Sb.DrawString(Font, logMsg, Vector2.Transform(new Vector2(5,5), CamLoc), Color.Black );
        }
        public void Log(string logMsg, Vector2 cord)
        {
            Sb.DrawString(Font, logMsg, cord, Color.Black);
        }

    }
}
