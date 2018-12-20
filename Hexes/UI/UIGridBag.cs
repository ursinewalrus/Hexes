using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hexes.UI
{
    //TODO: modify UI element things builders for dropping in here
    public class UIGridBag
    {
        public List<UIDrawable> GridElements = new List<UIDrawable>();
        public Vector2 UpperLeft;
        public float MaxWidth;
        public float MaxHeight;
        // x/y decoupled from x/y
        public int[] MoveOverAmount = new int[2];
        public int moveOverXIndex;
        public int moveOverYIndex;

        public UIGridBag(Vector2 upperLeft, float maxWidth, float maxHeight, bool fillHorizontal = true)
        {
            UpperLeft = upperLeft;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;

            MoveOverAmount[0] = 0;
            MoveOverAmount[1] = 0;

            if (fillHorizontal)
            {
                moveOverXIndex = 0;
                moveOverYIndex = 1;
            }
            else
            {
                moveOverXIndex = 1;
                moveOverYIndex = 0;
            }
            //fillHorizontal controls index for accessing vars in moved over height/width
            //MoveOverAmount = fillHorizontal ? new Vector2() : 
        }

        public void PlaceElements()
        {
            var currentUpperLeft = new Vector2(UpperLeft.X, UpperLeft.Y);
            float totalMovedOver = 0;
            float totalMovedDown = 0;
            var placed = true;
            foreach (var element in GridElements)
            {
                element.StartV = currentUpperLeft;
                element.Draw();
                
            }
        }
    }
}
