
namespace InteractionLib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Plugins.Exceptions;

    /// <summary>
    /// The plugin manager.
    /// </summary>
    /// [DebuggerStepThrough]
    public class PluginManager
    {
        /// <summary>
        /// The filename for serialization.
        /// </summary>
        private const string Name = "DataPlugins.bin";
        
        /// <summary>
        /// The start priority.
        /// </summary>
        private static Action<object> _startPriority;

        /// <summary>
        /// List plugin registered.
        /// </summary>
        private static List<Plugin> _dataPlugins = new List<Plugin>();

        /// <summary>
        /// Registers plugin and serialize it. If IconEditor type is already registered
        /// the rest type will be ignored. Only the first plugin of the same type will be registered.
        /// </summary>
        /// <param name="inst"> An <c>object</c> data of the new plugin. </param>
        public static void Register(Plugin inst)
        {
            if (_dataPlugins.All(p => p.Type != inst.Type))
            {
                _dataPlugins.Add(inst);
            }
            else
            {
                throw new PluginTypeDoubleDefinedException();
            }

            Serialization.BinSerialize(_dataPlugins, Name);
        }

        /// <summary>
        /// It executes a plugin that has been registered,
        /// plug-ins can be of only one type, 
        /// </summary>
        /// <param name="type"> Type of plugin to use.</param>
        /// <returns> An <c>object</c> data of the plugin. </returns>
        public static Plugin Execute(PluginType type)
        {
            Serialization.BinDeserialize(out object inst, Name);
            Plugin plugin = ((List<Plugin>)inst).SingleOrDefault(p => p.Type == type);

            return plugin ?? throw new PluginMissingException("Plugin " + type + " is missing or disabled..");
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
                
                foreach (var element in plugins)
                {
                    Plugin plugin = new Plugin();
                    Enum.TryParse(element.Attribute("Type")?.Value, true, out PluginType type);
                    plugin.Type = type;
                    plugin.Name = element.Attribute("Name")?.Value;
                    plugin.Author = element.Element("Author")?.Value;
                    plugin.Description = element.Element("Description")?.Value;
                    plugin.ExecuteAction = ParseExecute(element);
                    plugin.StartPriority = _startPriority;
                    Register(plugin);
                }
            }

            LaunchPriority();
        }

        /// <summary>
        /// Runs plug ins which needed to make changes before loading the program.
        /// Set the value of the attribute “StartPriority” to 1 so that the plug-in
        /// starts before the program is loaded.
        /// </summary>
        private static void LaunchPriority()
        {
            if (File.Exists(Name))
            {
                Serialization.BinDeserialize(out object inst, Name);
                List<Plugin> priority = (List<Plugin>)inst;
                foreach (var vars in priority)
                    vars.StartPriority?.Invoke("dd");
            }
        }
        
        /// <summary>
        /// Parsing an attribute value for a command. The StartPath attribute contains the <c>namespace</c> and class name.
        /// The method uses <c>this</c> string to search for the class and its Start method from which the plugin starts.
        /// </summary>
        /// <param name="element"> Element with attribute StartPath.</param>
        /// <returns> The method that start plugin. </returns>
        private static Action<object> ParseExecute(XElement element)
        {
            string path = element.Attribute("StartPath")?.Value;
            string method = element.Attribute("Execute")?.Value;
            string assambly = element.Attribute("Assambly")?.Value;

            Assembly asm = Assembly.LoadFrom("Plugins/" + assambly);

            Type startPath = asm.GetType(path ?? throw new InvalidOperationException(), true, true);

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

                        string priority = element.Attribute("StartPriority")?.Value;

                        if (priority != null)
                        {
                            var priorityMethod = startPath.GetMethod(priority);

                            if (priorityMethod != null)
                            {
                                var delegatePriority = (Action<object>)
                                    Delegate.CreateDelegate(type: typeof(Action<object>), method: priorityMethod);
                                _startPriority += delegatePriority;
                            }
                            else
                            {
                                throw new PluginNotFoundException("Method named: \"" + priority + "\" is not found.");
                            }
                        }

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

        /// <summary>
        /// The plugin data.
        /// </summary>
        [Serializable]
        public class Plugin
        {
            /// <summary>
            /// Gets or sets the execute action.
            /// </summary>
            public Action<object> ExecuteAction { get; set; }

            /// <summary>
            /// Gets or sets the start priority.
            /// </summary>
            public Action<object> StartPriority { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            public PluginType Type { get; set; }

            /// <summary>
            /// Gets or sets the author.
            /// </summary>
            public string Author { get; set; }

            /// <summary>
            /// Gets or sets the version.
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            public string Description { get; set; }
        }
    }
}
