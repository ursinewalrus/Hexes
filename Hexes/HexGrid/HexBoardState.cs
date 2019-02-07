﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hexes.Actors;
using Hexes.Control;

namespace Hexes.HexGrid
{
    //experimental
    //maybe
    //create / manage hexgrid /ui here
    public class HexBoardState
    {
        public HexGrid ActiveBoard;
        public BasicActor ActiveActor;
        public Camera Camera;

        public HexBoardState(HexGrid grid, Camera camera)
        {
            ActiveBoard = grid;
            Camera = camera;
        }

        public void CheckBoardStateLoop()
        {
            SetNextActorControl();
            //need to, while moving, lock it
            //if someone is moving dont execute below
            //this needs to be futzed with the AP thing isnt quiiiite working
            ActiveBoard.HighlightHex(ActiveActor.Location);
            if (ActiveActor.Controllable)
            {
                if (ActiveActor.TurnState == ActorTurnState.WaitingForTurn)
                {
                    ActiveActor.StartTurn();
                }
                HandleMouse.TacticalViewMouseHandle(ActiveBoard, Camera, ActiveActor);
            }
            else
            {
                ActiveActor.UseAIMoveAction();
            }
        }

        public void SetNextActorControl()
        {
            if(!ActiveBoard.ActorStorage.Any())
            {
               //no ones on the board? 
            }
            ActiveActor = ActiveBoard.ActorStorage.FirstOrDefault(a => (a.TurnState == ActorTurnState.WaitingForTurn || a.TurnState == ActorTurnState.OnTurn));
            if (ActiveActor == null)
            {
                //no one left to move/action, reset it all, re sort, asign
                ActiveBoard.ActorStorage.ForEach(a => a.TurnState = ActorTurnState.WaitingForTurn);
                ActiveBoard.ActorStorage = ActiveBoard.ActorStorage.OrderBy(a => a.Speed).ToList();
                ActiveActor = ActiveBoard.ActorStorage.FirstOrDefault(a => a.TurnState == ActorTurnState.WaitingForTurn);
            }
        }
    }
}
