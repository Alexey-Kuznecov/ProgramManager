using System.Collections;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Enums;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    public partial class DialogPackagesViewModel : PropertiesChanged
    {
        public DialogPackagesViewModel()
        {
            _windowInputName = new InputName();
            RemoveTextField = new RelayCommand(obj => TextField.Remove(obj as TextFieldModel));
            SavingPack = new RelayCommand(obj => SavingPackage());
            Messenger.Default.Register<InfoMessage>(this, action => InputCustomName(action.Name));
        }
        private const string AUTOCOMPLETE_ICON = "../../Resources/Icons/Businessman_48px.png";
        private const string DELETE_ICON = "../../Resources/Icons/Delete_48px.png";
        private InputName _windowInputName;
        public List<MenuItem> MenuItem { get; set; } = new List<MenuItem>()
        {
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.Author, "Автор", }, Header = "Автор", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.Version, "Версия", }, Header = "Версия", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.License, "Лицензия", }, Header = "Лицензия", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.Copyright, "Авторские права", }, Header = "Авторские права", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.CompanySite, "Официальный сайт", }, Header = "Официальный сайт", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.SerialKey, "Лицензионный ключ", }, Header = "Лицензионный ключ", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.Source, "Источник", }, Header = "Источник", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.HashSumm, "Хеш-суммы", }, Header = "Хеш-суммы", },
            new MenuItem { Command = MenuCommand, CommandParameter = new ArrayList { TFieldType.Other, "Другое", }, Header = "Другое", }
    };
        public static ObservableCollection<TextFieldModel> TextField { get; } = new ObservableCollection<TextFieldModel>()
        {
            new TextFieldModel { Label = "Автор: ", Hint = "Имя...", Type = TFieldType.Author, AutoCompleteIcon = AUTOCOMPLETE_ICON, DeleteTextFieldIcon = DELETE_ICON},
            new TextFieldModel { Label = "Версия", Hint = "Версия...", Type = TFieldType.Version, AutoCompleteIcon = AUTOCOMPLETE_ICON, DeleteTextFieldIcon = DELETE_ICON}
        };
        public string Description { get; set; }
        public string PackageName { get; set; }
        public ICommand RemoveTextField { get; }
        public ICommand SavingPack { get; }
        public ICommand OpenInputName => new RelayCommand(obj =>
        {
            _windowInputName.ShowDialog();
        });
        public static ICommand MenuCommand => new RelayCommand(array =>
        {
            ArrayList item = array as ArrayList;
            if (item != null)
            {
                TFieldType type = (TFieldType)item[0];
                AddTextField(item[1].ToString(), type);
            }
        });
    }
    
}
