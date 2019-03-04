using Hexes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Actors;
using Hexes.Geometry;
using Hexes.Utilities;

namespace Hexes.Control
{
    public static class HandleMouse //and keyboard...
    {
        public static Vector2 MouseCords;
        public static CardinalDirections.Direction RelativeMouseLocation;
        public static MouseState MouseState;

        public static bool PrevMouseClickedStateClicked = false;
        public static bool CompletedClick = false;

        //public static HandleMouse()
        //{
        //    //MouseState = Mouse.GetState();
        //    //MouseCords.X = MouseState.X;
        //    //MouseCords.Y = MouseState.Y;
        //    //RelativeMouseLocation = GetMouseCardinalDirection(game);
        //    //if(MouseState.LeftButton == ButtonState.Pressed)
        //    //{
        //    //    PrevMouseClickedStateClicked = true;
        //    //    CompletedClick = false;

        //    //}
        //    //if (PrevMouseClickedStateClicked && MouseState.LeftButton != ButtonState.Pressed)
        //    //{
        //    //    CompletedClick = true;
        //    //}

        //}
        public static void SetMouseState(Game1 game)
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
        private static CardinalDirections.Direction GetMouseCardinalDirection(Game1 game)
        {
          
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

        public static void TacticalViewMouseHandle(HexGrid.HexGrid hexMap, Camera camera, BasicActor actionableActor)
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
                Debugger.Log("Hover Hex " + selHex.R + ", " + selHex.Q);

                // :TODO this is annoying to do
                hexMap.HexStorage.Where(h => h.Key.Equals(selHex)).First().Value.Hovered = true;
            }
            if (CompletedClick/*mouseInfo.MouseState.LeftButton == ButtonState.Pressed*/)
            {
                //Debug.Log("Clicked ScreenCords " + MouseCords.X + ", " + MouseCords.Y);
                //Debug.Log("Clicked MouseCords " + conv.X + ", " + conv.Y);

                foreach (var bag in ActiveHexUIElements.AvailibleUIElements)
                {
                    foreach (var visibleElement in bag.Value.GridElements)
                    {
                        Vector2 elementLoc = Vector2.Transform(visibleElement.StartV,
                            Matrix.Invert(camera.Transform));
                        Vector2 elementSize = visibleElement.Size;
                        if (conv.X >= elementLoc.X && conv.Y >= elementLoc.Y && conv.X <= elementLoc.X + elementSize.X &&
                            conv.Y <= elementLoc.Y + elementSize.Y)
                        {
                            visibleElement.OnClick();
                            return;
                        }
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
                        hexKey.Value.Highlighted = true;
                        var seeable = hexMap.ActiveActor.CanSee();
                        hexMap.HighlightHexes(seeable);
                        #region create actor UI elements


                        //UIGridBag -> where do we put it
                        //show some text ui stats if not actionable :TODO
                        CreateActiveActorUIElements(hexMap,actionableActor);
                        CreateTextUIElements(hexMap, actionableActor);

                        #endregion

                    }
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
                ActiveHexUIElements.AvailibleUIElements.Remove(UIGridBagLocations.Left);
                hexMap.UnHighlightAll();

            }
        }

        public static void CreateActiveActorUIElements(HexGrid.HexGrid hexMap, BasicActor actionableActor)
        {
            if (hexMap.ActiveActor == actionableActor)
            {
                //:TODO dynamically choose formatting, maybe how many other elements a UI element will let share a line with it
                UIGridBag actorActionsUIBag = new UIGridBag(UIGridBagLocationCordinates.Left, new List<int>() { 1, 2, 1 });
                ActiveHexUIElements.AvailibleUIElements.Remove(UIGridBagLocations.Left);
                //maybe just loop through, remove all actor related ones, get list first, remove second :TODO

                var placedElementCount = 0;
                hexMap.ActiveActor.DefaultActions.ForEach(a =>
                {
                    UIDrawable actionElement = (ActionHandler.ActionsList.ContainsKey(a)) ?
                        new ActorDoActionAction(hexMap.ActiveActor, hexMap, a, ActionHandler.ActionsList[a]) :
                        new ActorDoActionAction(hexMap.ActiveActor, hexMap, a, new Dictionary<ActionArgs, string> { { ActionArgs.Type, a } });
                    actorActionsUIBag.GridElements.Add(actionElement);
                    //:TODO dubious
                    if (actorActionsUIBag.PerRow.Sum() <= placedElementCount)
                    {
                        actorActionsUIBag.PerRow.Add(1);
                    }
                    placedElementCount++;
                });
                ActiveHexUIElements.AvailibleUIElements[UIGridBagLocations.Left] = actorActionsUIBag;

            }
        }
        public static void CreateGenericActorUIElements(HexGrid.HexGrid hexMap, BasicActor actionableActor)
        {

        }

        //any? specific for if selecting non active actor?
        //should also have textures around them 
        //also harcoding, bad :TODO
        public static void CreateTextUIElements(HexGrid.HexGrid hexMap, BasicActor actionableActor)
        {
            if (ActiveHexUIElements.AvailibleUIElements[UIGridBagLocations.Left] == null)
            {
                UIGridBag actorActionsUIBag = new UIGridBag(UIGridBagLocationCordinates.Left, new List<int>());
                ActiveHexUIElements.AvailibleUIElements[UIGridBagLocations.Left] = actorActionsUIBag;
            }
            ActiveHexUIElements.AvailibleUIElements[UIGridBagLocations.Left].GridElements.Add(new TextUIElement(hexMap.ActiveActor, TextUIStatics.ActorStats.CurrentHP));
            ActiveHexUIElements.AvailibleUIElements[UIGridBagLocations.Left].PerRow.Add(1);
        }
    }
}
