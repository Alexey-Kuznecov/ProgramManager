using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Models;
using ProgramManager.Models.PackageModel;
using ProgramManager.Plugins;
using ProgramManager.Plugins.IconsEditor.Data;
using ProgramManager.Services;
using ProgramManager.Views;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    partial class PackagesDialogViewModel
    {
        private static List<string> _tagList;
        private int _id;
        public static CategoryModel Category;

        #region INITIALIZE DATA AND COMPONENTS
        public void InitialTagLs(object data)
        {
            var list = data as List<string>;
            if (list != null)
                _tagList = list;
        }
        /// <summary>
        /// Инициализация окна пакетов значениями по умолчанию.
        /// </summary>
        public void InitializePackageDialog()
        {
            TextField = new ObservableCollection<TextFieldModel>()
            {
                new TextFieldModel
                {
                    FieldValue = "This is Author", Types = "Author", AutoCompleteIcon = AutocompleteIcon,
                    DeleteTextFieldIcon = DeleteIcon
                },
                new TextFieldModel
                {
                    FieldValue = "This is Version", Types = "Version", AutoCompleteIcon = AutocompleteIcon,
                    DeleteTextFieldIcon = DeleteIcon
                }
            };
            SetCategory();
        }
        /// <summary>
        /// Устанавливает категорию для целевого пакета.
        /// </summary>
        public void SetCategory()
        {
            // Determining the type of package to send
            if (Category?.PackageType is PluginModel)
                SavePackage = new RelayCommand(SendPackage<PluginModel>);
            else if (Category?.PackageType is DriverModel)
                SavePackage = new RelayCommand(SendPackage<DriverModel>);
            else if (Category?.PackageType is GameModel)
                SavePackage = new RelayCommand(SendPackage<GameModel>);
            else if (Category?.PackageType is ModModel)
                SavePackage = new RelayCommand(SendPackage<ModModel>);
            else
            {
                Category = new CategoryModel();
                SavePackage = new RelayCommand(SendPackage<ProgramModel>);
            }
            SetContextMenuItem();
            LoadIcon(
                new IconModel(InteractonPackageEditor.IconDefault, 
                InteractonPackageEditor.IconForeDefault, InteractonPackageEditor.IconBackDefault));
        }
        /// <summary>
        /// Метод добавляет элементы в контекстное меню диалогового
        /// окна пакетов в зависимости от текущей категории
        /// </summary>
        public void SetContextMenuItem()
        {
            MenuItem = new List<MenuItem>();
            Category.SetMenuItem();

            foreach (var item in Category.MenuItem)
                MenuItem.Add(new MenuItem { Command = MenuCommand, CommandParameter = item.Key, Header = item.Value });
        }
        #endregion

        #region SAVING AND ADDING PACKAGES
        /// <summary>
        /// Отправляет данные для их добавления в базу данных.
        /// Вызывает события изменения данных.
        /// </summary>
        /// <param name="data">Данные входящие в папкет.</param>
        public void SendPackage<T>(object data) where T : PackageBase, new()
        {
            if (_tagList == null)
                _tagList = new List<string> { "Не подшитые" };

            // Получаем управление диалоговым окном пакетов.
            PackagesDialog window = data as PackagesDialog;
            EventAggregate connector = new EventAggregate();

            // Добавления полей базовго класса.
            var package = new T()
            {
                Id = _id,
                Name = PackageTitle,
                Author = TextField.SingleOrDefault(a => a.Types == FieldTypes.Author.ToString())?.FieldValue,
                HashSumm = TextField.SingleOrDefault(a => a.Types == FieldTypes.HashSumm.ToString())?.FieldValue,
                Source = TextField.SingleOrDefault(a => a.Types == FieldTypes.Source.ToString())?.FieldValue,
                Version = TextField.SingleOrDefault(a => a.Types == FieldTypes.Version.ToString())?.FieldValue,
                Image = TextField.SingleOrDefault(a => a.Types == FieldTypes.Image.ToString())?.FieldValue,
                Icon = new IconModel(Name, IconPath, IconForeground, IconBackground),
                TagList = _tagList,
                Description = Description,
            };
            // Добавления полей производных классов.
            AddUniqueField(package);
            // Добавления пользовательских полей.
            AddCustomField(package);

            // Todo: Убрать эти костыли немедленно!!!
            if (window?.Title == "Редактирование пакета")
                connector.OnPackageChanged(package);
            else
                connector.OnNewPackage(package);

            window?.Close();
        }
        /// <summary>
        /// Данный метод загружает данные выбранного пакета для редактирования
        /// </summary>
        /// <param name="package">Данные пакета выбранного в списке пакетов в основном окне.</param>
        public void LoadPackage(PackageBase package)
        {
            // Заполнение полей диалогового окна пакетов
            _id = package.Id;
            Description = package.Description;
            PackageTitle = package.Name;
            TextField.Clear();

            foreach (var textField in package.TextField)
            {
                // Добавления полей данного пакета
                TextField.Add(new TextFieldModel
                {
                    FieldValue = textField.FieldValue,
                    AutoCompleteIcon = "../../Resources/Icons/Businessman_48px.png",
                    DeleteTextFieldIcon = "../../Resources/Icons/Delete_48px.png",
                    Types = textField.Types
                });
                // Добавления данных полей в словарь ассоциаций 
                if (!PackageFieldConverter.Dictionary.ContainsKey(textField.Types))
                    PackageFieldConverter.Dictionary.Add(textField.Types, textField.Label);
            }
            //Посылает найденный ресурс иконки для пакета
            LoadIcon(package.Icon);
            DataSync.PackageLoad.Invoke(package);
        }
        #endregion

        #region PREPARATION OF THE PACKAGE TO THE SEND
        /// <summary>
        /// Метод заполняет уникальные свойства, которые объявлены в производных классах.
        /// </summary>
        /// <typeparam name="T">Тип производнного класса например ProgramModel.</typeparam>
        /// <param name="package">Объект с уникальными свойствами.</param>
        private void AddUniqueField<T>(T package)
        {
            foreach (var property in package.GetType().GetProperties())
            {
                if (TextField.SingleOrDefault(p => p.Types == property.Name) != null && property.GetValue(package) == null)
                {
                    property.SetValue(package, TextField.Single(p => p.Types == property.Name).FieldValue);
                }
            }
        }
        /// <summary>
        /// Метод заполняет свойство FieldList из абстрактного класса PackageBase — полями созданными пользователем.
        /// </summary>
        /// <typeparam name="T">Тип производнного класса например "ProgramModel".</typeparam>
        /// <param name="package">Объект с свойством FieldList</param>
        private void AddCustomField<T>(T package) where T : PackageBase
        {
            foreach (var field in TextField.Select(p => p))
            {
                if (field.Types.Contains(FieldTypes.Userfield.ToString()))
                {
                    package.FieldList.Add(field.Types, field.FieldValue);
                }
            }
        }
        #endregion

        #region FUNCTIONS FOR FIELD MANAGEMENT
        /// <summary>
        /// Метод для добавления нового поля.
        /// </summary>
        /// <param name="type">Принимает тип поля.</param>
        private static void AddTextField(string type)
        {
            var query = TextField.SingleOrDefault(s => s.Types == type);

            if (query == null)
            {
                TextField.Add(new TextFieldModel()
                {
                    Types = type,
                    AutoCompleteIcon = AutocompleteIcon,
                    DeleteTextFieldIcon = DeleteIcon,
                });
            }
            else
                _windowInputName.ShowDialog();
        }
        /// <summary>
        /// Так как месседжер вызывает этот метод дважды пришлось ввести это поля.
        /// В принципе вполне удачное рашение, так как не позволяет добавить поля с
        /// одинаковыми именами. Но месседжер, все равно хорошо было бы починить.
        /// </summary>
        private static string _name;
        /// <summary>
        /// Метод для добавления пользовательского поля.
        /// </summary>
        /// <param name="fieldName">Принимает имя поля</param>
        private static void InputCustomName(string fieldName)
        {
            if (fieldName != null && fieldName != _name)
            {
                var formatKey = FieldTypes.Userfield.ToString() + (TextField.Count + 1);

                PackageFieldConverter.Dictionary.Add(formatKey, fieldName);
                TextField.Add(new TextFieldModel
                {
                    FieldValue = fieldName,
                    Types = formatKey,
                    AutoCompleteIcon = AutocompleteIcon,
                    DeleteTextFieldIcon = DeleteIcon,
                });
                _windowInputName.Visibility = Visibility.Hidden;
            }
            _name = fieldName;
        }
        /// <summary>
        /// Метод удаляет из коллекции TextField поля а также очищает словарь.
        /// </summary>
        /// <param name="obj">Ожидается элемент коллекции который нужно удалить.</param>
        public void RemoveTextField(object obj)
        {
            TextFieldModel field = obj as TextFieldModel;

            // Removing the user field association from the dictionary
            if (field != null && field.Types.Contains(FieldTypes.Userfield.ToString()))
                PackageFieldConverter.Dictionary.Remove(field.Types);

            TextField.Remove(field);
        }
        #endregion

        #region FUNCTIONS FOR ICON MANAGEMENT
        /// <summary>
        /// Загружает иконку текущего пакета на редактирование.
        /// </summary>
        /// <param name="icon"></param>
        public void LoadIcon(IconModel icon)
        {
            IconPath = icon.Path;
            IconBackground = icon.BgroundColor;
            IconForeground = icon.FgroundColor;
        }
        #endregion
    }
}
