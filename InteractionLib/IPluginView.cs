
namespace InteractionLib
{
    using System.Windows.Controls;

    /// <summary>
    /// The plugin view.
    /// </summary>
    public interface IPluginView
    {
        /// <summary>
        /// The get view.
        /// </summary>
        /// <returns> The <see cref="UserControl"/>. </returns>
        UserControl GetView();
    }
}
