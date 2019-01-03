using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Control;

namespace Hexes.HexGrid
{
    //experimental
    //maybe
    //create / manage hexgrid /ui here
    public class HexBoardState
    {
        public HexGrid ActiveBoard;
        public Camera Camera;
        public bool PCControl;

        public HexBoardState(HexGrid grid, Camera camera)
        {
            ActiveBoard = grid;
            Camera = camera;
        }

        public void CheckBoardStateLoop()
        {
            if (PCControl)
            {
                HandleMouse.TacticalViewMouseHandle(ActiveBoard, Camera);
            }
        }
    }
}
