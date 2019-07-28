using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using ProgramManager.Models.PackageModel;
using ProgramManager.Views.DialogPacks;
using ProgramManager.ViewModels.Base;
using ProgramManager.Services;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models;
using ProgramManager.Plugins;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private string _description;
        private string _packageTitle;
        private static InputName _windowInputName;
        private SolidColorBrush _iconForeground;
        private SolidColorBrush _iconBackground;
        private Path _iconPath;

        #region Constructor
        public PackagesDialogViewModel()
        {
            // Initial fields.
            _windowInputName = new InputName();

            // Initial data.
            InitializePackageDialog();

            // Activate commands.
            CmdRemoveTextField = new RelayCommand(RemoveTextField);

            // Registration to receive data.
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<InputName>(this, action => _windowInputName = action);
            Messenger.Default.Register<PackageBase>(this, LoadPackage);
            Messenger.Default.Register<List<string>>(this, InitialDataSource);
            DataSync.IconLoad = LoadSelectIcon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Контекстное меню для вкладки поля.
        /// </summary>
        public List<MenuItem> MenuItem { get; set; }
        /// <summary>
        /// Collection fields that user add in the package.
        /// </summary>
        public static ObservableCollection<TextFieldModel> TextField { get; set; }
        /// <summary>
        /// Set an discription of current package.
        /// Get description to add it in db.
        /// </summary>
        public string Description
        {
            get { return _description;  }
            set { SetProperty(ref _description, value, () => Description); }
        }
        /// <summary>
        /// Set an name of current package.
        /// </summary>
        public string PackageTitle
        {
            get { return _packageTitle; }
            set
            {
                SetProperty(ref _packageTitle, value, () => PackageTitle);
            }
        }
        /// <summary>
        /// Contain an icon background, 
        /// that can be set in the icon editor.
        /// </summary>
        public SolidColorBrush IconBackground
        {
            get { return _iconBackground; }
            set
            {
                _iconBackground = value;
                OnPropertyChanged("IconBackground");
            }
        }
        /// <summary>
        /// Contain an icon color.
        /// that can be set in the icon editor.
        /// </summary>
        public SolidColorBrush IconForeground
        {
            get { return _iconForeground; }
            set
            {
                _iconForeground = value;
                OnPropertyChanged("IconForeground");
            }
        }
        /// <summary>
        /// Contain an icon geometry path that displayed in the View.
        /// Property can be set from the icon editor.
        /// </summary>
        public Path IconPath
        {
            get { return _iconPath; }
            set
            {
                _iconPath = value;
                OnPropertyChanged("IconPath");
            }
        }
        /// <summary>
        /// Icon name that to be used to add to the database. Package remembers its icon name 
        /// that to be displayed then next loading package. 
        /// </summary>
        public string Name  { get; set; }

        #endregion

        #region Commands

        public ICommand CmdRemoveTextField { get; }
        public static ICommand SavePackage { get; set; }
        public ICommand OpenInputName => new RelayCommand(obj => 
        {
            InputName windowInputName = new InputName();
            windowInputName.ShowDialog();
        });
        public ICommand OpenTagDialog => new RelayCommand(obj => 
        {
            TagDialog windowTagModify = new TagDialog();
            windowTagModify.ShowDialog();
        });
        /// <summary>
        /// Контекстное меню, команды для добавления полей.
        /// </summary>
        public static ICommand MenuCommand => new RelayCommand(type =>
        {
            if (type != null)
                AddTextField((string)type);
        });
        public static ICommand CancelChange => new RelayCommand(obj =>
        {
            Singleton.Status = true;
            DataSync.IconLoad.Invoke(null);
        });
        public ICommand CmdOpenDialogIcon => new RelayCommand(obj =>
        {
            Plugin plugin = PluginManager.Execute(PluginType.IconEditor);
            plugin.ExecuteAction(this);
        });
        #endregion

        #region Functions
        /// <summary>
        /// Function sets icon on button that open the icon editor. 
        /// <see cref="IconsEditorViewModel.SelectIconCommand">Command to add icon.</see>
        /// </summary>
        /// <param name="icon">Custom icon selected in the icon editor.</param>
        private void LoadSelectIcon(IconModel icon)
        {
            // Save button source state.
            if (Singleton.Back == null)
                Singleton.Back = new IconModel(Name, IconPath, IconForeground, IconBackground);
            
            // Sets flag to null to restore button source state. If user remove been set icon.
            if (icon != null)
            {
                Name = icon.Name;
                IconPath = icon.Path;
                IconBackground = icon.BgroundColor;
                IconForeground = icon.FgroundColor;
            }
            else
            {
                IconModel iconBack = (IconModel)Singleton.Back;
                Name = iconBack.Name;
                IconPath = iconBack.Path;
                IconBackground = iconBack.BgroundColor;
                IconForeground = iconBack.FgroundColor;
            }
        }
        #endregion
    }
}
