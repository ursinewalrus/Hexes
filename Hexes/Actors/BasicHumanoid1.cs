using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Hexes.Actors
//{
//    public class BasicActor: AbstractActor
//    {

//        public BasicActor(Dictionary<string, Dictionary<string, string>> actorData, string moduleName)
//        {
//            ModuleName = moduleName;
//            Name = actorData.First().Key;
//            AsignActorData(actorData[Name]);
//        }
//        public override void AsignActorData(Dictionary<string, string> attributes)
//        {
//            MoveDistance = Convert.ToInt32(attributes["moveDistance"]);
//            HP = Convert.ToInt32(attributes["defaultHP"]);
//            string assetPath = @"Modules\" + ModuleName + @"\" + attributes["texture"];
//            FileStream fs = new FileStream(assetPath, FileMode.Open);
//            Texture = Texture2D.FromStream(GraphicsDevice, fs);
//        }
//    }
//}
