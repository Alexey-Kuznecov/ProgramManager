

using System;

namespace ProgramManager.ViewModels
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using Base;
    using Views.SettingsControl;

    /// <summary>
    /// The settings view model.
    /// </summary>
    public class SettingsViewModel : PropertiesChanged
    {
        /// <summary>
        /// The settings view.
        /// </summary>
        private UserControl _settingsView;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            this.SettingsView = new GeneralControl();
        }

        /// <summary>
        /// Gets or sets the settings view.
        /// </summary>
        public UserControl SettingsView
        {
            get => this._settingsView;
            set
            {
                this._settingsView = value;
                this.OnPropertyChanged("SettingsView");
            }
        }

        /// <summary>
        /// The general settings.
        /// </summary>
        public ICommand GeneralSettings => new RelayCommand(obj =>
        {
            SettingsView = new GeneralControl();
        });

        /// <summary>
        /// The themes settings.
        /// </summary>
        public ICommand ThemesSettings => new RelayCommand(obj =>
        {
            SettingsView = new ThemeControl();
        });

        /// <summary>
        /// The plugin settings.
        /// </summary>
        public ICommand PluginSettings => new RelayCommand(obj =>
        {
            SettingsView = new PluginControl();
        });
    }
}
