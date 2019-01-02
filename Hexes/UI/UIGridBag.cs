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

        public UIGridBag(Vector2 upperLeft, float maxWidth, float maxHeight)
        {
            UpperLeft = upperLeft;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }

        public void Draw()
        {

            var nextToPlaced = 0;
            Vector2 lastUpperLeft = new Vector2(UpperLeft.X, UpperLeft.Y);
            Vector2 lastLowerRight = new Vector2(0,0);
            foreach (var element in GridElements)
            {
                element.StartV = lastUpperLeft;
                if (nextToPlaced == 0 || element.StartV.X + element.Size.X < MaxWidth)
                {
                    lastLowerRight = element.StartV + element.Size;
                    lastUpperLeft.X += element.Size.X;
                    nextToPlaced++;
                    element.Draw();
                }
                else
                {
                    element.StartV = new Vector2(UpperLeft.X, lastLowerRight.Y);
                    lastUpperLeft.X += element.Size.X;
                    nextToPlaced = 0;
                    element.Draw();

                }

            }
        }
    }
}
