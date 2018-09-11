using System.Windows;


namespace ProgramManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void CallWinCreatePack_Click(object sender, RoutedEventArgs e)
        {
            DialogPackages window = new DialogPackages();
            window.ShowDialog();
        }
    }
}
