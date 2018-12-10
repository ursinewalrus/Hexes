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

        public static void TacticalViewClick(HandleMouse mouseInfo, HexGrid.HexGrid hexMap, Camera camera)
        {
            //Probably in whatever houses the ActorActions instantiation
            //var actorUi = new ActorActions(new );
            //actorUi.DrawActorActions();
            if (mouseInfo.MouseState.LeftButton == ButtonState.Pressed)
            {

                var conv = Vector2.Transform(mouseInfo.MouseCords, Matrix.Invert(camera.Transform));
                foreach (var visibleElement in ActiveHexUIElements.AvailibleUIElements)
                {
                    Vector2 elementLoc = Vector2.Transform(visibleElement.Value.StartV, Matrix.Invert(camera.Transform));
                    Vector2 elementSize = visibleElement.Value.Size;
                    if (conv.X >= elementLoc.X && conv.Y >= elementLoc.Y && conv.X <= elementLoc.X + elementSize.X &&
                        conv.Y <= elementLoc.Y + elementSize.Y)
                    {
                        //how do param passing here, how do we know what it needs, can i pass a function maybe
                        visibleElement.Value.OnClick();
                        return;
                    }
                }

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
                        if (actorKey.Controllable)
                        {
                            hexMap.ActiveActor = actorKey;
                            ActiveHexUIElements.AvailibleUIElements.Remove("ActorActions");
                            ActiveHexUIElements.AvailibleUIElements["ActorActions"] =
                                new ActorActions(hexMap.ActiveActor, new Vector2(100, 100));
                            //inMoveDistance.ForEach(h => h.Color = Color.Red ); -> alpha channel?
                            //we need a custom contains method, too many of these -> override enum thing probs
                        }

                    }
                    if (hexMap.ActiveActor != null)
                    {
                        var inMoveDistance = hexMap.AllInRadiusOf(hexMap.ActiveActor.Location, hexMap.ActiveActor.MoveDistance);
                        if (inMoveDistance.Any(h => h.R == selHex.R && h.Q == selHex.Q))
                            //move this somewhere an element onclick can get at it
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
                ActiveHexUIElements.AvailibleUIElements.Remove("ActorActions");
            }
        }
    }
}
