using Hexes.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hexes.Utilities
{
    public class DebugLine
    {
        public Line Line { get; set; }
        public Dictionary<Vector2, string> DebugStrings { get; set; }

        public DebugLine()
        {
            DebugStrings = new Dictionary<Vector2, string>();
        }

        public void DrawDebugStrings()
        {
            foreach (var debugString in DebugStrings)
            {
                Debugger.Log(debugString.Value,debugString.Key);
            }
        }
    }
}
