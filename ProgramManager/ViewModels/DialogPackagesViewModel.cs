using System;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Windows.Input;
using ProgramManager.Views.DialogPacks;
using System.Collections.ObjectModel;
using ProgramManager.Contracts;

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
            new MenuItemModel { Id = 0, Name = "Автор" },
            new MenuItemModel { Id = 1, Name = "Версия" },
            new MenuItemModel { Id = 2, Name = "Лицензия" },
            new MenuItemModel { Id = 3, Name = "Авторские права" },
            new MenuItemModel { Id = 4, Name = "Официальный сайт" },
            new MenuItemModel { Id = 5, Name = "Лицензионный ключ" },
            new MenuItemModel { Id = 6, Name = "Источник" },
            new MenuItemModel { Id = 7, Name = "Хеш-суммы", }
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
        public void SavingPackage()
        {
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
            new BaseConnector().OnPackageChanged(newPack);
        }
        public ICommand InputNameField { get; }
        public ICommand AddTextField { get; }
        public ICommand RemoveTextField { get; }
        public ICommand SavingPack { get; }
    }
    
}
