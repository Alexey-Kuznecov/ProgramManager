
namespace InteractionLib
{
    using System.Windows.Controls;

    /// <summary>
    /// The plugin settings.
    /// </summary>
    public interface IPluginSettings
    {
        /// <summary>
        /// The get settings view.
        /// </summary>
        /// <returns> The <see cref="UserControl"/>. </returns>
        UserControl GetSettingsView();
    }
}
