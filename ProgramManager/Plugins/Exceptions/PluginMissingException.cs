using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Plugins.Exceptions
{
    class PluginMissingException : Exception
    {
        public PluginMissingException(string message)
            : base (message) { }
        public PluginMissingException()
            : base ("Plugin is missing or disabled..") { }
    }
}
