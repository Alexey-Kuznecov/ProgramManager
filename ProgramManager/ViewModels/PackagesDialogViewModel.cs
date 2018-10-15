using System.Collections;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Enums;

namespace ProgramManager.ViewModels
{
    public partial class PackagesDialogViewModel : PropertiesChanged
    {
        private const string AutocompleteIcon = "../../Resources/Icons/Businessman_48px.png";
        private const string DeleteIcon = "../../Resources/Icons/Delete_48px.png";
        private static InputName _windowInputName;

        public PackagesDialogViewModel()
        {
            _windowInputName = new InputName();
            RemoveTextField = new RelayCommand(obj => TextField.Remove(obj as TextFieldModel));
            SavePackage = new RelayCommand(obj => SendPackage());
            Messenger.Default.Register<InfoMessage>(this, action => InputCustomName(action.Name));
        }
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
            new TextFieldModel { Types = "Author", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon},
            new TextFieldModel { Types = "Version", AutoCompleteIcon = AutocompleteIcon, DeleteTextFieldIcon = DeleteIcon}
        };
        public string Description { get; set; }
        public string PackageTitle { get; set; }
        public ICommand RemoveTextField { get; }
        public ICommand SavePackage { get; }
        public ICommand OpenInputName => new RelayCommand(obj => { _windowInputName.ShowDialog(); });
        /// <summary>
        /// Контекстное меню, команды для юоваления полей.
        /// </summary>
        public static ICommand MenuCommand => new RelayCommand(type =>
        {
            if (type != null)
                AddTextField((string)type);
        });
        /// <summary>
        /// Отпровляет данные для их добавления в базу данных.
        /// </summary>
        public void SendPackage()
        {
            TextField.Add(new TextFieldModel { Types = "Title", FieldValue = PackageTitle });

            var anonymous = TextField.Select(field => new
            {
                name = field.Types,
                value = field.FieldValue,
            }).ToList();

            BaseConnector connector = new BaseConnector();
            connector.OnPackageChanged(anonymous);
            PackagesDialogVisibility.ClosePackageDialog();
        }
    }
    
}
