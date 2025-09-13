using System;

namespace ProgramManager.Plugins.Exceptions
{
    class PluginTypeDoubleDefinedException : Exception
    {
        public PluginTypeDoubleDefinedException(string message)
            : base(message) { }
        public PluginTypeDoubleDefinedException()
            : base("The plugin type was defined twice.") { }
    }
}
