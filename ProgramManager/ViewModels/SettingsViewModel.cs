using System;
using System.Windows.Controls;
using System.Windows.Input;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views.SettingsControl;

namespace ProgramManager.ViewModels
{
    class SettingsViewModel : PropertiesChanged
    {
        public SettingsViewModel()
        {
            SettingsView = new GeneralControl();
        }

        private UserControl _settingsView;

        public UserControl SettingsView
        {
            get { return _settingsView; }
            set
            {
                _settingsView = value;
                OnPropertyChanged("SettingsView");
            }
        }

        public ICommand GenaralSelection => new RelayCommand(obj =>
        {
            SettingsView = new GeneralControl();
        });
        public ICommand ThemesSelection => new RelayCommand(obj =>
        {
            SettingsView = new ThemeControl();
        });
    }
}
