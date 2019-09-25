
namespace ProgramManager.ViewModels.Settings
{
    using System.Collections.Generic;
    using Base;
    using InteractionLib;

    /// <summary>
    /// The plugin view model.
    /// </summary>
    public class PluginViewModel : PropertiesChanged
    {
        /// <summary>
        /// The plugin views.
        /// </summary>
        public List<IPluginSettings> PluginViews;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginViewModel"/> class.
        /// </summary>
        public PluginViewModel()
        {
            
        }
    }
}
