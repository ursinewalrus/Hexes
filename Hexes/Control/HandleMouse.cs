using Hexes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Geometry;

namespace Hexes.Control
{
    public class HandleMouse //and keyboard...
    {
        public Vector2 MouseCords;
        public CardinalDirections.Direction RelativeMouseLocation;
        public MouseState MouseState;
        public static Debugger Debug;

        public bool PrevMouseClickedStateClicked = false;
        public bool CompletedClick = false;

        public HandleMouse()
        {
            //MouseState = Mouse.GetState();
            //MouseCords.X = MouseState.X;
            //MouseCords.Y = MouseState.Y;
            //RelativeMouseLocation = GetMouseCardinalDirection(game);
            //if(MouseState.LeftButton == ButtonState.Pressed)
            //{
            //    PrevMouseClickedStateClicked = true;
            //    CompletedClick = false;

            //}
            //if (PrevMouseClickedStateClicked && MouseState.LeftButton != ButtonState.Pressed)
            //{
            //    CompletedClick = true;
            //}

        }
        public void SetMouseState(Game1 game)
        {
            MouseState = Mouse.GetState();
            MouseCords.X = MouseState.X;
            MouseCords.Y = MouseState.Y;
            RelativeMouseLocation = GetMouseCardinalDirection(game);
            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                PrevMouseClickedStateClicked = true;
                CompletedClick = false;

            }
            if (PrevMouseClickedStateClicked && MouseState.LeftButton == ButtonState.Released)
            {
                CompletedClick = true;
                PrevMouseClickedStateClicked = false;
            }
            else if (CompletedClick)
            {
                CompletedClick = false;
            }
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
            var keyState = Keyboard.GetState();

            

            if(keyState.IsKeyDown(Keys.D))
            {
                if(keyState.IsKeyDown(Keys.S))
                {
                    return CardinalDirections.Direction.SouthEast;
                }
                else if(keyState.IsKeyDown(Keys.W))
                {
                    return CardinalDirections.Direction.NorthEast;
                }
                return CardinalDirections.Direction.East;
            }
            else if(keyState.IsKeyDown(Keys.A))
            {
                if (keyState.IsKeyDown(Keys.S))
                {
                    return CardinalDirections.Direction.SouthWest;
                }
                else if(keyState.IsKeyDown(Keys.W))
                {
                    return CardinalDirections.Direction.NorthWest;
                }
                return CardinalDirections.Direction.West;
            }
            else if(keyState.IsKeyDown(Keys.S))
            {
                return CardinalDirections.Direction.South;
            }
            else if(keyState.IsKeyDown(Keys.W))
            {
                return CardinalDirections.Direction.North;
            }
            return CardinalDirections.Direction.Centered;

        }

        public void TacticalViewMouseHandle(HexGrid.HexGrid hexMap, Camera camera)
        {
            //Probably in whatever houses the ActorActions instantiation
            //var actorUi = new ActorActions(new );
            //actorUi.DrawActorActions();

            var conv = Vector2.Transform(MouseCords, Matrix.Invert(camera.Transform));

            var selHex = hexMap.SelectedHex(conv);

            hexMap.HexStorage.ToList().ForEach(h => h.Value.Hovered = false);
            //:TODO also annoying
            if (selHex != null)
            {
                Debug.Log("Hover Hex " + selHex.R + ", " + selHex.Q);

                // :TODO this is annoying to do
                hexMap.HexStorage.Where(h => h.Key.Equals(selHex)).First().Value.Hovered = true;
            }
            if (CompletedClick/*mouseInfo.MouseState.LeftButton == ButtonState.Pressed*/)
            {
                //Debug.Log("Clicked ScreenCords " + MouseCords.X + ", " + MouseCords.Y);
                //Debug.Log("Clicked MouseCords " + conv.X + ", " + conv.Y);

                foreach (var visibleElement in ActiveHexUIElements.AvailibleUIElements)
                {
                    Vector2 elementLoc = Vector2.Transform(visibleElement.Value.StartV, Matrix.Invert(camera.Transform));
                    Vector2 elementSize = visibleElement.Value.Size;
                    if (conv.X >= elementLoc.X && conv.Y >= elementLoc.Y && conv.X <= elementLoc.X + elementSize.X &&
                        conv.Y <= elementLoc.Y + elementSize.Y)
                    {
                        visibleElement.Value.OnClick();
                        return;
                    }
                }
               
                if (selHex != null)
                {

                    var hexKey =
                        hexMap.HexStorage.FirstOrDefault(h => h.Key.Equals(selHex));
                    var actorKey =
                        hexMap.ActorStorage.FirstOrDefault(actor => actor.Location.Equals(selHex));
                    //probably should be moved somewhere
                    hexMap.UnHighlightAll();
                    if (hexKey.Key != null)
                        hexMap.ActiveHex = hexKey;

                    if (hexMap.ActiveActor != null && hexMap.ActiveActor.Controllable)
                    {
                        //sets selected hex, :TODO when get select texture, have that set its own value
                        hexKey.Value.Highlighted = true;
                        //var moveable = hexMap.ActiveActor.AllInMoveRange(hexMap);
                        //moveable.ForEach(h => hexMap.HexStorage[h].Highlighted = true);

                        var seeable = hexMap.ActiveActor.CanSee(hexMap.ActiveActor.Location, hexMap);
                        ;
                        seeable.ForEach((s) =>
                        {
                            var insight = hexMap.HexStorage.Where(h => h.Key.Equals(s)).FirstOrDefault();
                            if(insight.Value != null)
                            {
                                insight.Value.Highlighted = true;
                            }
                        });

                        //bundle this all, whatever is being selected, create all UI elements for it, probably metatype property on basicactor 
                        #region create actor UI elements
                        //or just make this its own list of elements
                        //Virtual "grid" to  hold these so can just add more elements without needing to do any manual sizing :TODO -> priority
                        ActiveHexUIElements.AvailibleUIElements.Remove("ActorMoveActions"); //maybe just loop through, remove all actor related ones, get list first, remove second :TODO
                        var moveElement = new ActorMoveAction(hexMap.ActiveActor, hexKey.Key, hexMap);
                        ActiveHexUIElements.AvailibleUIElements[moveElement.ElementName] = moveElement;

                        ActiveHexUIElements.AvailibleUIElements.Remove("ActorRotateClockWiseActions");
                        var rotateClockwiseElement = new ActorRotateClockWise(hexMap.ActiveActor, hexKey.Key, hexMap);
                        ActiveHexUIElements.AvailibleUIElements[rotateClockwiseElement.ElementName] = rotateClockwiseElement;

                        ActiveHexUIElements.AvailibleUIElements.Remove("ActorRotateCounterClockWiseActions");
                        var rotateCounterClockwiseElement = new ActorRotateCounterClockWise(hexMap.ActiveActor, hexKey.Key, hexMap);
                        ActiveHexUIElements.AvailibleUIElements[rotateCounterClockwiseElement.ElementName] = rotateCounterClockwiseElement;


                        #endregion  
                        //inMoveDistance.ForEach(h => h.Color = Color.Red ); -> alpha channel?
                        //we need a custom contains method, too many of these -> override enum thing probs

                    }
                    //if (hexMap.ActiveActor != null)
                    //{
                    //    var inMoveDistance = hexMap.AllInRadiusOf(hexMap.ActiveActor.Location, hexMap.ActiveActor.MoveDistance);
                    //    if (inMoveDistance.Any(h => h.R == selHex.R && h.Q == selHex.Q))
                    //        //move this somewhere an element onclick can get at it
                    //        hexMap.ActiveActor.Location = selHex;
                    //}
                    if (hexMap.ActiveActor == null)
                    {
                        hexMap.ActiveActor = actorKey;
                    }
                }
            }
            //temp, not all conditions for UI in, remove :TODO
            if (MouseState.RightButton == ButtonState.Pressed)
            {
                hexMap.ActiveHex = new KeyValuePair<HexPoint, Hex>();
                hexMap.ActiveActor = null;
                ActiveHexUIElements.AvailibleUIElements.Remove("ActorMoveActions");
                ActiveHexUIElements.AvailibleUIElements.Remove("ActorRotateClockWiseActions");
                ActiveHexUIElements.AvailibleUIElements.Remove("ActorRotateCounterClockWiseActions");
                hexMap.UnHighlightAll();

            }
        }
    }
}
