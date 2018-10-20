using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using ProgramManager.Models;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Models.PackageModel;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{

    partial class PackagesDialogViewModel
    {
        private static List<string> _tagList;
        private static Dictionary<string, string> _dataList = new Dictionary<string, string>();
        public void InitialDataSource(object data)
        {
            if (data is List<string>)
                _tagList = data as List<string>;
        }
        /// <summary>
        /// Отправляет данные для их добавления в базу данных.
        /// Вызывает события изменения данных.
        /// </summary>
        /// <param name="data">Данные входящие в папкет.</param>
        public void SendPackage<T>(object data) where T: PackageBase, new ()
        {
            // Получаем управление диалоговым окном пакетов.
            PackagesDialog window = data as PackagesDialog;
            if (_tagList == null)
                _tagList = new List<string>() { "Не подшитые" };
                

            // Добавления полей базовго класса.
            T package = new T()
            {
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
            foreach (var property in package.GetType().GetProperties())
            {
                if (TextField.SingleOrDefault(p => p.Types == property.Name) != null && property.GetValue(package) == null)
                {
                    property.SetValue(package, TextField.Single(p => p.Types == property.Name).FieldValue);
                }
            }
            // Добавления пользовательских полей.
            foreach (var field in TextField.Select(p => p))
            {
                if (field.Types.Contains(FieldTypes.Userfield.ToString()))
                {
                    package.FieldList.Add(field.Types, field.FieldValue);
                }
            }
            BaseConnector connector = new BaseConnector();
            connector.OnPackageChanged(package);
            window?.Close();
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
    }
}
