using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hexes
{
    class LoadModules
    {
        private static string ModulesDir = Environment.CurrentDirectory + @"\Modules\";
        private static List<string> ModuleFiles = new List<string>();

        public static void GetModules()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ModulesDir);
            List<DirectoryInfo> modules =  dirInfo.GetDirectories().ToList();

            foreach(var module in modules)
            {
                var moduleFullPathName = ModulesDir + module.Name;
                LoadModule(moduleFullPathName);
            }
            ;
        }

        public static void GetDirContents(string dirName, string extension)
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

        public static void LoadModule(string moduleName)
        {

            Dictionary<string, Dictionary<string, string>> loadedBackgroundTiles = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> loadedMaps = new Dictionary<string, Dictionary<string, string>>();

            GetDirContents(moduleName, ".xml");
            //now have all xml files in a module
            foreach(var moduleFile in ModuleFiles)
            {
                XElement rootNode = XElement.Load(moduleFile);
                switch (rootNode.Name.ToString())
                {
                    case "BackgroundTiles":
                        //loadedBackgroundTiles = LoadBackground(rootNode);
                        GetElementsAttributes(rootNode).ToList().ForEach(k => loadedBackgroundTiles[k.Key] = k.Value);
                        break;
                    case "Maps":
                        GetElementsAttributes(rootNode).ToList().ForEach(k => loadedMaps[k.Key] = k.Value);
                        break;
                }
            }
        }

        public static Dictionary<string, Dictionary<string, string>> GetElementsAttributes(XElement rootNode)
        {
            Dictionary<string, Dictionary<string, string>> tileAttributes = new Dictionary<string, Dictionary<string, string>>();
            var tiles =  rootNode.Elements();
            //only goes down one level from root...
            foreach(XElement tile in tiles)
            {
                tileAttributes[tile.FirstAttribute.ToString()] = LoadNodeAttributes(tile);
            }
            return tileAttributes;
        }

        public static Dictionary<string, string> LoadNodeAttributes(XElement node)
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
