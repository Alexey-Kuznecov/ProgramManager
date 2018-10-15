using System.Linq;
using System.Windows;
using ProgramManager.Models;
using ProgramManager.Converters;

namespace ProgramManager.ViewModels
{
    partial class PackagesDialogViewModel
    {
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
            else _windowInputName.ShowDialog();
        }
        private void InputCustomName(string fieldName)
        {
            if (fieldName != null)
            {
                var formatKey = "UserField" + (TextField.Count + 1);

                FieldConverter.Descriptions.Add(formatKey, fieldName);

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
