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
        public Vector2 MouseCords;
        public CardinalDirections.Direction RelativeMouseLocation;
        public MouseState MouseState;
        public HandleMouse(Game1 game)
        {
            MouseState = Mouse.GetState();
            MouseCords.X = MouseState.X;
            MouseCords.Y = MouseState.Y;
            RelativeMouseLocation = GetMouseCardinalDirection(game);
        }


        private CardinalDirections.Direction GetMouseCardinalDirection(Game1 game)
        {
            float mouseX = MouseCords.X;
            float mouseY = MouseCords.Y;

            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            var xScrollTrigger = width / 5;
            var yScrollTrigger = height / 5;

            float XDistance = width - mouseX;
            float YDistance = height - mouseY;


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
