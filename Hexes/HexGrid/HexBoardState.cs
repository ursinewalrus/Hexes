using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public bool PCControl;

        public HexBoardState(HexGrid grid, Camera camera)
        {
            ActiveBoard = grid;
            Camera = camera;
        }

        public void CheckBoardStateLoop()
        {
            SetNextActorControl();
            if (PCControl)
            {
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
            ActiveActor = ActiveBoard.ActorStorage.FirstOrDefault(a => !a.Moved);
            if (ActiveActor != null)
            {
                PCControl = ActiveActor.Controllable;
            }
            else
            {
                ActiveBoard.ActorStorage.ForEach(a => a.Moved = false);
            }
        }
    }
}
