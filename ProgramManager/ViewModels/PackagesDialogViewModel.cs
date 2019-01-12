using System;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models.PackageModel;

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
            RemoveTextField = new RelayCommand(obj => TextField.Remove(obj as TextFieldModel));
            SavePackage = new RelayCommand(obj => SendPackage<ProgramModel>(obj));

            // Registration to receive data.
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<PackageBase>(this, action => LoadPackage(action));
            Messenger.Default.Register<List<string>>(this, InitialDataSource);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Контекстное меню для вкладки поля.
        /// </summary>
        public List<MenuItem> MenuItem { get; set; } = new List<MenuItem>()
        {
            new MenuItem { Command = MenuCommand, CommandParameter = "Author", Header = "Автор", },
            new MenuItem { Command = MenuCommand, CommandParameter = "Version", Header = "Версия", },
            new MenuItem { Command = MenuCommand, CommandParameter = "License", Header = "Лицензия", },
            new MenuItem { Command = MenuCommand, CommandParameter = "Copyright", Header = "Авторские права", },
            new MenuItem { Command = MenuCommand, CommandParameter = "CompanySite", Header = "Официальный сайт", },
            new MenuItem { Command = MenuCommand, CommandParameter = "SerialKey", Header = "Лицензионный ключ", },
            new MenuItem { Command = MenuCommand, CommandParameter = "Source", Header = "Источник", },
            new MenuItem { Command = MenuCommand, CommandParameter = "HashSumm", Header = "Хеш-суммы", },
            new MenuItem { Command = MenuCommand, CommandParameter = "Other", Header = "Другое", }
        };
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

        public ICommand RemoveTextField { get; }
        public ICommand SavePackage { get; }
        public ICommand OpenInputName => new RelayCommand(obj => _windowInputName.ShowDialog());
        public ICommand OpenTagDialog => new RelayCommand(obj => { _windowTagModify.ShowDialog(); });
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
