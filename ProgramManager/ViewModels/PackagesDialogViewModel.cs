using System;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models.PackageModels;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private static InputName _windowInputName;
        private static TagDialog _windowTagModify;
        #region Constructor

        public PackagesDialogViewModel()
        {
            _windowInputName = new InputName();
            _windowTagModify = new TagDialog();
            RemoveTextField = new RelayCommand(obj => TextField.Remove(obj as TextFieldModel));
            SavePackage = new RelayCommand(obj => SendPackage<ProgramModel>(obj));
            Messenger.Default.Register<InputNameViewModel>(this, action => InputCustomName(action.Name));
            Messenger.Default.Register<List<string>>(this, InitialDataSource);
            Description = "This is Description";
            PackageTitle = "This is PackageTitle";
        }

        #endregion


        #region Properties

        /// <summary>
        /// Создание контекстного меню вкладки поля.
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
        public static ObservableCollection<TextFieldModel> TextField { get; } = new ObservableCollection<TextFieldModel>()
        {
            new TextFieldModel { FieldValue = "This is Author", Types = "Author", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "This is Version", Types = "Version", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "This is Source", Types = "Source", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "This is HashSumm", Types = "HashSumm", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "This is CompanySite", Types = "CompanySite", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "This is License", Types = "License", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "Userfield1", Types = "Userfield1", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "Userfield2", Types = "Userfield2", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "Userfield3", Types = "Userfield3", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { FieldValue = "Userfield4", Types = "Userfield4", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
        };
        public string Description { get; set; }
        public string PackageTitle { get; set; }

        #endregion

        #region Commands

        public ICommand RemoveTextField { get; }
        public ICommand SavePackage { get; }
        public ICommand OpenInputName => new RelayCommand(obj => _windowInputName.ShowDialog());
        public ICommand OpenTagDialog => new RelayCommand(obj => { _windowTagModify.Show(); });
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
