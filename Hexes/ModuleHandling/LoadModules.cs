using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hexes
{
    public class LoadModule
    {
        public Dictionary<string, Dictionary<string, string>> LoadedBackgroundTiles = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, Dictionary<string, string>> LoadedMaps = new Dictionary<string, Dictionary<string, string>>();
        private List<string> ModuleFiles = new List<string>();
        public string ModuleName;
       

        public void GetDirContents(string dirName, string extension)
        {
            foreach (string dir in Directory.GetDirectories(dirName))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    if (Path.GetExtension(file) == extension)
                    {
                        ModuleFiles.Add(file);
                    }
                }
                GetDirContents(dir, extension);
            }
        }

        public LoadModule(string moduleName)
        {
            ModuleName = moduleName.Split('\\').Last();
            GetDirContents(moduleName, ".xml");
            //now have all xml files in a module
            foreach(var moduleFile in ModuleFiles)
            {
                XElement rootNode = XElement.Load(moduleFile);
                switch (rootNode.Name.ToString())
                {
                    case "BackgroundTiles":
                        GetElementsAttributes(rootNode).ToList().ForEach(k => LoadedBackgroundTiles[k.Key] = k.Value);
                        break;
                    case "Maps":
                        GetElementsAttributes(rootNode).ToList().ForEach(k => LoadedMaps[k.Key] = k.Value);
                        break;
                }
            }
        }

        public Dictionary<string, Dictionary<string, string>> GetElementsAttributes(XElement rootNode)
        {
            Dictionary<string, Dictionary<string, string>> tileAttributes = new Dictionary<string, Dictionary<string, string>>();
            var tiles =  rootNode.Elements();
            //only goes down one level from root...
            foreach(XElement tile in tiles)
            {
                tileAttributes[tile.FirstAttribute.Value] = LoadNodeAttributes(tile);
            }
            return tileAttributes;
        }

        public Dictionary<string, string> LoadNodeAttributes(XElement node)
        {
            var nodeAttributes = new Dictionary<string, string>();
            //only goes down one level from node...
            foreach (var elementType in node.Elements())
            {
                node.Element(elementType.Name.ToString()).Attributes().ToList().ForEach(k => nodeAttributes[k.Name.ToString()] = k.Value);
            }
           
            return nodeAttributes;

        }
    }
}
