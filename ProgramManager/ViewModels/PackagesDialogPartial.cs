using System.Linq;
using System.Windows;
using ProgramManager.Models;
using ProgramManager.Converters;
using ProgramManager.Enums;

namespace ProgramManager.ViewModels
{
    partial class PackagesDialogViewModel
    {
        /// <summary>
        /// Отпровляет данные для их добавления в базу данных.
        /// Вызывает события изменения данных.
        /// </summary>
        public void SendPackage()
        {
            // Инициальзация данных
            var title = new { Name = "Title", Value = PackageTitle };
            var descripion = new { Name = "Description", Value = Description };
            var fields = TextField.Select(s => new { Name = s.Types, Value = s.FieldValue, }).ToList();

            fields.Add(title);
            fields.Add(descripion);

            BaseConnector connector = new BaseConnector();
            connector.OnPackageChanged(fields);
            PackagesDialogVisibility.ClosePackageDialog();
        }
        /// <summary>
        /// Метод добавления нового поля.
        /// </summary>
        /// <param name="type">Принимает тип поля</param>
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
        /// Метод для добавления пользовательского поля.
        /// </summary>
        /// <param name="fieldName">Принимает имя поля</param>
        private void InputCustomName(string fieldName)
        {
            if (fieldName != null)
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
        }
    }
}
