using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    public class HandleMouse
    {
        public CardinalDirections.Direction RelativeMouseLocation;
        public HandleMouse(Game1 game)
        {
            RelativeMouseLocation = GetMouseCardinalDirection(game);
        }


        private CardinalDirections.Direction GetMouseCardinalDirection(Game1 game)
        {
            var mouseState = Mouse.GetState();
            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            var xScrollTrigger = width / 5;
            var yScrollTrigger = height / 5;

            float XDistance = width - mouseX;
            float YDistance = height - mouseY;

            if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                return CardinalDirections.Direction.Centered;
            }

            if (XDistance < xScrollTrigger)
            {
                if(YDistance < yScrollTrigger)
                {
                    return CardinalDirections.Direction.SouthEast;
                }
                else if(height - yScrollTrigger < YDistance)
                {
                    return CardinalDirections.Direction.NorthEast;
                }
                return CardinalDirections.Direction.East;
            }
            else if( width - xScrollTrigger < XDistance)
            {
                if (YDistance < yScrollTrigger)
                {
                    return CardinalDirections.Direction.SouthWest;
                }
                else if (height - yScrollTrigger < YDistance)
                {
                    return CardinalDirections.Direction.NorthWest;
                }
                return CardinalDirections.Direction.West;
            }
            else if(YDistance < yScrollTrigger)
            {
                return CardinalDirections.Direction.South;
            }
            else if( height - yScrollTrigger < YDistance)
            {
                return CardinalDirections.Direction.North;
            }
            return CardinalDirections.Direction.Centered;

        }
    }
}
