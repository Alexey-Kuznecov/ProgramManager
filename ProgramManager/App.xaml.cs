using System.Windows;
using ProgramManager.ViewModels;
using ProgramManager.Views;
using ProgramManager.Models;
using ProgramManager.Enums;

namespace ProgramManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new Window();
            var view = new MainView();


            window = view;
            window.Show();
        }
    }
}
