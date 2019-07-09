using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views;
using System.Windows.Input;
using System.Windows.Media;
using ProgramManager.Resources;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    public class IconViewModel : PropertiesChanged
    {
        public IconViewModel()
        {
            Messenger.Default.Register<Icon>(this, LoadIcon);
            CmdSelectDialogIcon = new RelayCommand(SelectDialogIcon);
        }
        private DrawingBrush _iconGeometry;
        private SolidColorBrush _iconBackground;
        private SolidColorBrush _iconForeground;

        #region Properties

        public DrawingBrush IconGeometry
        {
            get { return _iconGeometry; }
            set
            {
                _iconGeometry = value;
                SetProperty(ref _iconGeometry, value, () => IconGeometry);
            }
        }
        public SolidColorBrush IconBackground
        {
            get { return _iconBackground; }
            set
            {
                _iconBackground = value;
                SetProperty(ref _iconBackground, value, () => IconBackground);
            }
        }
        public SolidColorBrush IconForeground
        {
            get { return _iconForeground; }
            set
            {
                _iconForeground = value;
                SetProperty(ref _iconForeground, value, () => IconForeground);
            }
        }
        public string IconName { get; set; }

        #endregion

        #region Commands

        public ICommand CmdSelectDialogIcon { get; }

        #endregion

        #region Functions

        private void LoadIcon(Icon icon)
        {
            DrawingBrush brush = icon.Brush;
            DrawingGroup group = brush?.Drawing as DrawingGroup;
            if (@group != null)
                foreach (var item in @group.Children)
                {
                    var geometry = item as GeometryDrawing;
                    if (geometry != null) geometry.Brush = icon.FgroundColor;
                }
            IconGeometry = brush;
            IconBackground = icon.BgroundColor;
            IconForeground = icon.FgroundColor;
            IconName = icon.Name;
        }
        private void SelectDialogIcon(object obj)
        {
            DialogIcons instance = new DialogIcons();
            instance.Show();
        }

        #endregion
    }
}
