using System;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ProgramManager.ViewModels
{
    public class DialogPackagesViewModel : PropertiesChanged
    {
        public DialogPackagesViewModel()
        {
            AddTextField = new RelayCommand(selected => AddNewTextField((MenuItemModel) selected));
            RemoveTextField = new RelayCommand(obj => TextField.Remove(obj as TextFieldModel));
            SavingPack = new RelayCommand(obj => SavingPackage());
            InputNameField = new RelayCommand(obj =>
            {
                InputName inputName = new InputName();
                inputName.ShowDialog();
            });
        }
        private const string AUTOCOMPLETE_ICON = "../../Resources/Icons/Businessman_48px.png";
        private const string DELETE_ICON = "../../Resources/Icons/Delete_48px.png";

        public ObservableCollection<TextFieldModel> TextField { get; } = new ObservableCollection<TextFieldModel>()
        {
            new TextFieldModel { LabelField = "Автор: ", HintField = "Имя...", AutoCompleteIcon = AUTOCOMPLETE_ICON, DeleteTextFieldIcon = DELETE_ICON},
            new TextFieldModel { LabelField = "Версия", HintField = "Версия...", AutoCompleteIcon = AUTOCOMPLETE_ICON, DeleteTextFieldIcon = DELETE_ICON}
        };
        public List<MenuItemModel> MenuItem { get; set; } = new List<MenuItemModel>()
        {
            new MenuItemModel { Name = "Автор", Id = 0},
            new MenuItemModel { Name = "Версия", Id = 1},
            new MenuItemModel { Name = "Лицензия", Id = 2},
            new MenuItemModel { Name = "Авторские права", Id = 3},
            new MenuItemModel { Name = "Официальный сайт", Id = 4},
            new MenuItemModel { Name = "Лицензионный ключ", Id = 5},
            new MenuItemModel { Name = "Источник", Id = 6},
            new MenuItemModel { Name = "Хеш-суммы", Id = 7}
        };
        public string Description { get; set; }

        private void AddNewTextField(MenuItemModel selectedItem)
        {
            if (selectedItem == null) throw new ArgumentNullException(nameof(selectedItem));

            TextField.Add(new TextFieldModel()
            {
                LabelField = selectedItem.Name,
                HintField = selectedItem.Name,
                AutoCompleteIcon = AUTOCOMPLETE_ICON,
                DeleteTextFieldIcon = DELETE_ICON
            });
        }
        public PackageModel SavingPackage()
        {
            Debug.Assert(condition: TextField != null, message: nameof(TextField) + " != null");

            PackageModel newPack = new PackageModel {Description = Description};

            for (var index = 0; index < TextField.Count; index++)
            {
                var fieldModel = TextField[index];

                if (MenuItem[index].Id == 0)
                    newPack.Author = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 1)
                    newPack.Version = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 2)
                    newPack.Lincense = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 3)
                    newPack.Copyright = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 4)
                    newPack.CompanySite = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 5)
                    newPack.SerialKey = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 6)
                    newPack.Source = fieldModel.FieldValue;
                else if (MenuItem[index].Id == 7)
                    newPack.HashSumm = fieldModel.FieldValue;
            }

            return newPack;
        }
        public ICommand InputNameField { get; }
        public ICommand AddTextField { get; }
        public ICommand RemoveTextField { get; }
        public ICommand SavingPack { get; }
    }
    
}
