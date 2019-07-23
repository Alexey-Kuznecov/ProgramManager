using System.Windows;
using ProgramManager.Views;

namespace ProgramManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var view = new MainView();
            Window window = view;
            window.Show();
        }
    }
}
