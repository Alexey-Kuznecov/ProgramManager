<<<<<<< HEAD
﻿
namespace ProgramManager.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using AlexLibWpf.Models;
    using Base;
    using GalaSoft.MvvmLight.Messaging;
    using InteractionLib;
    using Models.PackageModel;
    using Views;
    using Views.DialogPacks;

    /// <summary>
    /// The packages dialog view model.
    /// </summary>
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        /// <summary>
        /// The autocomplete icon.
        /// </summary>
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";

        /// <summary>
        /// The delete icon.
        /// </summary>
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";

        /// <summary>
        /// The _window input name.
        /// </summary>
        private static InputName _windowInputName;

        /// <summary>
        /// The _icon model back.
        /// </summary>
        private IconModel _iconModelBack;

        /// <summary>
        /// The _tag dialog.
        /// </summary>
        private TagDialog _tagDialog;

        /// <summary>
        /// The _description.
        /// </summary>
        private string _description;

        /// <summary>
        /// The _package title.
        /// </summary>
        private string _packageTitle;

        /// <summary>
        /// The _icon foreground.
        /// </summary>
        private SolidColorBrush _iconForeground;

        /// <summary>
        /// The _icon background.
        /// </summary>
        private SolidColorBrush _iconBackground;

        /// <summary>
        /// The _icon path.
        /// </summary>
        private Path _iconPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackagesDialogViewModel"/> class.
        /// </summary>
=======
﻿using System.Collections.Generic;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models.PackageModel;
using ProgramManager.Views;
using System.Windows;
using System.Windows.Media;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private static InputName _windowInputName;
        private static IconViewModel _iconViewModel;
        private static DialogIcons _windowAddIcons;
        private string _description;
        private string _packageTitle;

        #region Constructor

>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        public PackagesDialogViewModel()
        {
            // Initial fields.
            _windowInputName = new InputName();
<<<<<<< HEAD

            // Initial data.
            this.InitializePackageDialog();

            // Activate commands.
            this.CommandRemoveTextField = new RelayCommand(RemoveTextField);

            // Registration to receive data.
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<InputName>(this, action => _windowInputName = action);
            Messenger.Default.Register<PackageBase>(this, this.LoadPackage);
            DataSync.TagLoad = this.InitialTagLs;
            DataSync.IconLoad = this.LoadSelectIcon;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the collection fields that user add in the package.
        /// </summary>
        public static ObservableCollection<TextFieldModel> TextField { get; set; }

        /// <summary>
        /// Gets or sets the context menu for the field tab.
        /// </summary>
        public List<MenuItem> MenuItem { get; set; }

        /// <summary>
        /// Gets or sets an description of current package..
        /// </summary>
        public string Description
        {
            get => this._description;
            set { this.SetProperty(ref this._description, value, () => this.Description); }
        }

        /// <summary>
        /// Gets or sets name of current package.
        /// </summary>
        public string PackageTitle
        {
            get => this._packageTitle;
            set
            {
                this.SetProperty(ref this._packageTitle, value, () => this.PackageTitle);
            }
        }

        /// <summary>
        /// Gets or sets an icon background of package.
        /// </summary>
        public SolidColorBrush IconBackground
        {
            get => this._iconBackground;
            set
            {
                this._iconBackground = value;
                this.OnPropertyChanged("IconBackground");
            }
        }

        /// <summary>
        /// Gets or sets an icon foreground of package.
        /// </summary>
        public SolidColorBrush IconForeground
        {
            get => this._iconForeground;
            set
            {
                this._iconForeground = value;
                this.OnPropertyChanged("IconForeground");
            }
        }

        /// <summary>
        /// Gets or sets an icon geometry of package.
        /// </summary>
        public Path IconPath
        {
            get => this._iconPath;
            set
            {
                this._iconPath = value;
                this.OnPropertyChanged("IconPath");
            }
        }

        /// <summary>
        /// Gets or sets icon name that to be used to add to the database. Package remembers its icon name 
        /// that to be displayed then next loading package. 
        /// </summary>
        public string IconName { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to cancel change.
        /// </summary>
        public ICommand CancelChange => new RelayCommand(obj =>
        {
            DataSync.IconLoad.Invoke(null);
        });

        /// <summary>
        /// Gets or sets the command to save package.
        /// </summary>
        public ICommand SavePackage { get; set; }

        /// <summary>
        /// The context menu commands for adding fields.
        /// </summary>
        public ICommand MenuCommand => new RelayCommand(type =>
        {
            if (type != null)
            {
                AddTextField((string)type);
            }
        });

        /// <summary>
        /// Gets the command to remove text field.
        /// </summary>
        public ICommand CommandRemoveTextField { get; }

        /// <summary>
        /// The command to open input name.
        /// </summary>
=======
            _windowAddIcons = new DialogIcons();
            _iconViewModel= new IconViewModel();

            // Initial data.
            InitializePackageDialog();

            // Activate commands.
            CmdRemoveTextField = new RelayCommand(obj => RemoveTextField(obj));

            // Registration to receive data.
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<InputName>(this, action => _windowInputName = action as InputName);
            Messenger.Default.Register<PackageBase>(this, action => LoadPackage(action));
            Messenger.Default.Register<List<string>>(this, InitialDataSource);
            Messenger.Default.Register<IconEditorViewModel>(this, LoadIcon);
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
        public ICommand OpenDialogIcons => new RelayCommand(obj => 
        {
            _windowAddIcons = new DialogIcons();
            _windowAddIcons.ShowDialog();
        });
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        public ICommand OpenInputName => new RelayCommand(obj => 
        {
            InputName windowInputName = new InputName();
            windowInputName.ShowDialog();
        });
<<<<<<< HEAD

        /// <summary>
        /// The command to open tag dialog.
        /// </summary>
        public ICommand OpenTagDialog => new RelayCommand(obj =>
        {
            _tagDialog = new TagDialog();
            _tagDialog.Show();
            
            //Plugin plugin = PluginManager.Execute(PluginType.TagEditor);
            //plugin.ExecuteAction(this);
        });

        /// <summary>
        /// The command to open dialog icon.
        /// </summary>
        public ICommand OpenDialogIcon => new RelayCommand(obj =>
        {
            Process.Start(@"..\..\..\IconMaker\bin\Debug\IconMaker.exe");
            
            //Plugin plugin = PluginManager.Execute(PluginType.IconEditor);
            //plugin.ExecuteAction(this);
        });
        
        #endregion

        /// <summary>
        /// Function sets icon on button that open the icon editor. 
        /// </summary>
        /// <param name="icon">Custom icon selected in the icon editor.</param>
        private void LoadSelectIcon(IconModel icon)
        {
            // Save button source state.
            if (this._iconModelBack == null)
            {
                this._iconModelBack = new IconModel(this.IconName, this.IconPath, this.IconForeground, this.IconBackground);
            }
            
            // Sets flag to null to restore button source state. If user remove been set icon.
            if (icon != null)
            {
                this.IconName = icon.Name;
                this.IconPath = icon.Path;
                this.IconBackground = icon.BgroundColor;
                this.IconForeground = icon.FgroundColor;       
                
                //ImageCover = new ImageCover(icon);
                //ImageCover?.Save(".png", "../../Resources/User/Images/");
            }
            else
            {
                IconModel iconBack = this._iconModelBack;
                this.IconName = iconBack.Name;
                this.IconPath = iconBack.Path;
                this.IconBackground = iconBack.BgroundColor;
                this.IconForeground = iconBack.FgroundColor;
                this._iconModelBack = null;
            }
        }
    }
=======
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

        private void LoadIcon(IconEditorViewModel obj)
        {
            //IconBrush = obj.IconBrush;
        }

    }
    
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
}
