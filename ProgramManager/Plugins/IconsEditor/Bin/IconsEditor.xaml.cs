using System.Windows;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    /// <summary>
    /// Логика взаимодействия для IconsEditor.xaml
    /// </summary>
    public partial class IconsEditor : Window
    {
        public IconsEditor()
        {
            InitializeComponent();
        }
        private void grid_ContextMenuClosing(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            MessageBox.Show("grid_ContextMenuClosing");
        }

        private void d_DragEnter_1(object sender, DragEventArgs e)
        {
            MessageBox.Show("d_DragEnter_1");
        }

        private void d_DragLeave(object sender, DragEventArgs e)
        {
            MessageBox.Show("d_DragLeave");
        }

        private void d_DragOver(object sender, DragEventArgs e)
        {
            MessageBox.Show("d_DragOver");
        }

        private void d_Drop_1(object sender, DragEventArgs e)
        {
            MessageBox.Show("d_Drop_1");
        }

        private void d_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("d_MouseDown");
        }

        private void d_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MessageBox.Show("d_MouseEnter");
        }
    }
}
