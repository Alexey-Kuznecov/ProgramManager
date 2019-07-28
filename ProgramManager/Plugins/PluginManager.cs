using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ProgramManager.Plugins.Exceptions;
using ProgramManager.Services;

namespace ProgramManager.Plugins
{
    [DebuggerStepThrough]
    class PluginManager
    {
        private const string Name = "../../DataPlugins.bin"; // File name for serialization.
        public static Action<object> PluginLoader;
        internal static List<Plugin> RegistPlugins = new List<Plugin>(); // List plugin registered.
        /// <summary>
        /// Registers plugin and serialize it. If IconEditor type is already registered
        /// the rest type will be ignored. Only the first plugin of the same type will be registered.
        /// </summary>
        /// <param name="inst"></param>
        public static void Register(Plugin inst)
        {
            if (RegistPlugins.All(p => p.Type != inst.Type))
                RegistPlugins.Add(inst);
            else throw new PluginTypeDoubleDefinedException();
            Serialization.BinSerialize(RegistPlugins, Name);
        }
        /// <summary>
        /// It executes a plugin that has been registered,
        /// plug-ins can be of only one type, 
        /// </summary>
        /// <param name="type">Type of plugin to use.</param>
        /// <returns></returns>
        public static Plugin Execute(PluginType type)
        {
            object inst;
            Serialization.BinDeserialize(out inst, Name);
            Plugin plugin = ((List<Plugin>)inst).SingleOrDefault(p => p.Type == type);
            
            if (plugin != null)
                return plugin;
            throw new PluginMissingException("Plugin " + type + " is missing or disabled..");
        }
        /// <summary>
        /// Initializes plugin using data configuration file. 
        /// Configuration file must be placement in the root folder of the plugins .
        /// </summary>
        public static void PluginReader()
        {
            if (!File.Exists(Name))
            {
                XElement root = XElement.Load("../../Plugins/plugin.xml");
                var plugins = from element in root.Elements() select element;
                
                PluginType type;
                foreach (var element in plugins)
                {
                    Plugin plugin = new Plugin();
                    Enum.TryParse(element.Attribute("Type")?.Value, true, out type);
                    plugin.Type = type;
                    plugin.Name = element.Attribute("Name")?.Value;
                    plugin.Author = element.Element("Author")?.Value;
                    plugin.Discription = element.Element("Description")?.Value;
                    plugin.ExecuteAction = ParseExecute(element);
                    plugin.StartPriority = PluginLoader;
                    Register(plugin);
                }
            }
            LaunchPriority();
        }
        /// <summary>
        /// Runs plugins which needed to make changes before loading the program.
        /// Set the value of the attribute “StartPriority” to 1 so that the plug-in
        /// starts before the program is loaded.
        /// </summary>
        private static void LaunchPriority()
        {
            if (File.Exists(Name))
            {
                object inst;
                Serialization.BinDeserialize(out inst, Name);
                List<Plugin> priority = (List<Plugin>)inst;
                foreach (var vars in priority)
                    vars.StartPriority?.Invoke("");
            }
        }
        /// <summary>
        /// Parsing an attribute value for a command. The Start Path attribute contains the namespace and class name.
        /// The method uses this string to search for the class and its Start method from which the plugin starts.
        /// </summary>
        /// <param name="element">Eelement with attrubute StartPath.</param>
        /// <returns></returns>
        private static Action<object> ParseExecute(XElement element)
        {
            string path = "ProgramManager.Plugins." + element.Attribute("StartPath")?.Value;
            string method = element.Attribute("Execute")?.Value;

            Type startPath = Type.GetType(path);

            if (method != null)
            {
                if (startPath != null)
                {
                    // Find method with name poiner to the Execute attribute.
                    MethodInfo execute = startPath.GetMethod(method);

                    if (execute != null)
                    {
                        var @delegate = (Action<object>)
                            Delegate.CreateDelegate(type: typeof(Action<object>), method: execute);

                        if (element.Attribute("StartPriority")?.Value == "1")
                            PluginLoader += @delegate;
                        return @delegate;
                    }
                    throw new PluginNotFoundException("Method named: \"" + method + "\" is not found.");
                }
                throw new PluginNotFoundException("Path: \"" + path + "\" is not found.");
            }
            // Path to plugin not found. Check the path in the configuration file:
            // 1. StartPath attribute should point to the class with method to run plugin.
            // 2. Execute attribute should point to the method that runs the plugin.
            throw new PluginNotFoundException();
        }
    }
    [Serializable]
    class Plugin
    {
        public Action<object> ExecuteAction { get; set; }
        public Action<object> StartPriority { get; set; }
        public string Name { get; set; }
        public PluginType Type { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Discription { get; set; }
    }
}
