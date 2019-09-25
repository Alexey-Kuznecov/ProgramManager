
namespace ProgramManager.Data
{
    using System.Collections.Generic;
    using InteractionLib;

    /// <summary>
    /// The settings.
    /// </summary>
    public struct Settings
    {
        /// <summary>
        /// Gets or sets the current theme.
        /// </summary>
        public static Themes CurrentTheme { get; set; }

        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        public static List<PluginManager.Plugin> Plugins { get; set; }
    }
}
