
namespace ProgramManager
{
    using System.Windows;
    using InteractionLib;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The on startup.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // PluginManager.PluginReader();
            PluginManager.FindAssambly();
            var view = new MainView();
            Window window = view;
            window.Show();
        }
    }
}
