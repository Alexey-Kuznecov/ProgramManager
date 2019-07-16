using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views;
using System.Windows.Input;
using System.Windows.Media;
using ProgramManager.Resources;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    public class IconViewModelBase : PropertiesChanged
    {
        #region Properties

        public DrawingBrush IconGeometry { get; set; }
        public SolidColorBrush IconBackground { get; set; }
        public SolidColorBrush IconForeground { get; set; }
        public string IconName { get; set; }

        #endregion

        #region Functions

        public void LoadIcon(Icon icon)
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
    public class IconControlViewModel : IconViewModelBase
    {
        public IconControlViewModel()
        {
            CmdOpenDialogIcon = new RelayCommand(obj => OpenDialogIcon());
        }

        public ICommand CmdOpenDialogIcon { get; }

        public void OpenDialogIcon()
        {
            using (WindowDispatchers wd = new WindowDispatchers())
            {
                wd.IconsEditor = new IconsEditor();
                wd.IconsEditor.Show();
            }
        }
    }
}
