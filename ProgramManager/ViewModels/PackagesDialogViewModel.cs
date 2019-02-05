using System.Collections.Generic;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models.PackageModel;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Services;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private static InputName _windowInputName;
        private static TagDialog _windowTagModify;
        private string _description;
        private string _packageTitle;

        #region Constructor

        public PackagesDialogViewModel()
        {
            // Initial fields.
            _windowInputName = new InputName();
            _windowTagModify = new TagDialog();

            // Initial data.
            InitializePackageDialog();

            // Activate commands.
            CmdRemoveTextField = new RelayCommand(obj => RemoveTextField(obj));

            // Registration to receive data.
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<InputName>(this, action => _windowInputName = action as InputName);
            Messenger.Default.Register<PackageBase>(this, action => LoadPackage(action));
            Messenger.Default.Register<List<string>>(this, InitialDataSource);
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

        #endregion

    }
    
}
