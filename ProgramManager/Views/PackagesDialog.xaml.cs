using System.Windows;

namespace ProgramManager.Views
{
    /// <summary>
    /// Interaction logic for PackagesDialog.xaml
    /// </summary>
    public partial class PackagesDialog : Window
    {
        public PackagesDialog()
        { 
            InitializeComponent();
        }
        // Это нарушение паттерна MVVM, используется только для удобства разработки.
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application app = Application.Current;
            app.Shutdown();
        }
    }
}
