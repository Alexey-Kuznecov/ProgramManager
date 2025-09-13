using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgramManager.Resources
{
    class IconResources
    {
        public DrawingBrush Icon { get; set; }
        public Brush Color { get; set; }

        public Grid DrawIcon()
        {
            //ResourceDictionary resource = new ResourceDictionary();
            //resource.Source = new Uri("../../Resources/Icons.xaml", UriKind.Relative);
            //DynamicResourceExtension dynamicResource = new DynamicResourceExtension();

            Grid grid = new Grid();
            Button button = new Button();
            button.Width = 48;
            button.Height = 48;

            button.Background = Application.Current.FindResource("SettingIcon") as Brush;
            grid.Children.Add(button);

            return grid;
        }
    }
}
