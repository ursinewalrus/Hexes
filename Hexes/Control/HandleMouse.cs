using Hexes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Control
{
    public class HandleMouse
    {
        public Vector2 MouseCords;
        public CardinalDirections.Direction RelativeMouseLocation;
        public MouseState MouseState;
        public static Debugger Debug;
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

            var xScrollTrigger = width / 8;
            var yScrollTrigger = height / 8;

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

        public static void GridSelect(HandleMouse mouseInfo, HexGrid.HexGrid hexMap, Camera camera)
        {
            //Probably in whatever houses the ActorActions instantiation
            //var actorUi = new ActorActions(new );
            //actorUi.DrawActorActions();
            if (mouseInfo.MouseState.LeftButton == ButtonState.Pressed)
            {
                var conv = Vector2.Transform(mouseInfo.MouseCords, Matrix.Invert(camera.Transform));
                var selHex = hexMap.SelectedHex(conv);
                if (selHex != null)
                {
                    Debug.Log("Clicked Hex " + selHex.R + ", " + selHex.Q);

                    var hexKey =
                        hexMap.HexStorage.Where(h => h.Key.R == selHex.R && h.Key.Q == selHex.Q).FirstOrDefault();
                    var actorKey =
                        hexMap.ActorStorage.Where(actor => actor.Location.Equals(selHex)).FirstOrDefault();

                    if (hexKey.Key != null)
                        hexMap.ActiveHex = hexMap.HexStorage[hexKey.Key];

                    if (actorKey != null)
                    {
                        hexMap.ActiveActor = actorKey;

                        //inMoveDistance.ForEach(h => h.Color = Color.Red ); -> alpha channel?
                        //we need a custom contains method, too many of these -> override enum thing probs

                    }
                    if (hexMap.ActiveActor != null)
                    {
                        var inMoveDistance = hexMap.AllInRadiusOf(hexMap.ActiveActor.Location, hexMap.ActiveActor.MoveDistance);
                        if (inMoveDistance.Any(h => h.R == selHex.R && h.Q == selHex.Q))
                            hexMap.ActiveActor.Location = selHex;
                    }
                    if (hexMap.ActiveActor == null)
                    {
                        //HexMap.ActiveActor
                    }
                }
            }
            if (mouseInfo.MouseState.RightButton == ButtonState.Pressed)
            {
                hexMap.ActiveHex = null;
                hexMap.ActiveActor = null;
            }
        }
    }
}
