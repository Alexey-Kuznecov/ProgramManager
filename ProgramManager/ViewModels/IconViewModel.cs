using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views;
using System.Windows.Input;
using System.Windows.Media;
using ProgramManager.Resources;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    public class IconViewModelBase : PropertiesChanged
    {
        public IconViewModelBase()
        {
            Messenger.Default.Register<Icon>(this, LoadIcon);
        }

        #region Properties

        public DrawingBrush IconGeometry { get; set; }
        public SolidColorBrush IconBackground { get; set; }
        public SolidColorBrush IconForeground { get; set; }
        public string IconName { get; set; }

        #endregion

        #region Functions

        protected void LoadIcon(Icon icon)
        {
            DrawingBrush brush = icon.Brush;
            DrawingGroup group = brush?.Drawing as DrawingGroup;
            if (@group != null)
            {
                foreach (var item in @group.Children)
                {
                    var geometry = item as GeometryDrawing;
                    if (geometry != null) geometry.Brush = icon.FgroundColor;
                }
            }
            IconGeometry = brush;
            IconBackground = icon.BgroundColor;
            IconForeground = icon.FgroundColor;
            IconName = icon.Name;
        }
        #endregion
    }
    public class IconViewModel : IconViewModelBase
    {
        public IconViewModel()
        {
            CmdSelectDialogIcon = new RelayCommand(obj =>
            {
                DialogIcons instance = new DialogIcons();
                instance.Show();
            });
        }
        public ICommand CmdSelectDialogIcon { get; }
    }
}
