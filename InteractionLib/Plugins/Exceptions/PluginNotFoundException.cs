using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionLib.Plugins.Exceptions
{
    class PluginNotFoundException : Exception
    {
        public PluginNotFoundException(string message)
            : base (message) { }
        public PluginNotFoundException()
        : base ("Path to plugin not found. Check the path in the configuration file, " +
                "the StartPath attribute should point to the class and the Execute attribute should point to the method that runs the plugin.") { }
    }
}
