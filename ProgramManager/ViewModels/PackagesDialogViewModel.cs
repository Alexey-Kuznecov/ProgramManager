using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProgramManager.Models.PackageModel;
using ProgramManager.Views.DialogPacks;
using ProgramManager.ViewModels.Base;
using ProgramManager.Resources;
using ProgramManager.Services;
using GalaSoft.MvvmLight.Messaging;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private string _description;
        private string _packageTitle;
        private static InputName _windowInputName;
        private static PackageBase _package;
        private IconControl _iconControl;

        #region Constructor

        public PackagesDialogViewModel()
        {
            // Initial fields.
            _windowInputName = new InputName();
            IconControl = new IconControl();

            // Initial data.
            InitializePackageDialog();

            // Activate commands.
            CmdRemoveTextField = new RelayCommand(RemoveTextField);

            // Registration to receive data.
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<InputName>(this, action => _windowInputName = action);
            Messenger.Default.Register<PackageBase>(this, LoadPackage);
            Messenger.Default.Register<List<string>>(this, InitialDataSource);
            Synchronizer.IconLoad = LoadSelectIcon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Контекстное меню для вкладки поля.
        /// </summary>
        public List<MenuItem> MenuItem { get; set; }
        public static ObservableCollection<TextFieldModel> TextField { get; set; }
        public string Description
        {
            get { return _description;  }
            set { SetProperty(ref _description, value, () => Description); }
        }
        public string PackageTitle
        {
            get { return _packageTitle; }
            set
            {
                SetProperty(ref _packageTitle, value, () => PackageTitle);
            }
        }
        public IconControl IconControl
        {
            get { return _iconControl; }
            set
            {
                _iconControl = value;
                SetProperty(ref _iconControl, value, () => IconControl);
            }
        }
        
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
            Synchronizer.IconLoad.Invoke(null);
        });
        #endregion

        #region Functions

        private void LoadSelectIcon(Icon obj)
        {
            if (Singleton.Back == null)
                Singleton.Back = IconControl.DataContext as IconControlViewModel;

            if (!Singleton.Status)
            {
                IconControlViewModel iconViewModel = new IconControlViewModel();
                _package.Icon.Brush = obj.Brush;
                _package.Icon.Name = obj.Name;
                _package.Icon.BgroundColor = obj.BgroundColor;
                _package.Icon.FgroundColor = obj.FgroundColor;
                iconViewModel.LoadIcon(_package.Icon);
                IconControl.DataContext = iconViewModel;
            }
            else IconControl.DataContext = Singleton.Back;
        }

        #endregion
    }
}
