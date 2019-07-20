using System.Windows;
using ProgramManager.Services;

namespace ProgramManager.Views
{
    /// <summary>
    /// Логика взаимодействия для IconsEditor.xaml
    /// </summary>
    public partial class IconsEditor : Window
    {
        public IconsEditor()
        {
            InitializeComponent();
            Singleton.Back = this;
        }
    }
}
