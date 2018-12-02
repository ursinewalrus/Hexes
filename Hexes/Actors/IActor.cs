using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes.Actors
{
    interface IActor
    {
        void AsignActorData(Dictionary<string, string> actorData); 
    }
}
