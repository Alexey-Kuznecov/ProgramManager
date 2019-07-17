using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ProgramManager.Models.PackageModel;
using ProgramManager.Views.DialogPacks;
using ProgramManager.ViewModels.Base;
using ProgramManager.Resources;
using ProgramManager.Services;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private string _description;
        private string _packageTitle;
        private static InputName _windowInputName;
        private DrawingBrush _iconBrush;
        private SolidColorBrush _iconForeground;
        private SolidColorBrush _iconBackground;

        #region Constructor

        public PackagesDialogViewModel()
        {
            // Initial fields.
            _windowInputName = new InputName();

            // Initial data.
            InitializePackageDialog();

            // Activate commands.
            CmdRemoveTextField = new RelayCommand(RemoveTextField);
            CmdOpenDialogIcon = new RelayCommand(obj => OpenDialogIcon());

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
        public DrawingBrush IconBrush
        {
            get { return _iconBrush; }
            set
            {
                _iconBrush = value;
                OnPropertyChanged("IconBrush");
            }
        }
        public SolidColorBrush IconBackground
        {
            get { return _iconBackground; }
            set
            {
                _iconBackground = value;
                OnPropertyChanged("IconBackground");
            }
        }
        public SolidColorBrush IconForeground
        {
            get { return _iconForeground; }
            set
            {
                _iconForeground = value;
                OnPropertyChanged("IconForeground");
            }
        }
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
            Synchronizer.IconLoad.Invoke(null);
        });
        public ICommand CmdOpenDialogIcon { get; }
        #endregion

        #region Functions

        private void LoadSelectIcon(IconModel icon)
            {
                if (Singleton.Back == null)
                    Singleton.Back = new IconModel(Name, IconBrush, IconForeground, IconBackground);

                if (!Singleton.Status)
                {
                    Name = icon.Name;
                    IconBrush = icon.Brush;
                    IconBackground = icon.BgroundColor;
                    IconForeground = icon.FgroundColor;
                }
                else
                {
                    IconModel iconBack = (IconModel)Singleton.Back;
                    Name = iconBack.Name;
                    IconBrush = iconBack.Brush;
                    IconBackground = iconBack.BgroundColor;
                    IconForeground = iconBack.FgroundColor;
                }
            }
        public void OpenDialogIcon()
            {
                using (WindowDispatchers wd = new WindowDispatchers())
                {
                    wd.IconsEditor = new IconsEditor();
                    wd.IconsEditor.Show();
                }
            }

        #endregion
    }
}
