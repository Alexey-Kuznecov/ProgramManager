<<<<<<< HEAD
﻿
namespace ProgramManager.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Base;
    using Converters;
    using Enums;
    using InteractionLib;
    using Models;
    using Models.PackageModel;
    using Plugins;
    using Services;
    using Views;

    /// <summary>
    /// The packages dialog view model.
    /// </summary>
    public partial class PackagesDialogViewModel
    {
        /// <summary>
        /// Since the messenger calls this method twice, I had to enter this field.
        /// In principle, a completely successful solution, since it does not allow adding fields with the same name.
        /// But the messenger, it would still be nice to fix it.
        /// </summary>
        private static string _name;

        /// <summary>
        /// The _tag list.
        /// </summary>
        private static List<string> _tagList;

        /// <summary>
        /// The package Identificator.
        /// </summary>
        // ReSharper disable once StyleCop.SA1650
        private int _id;
        
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public static CategoryModel Category { get; set; }

        #region INITIALIZE DATA OF PACKAGE AND COMPONENTS

        /// <summary>
        /// The initial tag list.
        /// </summary>
        /// <param name="tagList"> The data. </param>
        public void InitialTagLs(object tagList)
        {
            if (tagList is List<string> list)
            {
                _tagList = list;
            }
        }

        /// <summary>
        /// Initializing the package window with default values.
=======
﻿using System;
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
using System.Windows.Media;
using ProgramManager.Resources;
using ProgramManager.Views.DialogPacks;

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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
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

<<<<<<< HEAD
            this.SetCategory();
        }

        /// <summary>
        /// Sets the category for the target package.
=======
            SetCategory();
        }
        /// <summary>
        /// Устанавливает категорию для целевого пакета.
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        /// </summary>
        public void SetCategory()
        {
            // Determining the type of package to send
<<<<<<< HEAD
            if (Category?.PackageType is PluginModel)
            {
                SavePackage = new RelayCommand(this.SendPackage<PluginModel>);
            }
            else if (Category?.PackageType is DriverModel)
            {
                SavePackage = new RelayCommand(this.SendPackage<DriverModel>);
            }
            else if (Category?.PackageType is GameModel)
            {
                SavePackage = new RelayCommand(this.SendPackage<GameModel>);
            }
            else if (Category?.PackageType is ModModel)
            {
                SavePackage = new RelayCommand(this.SendPackage<ModModel>);
            }
            else
            {
                Category = new CategoryModel();
                SavePackage = new RelayCommand(this.SendPackage<ProgramModel>);
            }

            this.SetContextMenuItem();
            this.LoadIcon(new IconModel(InteractonPackageEditor.IconDefault, InteractonPackageEditor.IconForeDefault, InteractonPackageEditor.IconBackDefault));
        }

        /// <summary>
        /// The method adds elements to the context menu of the dialog
        /// package windows depending on the current category.
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        /// </summary>
        public void SetContextMenuItem()
        {
            MenuItem = new List<MenuItem>();
<<<<<<< HEAD
            Category.SetMenuItem();

            foreach (var item in Category.MenuItem)
            {
                MenuItem.Add(new MenuItem { Command = MenuCommand, CommandParameter = item.Key, Header = item.Value });
            }
        }

        #endregion

        #region SAVING AND ADDING PACKAGES

        /// <summary>
        /// Sends data to be added to the database. Raises data change events.
        /// </summary>
        /// <param name="data"> Data included in the package. </param>
        /// <typeparam name="T"> Derived objects from the PackageBase class. </typeparam>
        public void SendPackage<T>(object data) where T : PackageBase, new()
        {
            if (_tagList == null)
            {
                _tagList = new List<string> { "Не подшитые" };
            }

            // Adding base class fields ...
            var package = new T
            {
                Id = this._id,
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                Name = PackageTitle,
                Author = TextField.SingleOrDefault(a => a.Types == FieldTypes.Author.ToString())?.FieldValue,
                HashSumm = TextField.SingleOrDefault(a => a.Types == FieldTypes.HashSumm.ToString())?.FieldValue,
                Source = TextField.SingleOrDefault(a => a.Types == FieldTypes.Source.ToString())?.FieldValue,
                Version = TextField.SingleOrDefault(a => a.Types == FieldTypes.Version.ToString())?.FieldValue,
<<<<<<< HEAD
                Icon = new IconModel(IconName, IconPath, IconForeground, IconBackground),
                TagList = _tagList,
                Description = Description,
            };

            // Adding fields to derived classes.
            this.AddUniqueField(package);

            // Adding custom fields.
            this.AddCustomField(package);

            var window = data as PackagesDialog;
            var connector = new EventAggregate();

            // Todo:Remove these crutches immediately!!!
            if (window?.Title == "Редактирование пакета")
            {
                connector.OnPackageChanged(package);
            }
            else
            {
                connector.OnNewPackage(package);
            }

            window?.Close();
        }

        /// <summary>
        /// This method loads the selected Package for editing.
        /// </summary>
        /// <param name="package">Package data selected in the Package list.</param>
        public void LoadPackage(PackageBase package)
        {
            // Fill in the fields of the package dialog box.
            this._id = package.Id;
            Description = package.Description;
            PackageTitle = package.Name;
            TextField.Clear();

            foreach (var textField in package.TextField)
            {
                // Adding fields to this package.
                TextField.Add(new TextFieldModel
                {
                    FieldValue = textField.FieldValue,
                    AutoCompleteIcon = "../../Resources/Icons/Businessman_48px.png",
                    DeleteTextFieldIcon = "../../Resources/Icons/Delete_48px.png",
                    Types = textField.Types
                });

                // Adding field data to the association dictionary.
                if (!PackageFieldConverter.Dictionary.ContainsKey(textField.Types))
                {
                    PackageFieldConverter.Dictionary.Add(textField.Types, textField.Label);
                }
            }

            // Sends the found icon resource for a package.
            this.LoadIcon(package.Icon);
            DataSync.PackageLoad.Invoke(package);
        }
        
        #endregion

        #region FUNCTIONS FOR FIELD MANAGEMENT

        /// <summary>
        /// Method for adding a new field.
        /// </summary>
        /// <param name="type"> The field Type. </param>
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
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
<<<<<<< HEAD
            {
                _windowInputName.ShowDialog();
            }
        }

        /// <summary>
        /// Method for adding a custom field.
        /// </summary>
        /// <param name="fieldName"> The field Type. </param>
        private static void InputCustomName(string fieldName)
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        {
            if (fieldName != null && fieldName != _name)
            {
                var formatKey = FieldTypes.Userfield.ToString() + (TextField.Count + 1);

<<<<<<< HEAD
                PackageFieldConverter.Dictionary.Add(formatKey, fieldName);
                TextField.Add(new TextFieldModel
=======
                FieldConverter.Dictionary.Add(formatKey, fieldName);
                TextField.Add(new TextFieldModel()
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                {
                    FieldValue = fieldName,
                    Types = formatKey,
                    AutoCompleteIcon = AutocompleteIcon,
                    DeleteTextFieldIcon = DeleteIcon,
                });
                _windowInputName.Visibility = Visibility.Hidden;
            }
<<<<<<< HEAD

            _name = fieldName;
        }

        /// <summary>
        /// The method removes fields from the TextField collection and also clears the dictionary.
        /// </summary>
        /// <param name="obj"> The collection item to be deleted is expected. </param>
        private void RemoveTextField(object obj)
=======
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
            
            //Посылает найденый ресурс иконки для пакета
            Messenger.Default.Send(Application.Current.FindResource("Photoshop"));

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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        {
            TextFieldModel field = obj as TextFieldModel;

            // Removing the user field association from the dictionary
<<<<<<< HEAD
            if (field != null && field.Types.Contains(FieldTypes.Userfield.ToString()))
            {
                PackageFieldConverter.Dictionary.Remove(field.Types);
            }

            TextField.Remove(field);
        }

        #endregion

        #region PREPARATION OF THE PACKAGE TO THE SEND

        /// <summary>
        /// The method fills in unique properties that are declared in derived classes.
        /// </summary>
        /// <typeparam name="T">The type of the derived class, for example, ProgramModel.</typeparam>
        /// <param name="package">An objects with unique properties.</param>
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
        /// The method populates the FieldList property from the Abstract PackageBase class - fields created by the user.
        /// </summary>
        /// <typeparam name="T">A type of derived class, for example, ProgramModel. </typeparam>
        /// <param name="package"> The FieldList property is expected. </param>
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

        /// <summary>
        /// Loads the add Icon.
        /// </summary>
        /// <param name="icon"> The add Icon. </param>
        private void LoadIcon(IconModel icon)
        {
            IconPath = icon.Path;
            IconBackground = icon.BackgroundColor;
            IconForeground = icon.ForegroundColor;
        }
=======
            if (field.Types.Contains(FieldTypes.Userfield.ToString()))
                FieldConverter.Dictionary.Remove(field.Types);

            TextField.Remove(field);
        }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    }
}
