using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public static class HandleMouse
    {
        public static void HandleMouseAction(Game game, MouseState mousestate)
        {
            if(mousestate.LeftButton == ButtonState.Pressed)
            {
                var xCor = mousestate.X;
                var ycor = mousestate.Y;
                var w = game.GraphicsDevice.Viewport.Width;
                var h = game.GraphicsDevice.Viewport.Height;
                if(Math.Abs(xCor - w) < 50)
                {
                    //http://community.monogame.net/t/simple-2d-camera/9135
                }
                ;
            }
        }

    }
}
