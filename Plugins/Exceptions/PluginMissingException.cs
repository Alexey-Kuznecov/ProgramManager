

namespace ProgramManager.Plugins.Exceptions
{
    using System;

    /// <summary>
    /// The plugin is missing.
    /// </summary>
    class PluginMissingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMissingException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public PluginMissingException(string message)
            : base (message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMissingException"/> class.
        /// </summary>
        public PluginMissingException()
            : base ("Plugin is missing or disabled..") { }
    }
}
