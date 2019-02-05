using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Models.PackageModel;
using ProgramManager.Services;
using ProgramManager.Views;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace ProgramManager.ViewModels
{

    partial class PackagesDialogViewModel
    {
        private static List<string> _tagList;
        private int _id;
        private static Dictionary<string, string> _dataList = new Dictionary<string, string>();
        public static CategoryModel _category;

        public void InitialDataSource(object data)
        {
            if (data is List<string>)
                _tagList = data as List<string>;
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
            if (_category?.PackageType is PluginModel)
                SavePackage = new RelayCommand(obj => SendPackage<PluginModel>(obj));
            else if (_category?.PackageType is DriverModel)
                SavePackage = new RelayCommand(obj => SendPackage<DriverModel>(obj));
            else if (_category?.PackageType is GameModel)
                SavePackage = new RelayCommand(obj => SendPackage<GameModel>(obj));
            else if (_category?.PackageType is ModModel)
                SavePackage = new RelayCommand(obj => SendPackage<ModModel>(obj));
            else
            {
                _category = new CategoryModel();
                SavePackage = new RelayCommand(obj => SendPackage<ProgramModel>(obj));
            }

            SetContextMenuItem();
        }
        /// <summary>
        /// Метод добавляет элементы в контекстное меню диалогового
        /// окна пакетов в зависимости от текущей категории
        /// </summary>
        public void SetContextMenuItem()
        {
            MenuItem = new List<MenuItem>();
            _category.SetMenuItem();

            foreach (var item in _category.MenuItem)
                MenuItem.Add(new MenuItem { Command = MenuCommand, CommandParameter = item.Key, Header = item.Value });
        }
        /// <summary>
        /// Отправляет данные для их добавления в базу данных.
        /// Вызывает события изменения данных.
        /// </summary>
        /// <param name="data">Данные входящие в папкет.</param>
        public void SendPackage<T>(object data) where T: PackageBase, new ()
        {
            if (_tagList == null)
                _tagList = new List<string>() { "Не подшитые" };
            
            // Получаем управление диалоговым окном пакетов.
            PackagesDialog window = data as PackagesDialog;
            EventAggregate connector = new EventAggregate();

            // Добавления полей базовго класса.
            T package = new T()
            {
                Id = _id,
                Name = PackageTitle,
                Author = TextField.SingleOrDefault(a => a.Types == FieldTypes.Author.ToString())?.FieldValue,
                HashSumm = TextField.SingleOrDefault(a => a.Types == FieldTypes.HashSumm.ToString())?.FieldValue,
                Source = TextField.SingleOrDefault(a => a.Types == FieldTypes.Source.ToString())?.FieldValue,
                Version = TextField.SingleOrDefault(a => a.Types == FieldTypes.Version.ToString())?.FieldValue,
                Image = TextField.SingleOrDefault(a => a.Types == FieldTypes.Image.ToString())?.FieldValue,
                TagList = _tagList,
                Description = Description,
            };
            // Добавления полей производных классов.
            AddUniqueField(package);
            // Добавления пользовательских полей.
            AddCustomField(package);

            if (window.Title == "Редактирование пакета")
                connector.OnPackageChanged(package);
            else
                connector.OnNewPackage(package);

            window?.Close();
        }
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
        private void InputCustomName(string fieldName)
        {
            if (fieldName != null && fieldName != _name)
            {
                var formatKey = FieldTypes.Userfield.ToString() + (TextField.Count + 1);

                FieldConverter.Dictionary.Add(formatKey, fieldName);
                TextField.Add(new TextFieldModel()
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

            for (int index = 0; index < package.TextField.Count; index++)
            {
                // Добавления полей данного пакета
                TextField.Add(new TextFieldModel()
                {
                    FieldValue = package.TextField[index].FieldValue,
                    AutoCompleteIcon = "../../Resources/Icons/Businessman_48px.png",
                    DeleteTextFieldIcon = "../../Resources/Icons/Delete_48px.png",
                    Types = package.TextField[index].Types
                });      
                // Добавления данных полей в словарь ассоциаций 
                if (!FieldConverter.Dictionary.ContainsKey(package.TextField[index].Types))
                    FieldConverter.Dictionary.Add(package.TextField[index].Types, package.TextField[index].Label);
            }
        }
        /// <summary>
        /// Метод удаляет из коллекции TextField поля а также очищает словарь.
        /// </summary>
        /// <param name="obj">Ожидается элемент коллекции который нужно удалить.</param>
        public void RemoveTextField(object obj)
        {
            TextFieldModel field = obj as TextFieldModel;

            // Removing the user field association from the dictionary
            if (field.Types.Contains(FieldTypes.Userfield.ToString()))
                FieldConverter.Dictionary.Remove(field.Types);

            TextField.Remove(field);
        }
    }
}
