using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgramManager.ViewModels
{
    public class IconViewModel : PropertiesChanged
    {
        public IconViewModel()
        {
            Messenger.Default.Register<object>(this, LoadIcon);
            CmdSelectDialogIcon = new RelayCommand(obj => SelectDialogIcon(obj));
        }

        private DrawingBrush _iconButton;

        public DrawingBrush IconButton
        {
            get { return _iconButton; }
            set
            {
                _iconButton = value;
                SetProperty(ref _iconButton, value, () => IconButton);
            }
        }

        public ICommand CmdSelectDialogIcon { get; }

        private void LoadIcon(object obj)
        {
            DrawingBrush brush = obj as DrawingBrush;
            DrawingGroup group = brush.Drawing as DrawingGroup;
            DrawingBrush newBrush = new DrawingBrush();
            DrawingGroup newGroup = new DrawingGroup();
            SolidColorBrush color = new SolidColorBrush(Color.FromRgb(149, 130, 255));

            foreach (var item in group.Children)
            {
                GeometryDrawing geometry = item as GeometryDrawing;
                newGroup.Children.Add(new GeometryDrawing() { Brush = color, Geometry = geometry.Geometry });
                newBrush.Drawing = newGroup;
            };                       
            IconButton = newBrush;
        }
        private void SelectDialogIcon(object obj)
        {
            DialogIcons instance = new DialogIcons();
            instance.Show();
        }
    }
}
