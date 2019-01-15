using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexes.Geometry;
using Microsoft.Xna.Framework;

namespace Hexes.UI
{
    //TODO: modify UI element things builders for dropping in here
    public class UIGridBag
    {
        public List<UIDrawable> GridElements = new List<UIDrawable>();
        public Vector2 UpperLeft;
        public List<int> PerRow;
        // x/y decoupled from x/y
        public int[] MoveOverAmount = new int[2];

        public UIGridBag(Vector2 upperLeft, List<int> perRow)
        {
            UpperLeft = upperLeft;
            PerRow = perRow;
        }

        public void Draw()
        {
#if DEBUG
            //var edges = new List<Line>()
            //{
            //    new Line(UpperLeft.X,UpperLeft.Y,UpperLeft.X+MaxWidth,UpperLeft.Y,2,Color.Black),
            //    new Line(UpperLeft.X,UpperLeft.Y,UpperLeft.X,UpperLeft.Y+MaxHeight,2,Color.Black),
            //    new Line(UpperLeft.X,UpperLeft.Y+MaxHeight,UpperLeft.X+MaxWidth,UpperLeft.Y+MaxHeight,2,Color.Black),
            //    new Line(UpperLeft.X+MaxWidth,UpperLeft.Y,UpperLeft.X+MaxWidth,UpperLeft.Y+MaxHeight,2,Color.Black)
            //};
            //edges.ForEach(e => e.Draw()); -> need camera transform
#endif
            int rowIndex = 0;
            float movedOver = 0;
            var rowCounter = new List<int>(PerRow);
            //we are going to assume all UI elements are the same height atm
            foreach (var element in GridElements)
            {
                if (rowCounter[rowIndex] < 1 && rowIndex + 1 <= rowCounter.Count())
                {
                    rowIndex++;
                    movedOver = 0;
                }
                if(rowCounter[rowIndex] > 0)
                {
                    element.StartV = new Vector2(UpperLeft.X + movedOver, UpperLeft.Y + (rowIndex * element.Size.Y) / 2);
                    element.Draw();
                    rowCounter[rowIndex]--;
                    movedOver += element.Size.X / 2;
                }
            }
        }
    }
}
