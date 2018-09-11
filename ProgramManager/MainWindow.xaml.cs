using ProgramManager.Views;
using System.Windows;

namespace ProgramManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CallWinCreatePack_Click(object sender, RoutedEventArgs e)
        {
            CreatePackageWindow window = new CreatePackageWindow();
            window.ShowDialog();
        }
    }
}
