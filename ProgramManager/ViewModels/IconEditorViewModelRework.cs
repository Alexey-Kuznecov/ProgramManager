using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModelRework : PropertiesChanged
    {
        public IconEditorViewModelRework()
        {
            List<IconControl> listIconControl = new List<IconControl> ();
            IconControl iconControl = new IconControl ();
            IconViewModel iconView = iconControl.DataContext as IconViewModel;

        }

        private List<IconControl> _iconControl;

        public List<IconControl> IconControl
        {
            get { return _iconControl; }
            set
            {
                _iconControl = value;
                SetProperty(ref _iconControl, value, () => IconControl);
            }
        }
        /// <summary>
        /// Создает контейнер для иконок, метод нужнен для 
        /// отображения иконок по горизонтали 
        /// </summary>
        public void WrapperIcons()
        {
            
        }
    }
}
