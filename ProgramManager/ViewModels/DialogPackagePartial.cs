using System;
using System.Reflection;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Contracts;
using ProgramManager.Enums;
using ProgramManager.Models;
using ProgramManager.Views;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.ViewModels
{
    partial class DialogPackagesViewModel
    {
        private void SavingPackage()
        {
            /** Метод формирует новый объект на основе данных введенных пользователем в соответстующие поля ввода
            /*  Метод денамический получает имена свойств объекта PackageModel для его сровнения со значением
            /*  свойства TextFieldModel.Type (это перечисление содержит типы полей, наример: Author).*/
            PackageModel newPack = new PackageModel
            {
                Description = Description,
                Name = PackageName
            };
            PropertyInfo[] properties = newPack.GetType().GetProperties();

            for (int i = 0; i < TextField.Count; i++)
            {
                foreach (PropertyInfo property in properties)
                {
                    // Сравнивает значение с именем свойства
                    // TextField.Type <— Author == PackageModel.Author
                    if (TextField[i].Type.ToString() == property.Name)
                    {
                        property.SetValue(newPack, TextField[i].FieldValue);
                        break;
                    }
                }
            }
            BaseConnector connector = new BaseConnector();
            connector.OnPackageChanged(newPack);
            PackagesDialog.ClosePackageDialog();
        }
        private static void AddTextField(string name, TFieldType type)
        {
            TextField.Add(new TextFieldModel()
            {
                Label = name + ":",
                Hint = name + "...",
                Type = type,
                AutoCompleteIcon = AUTOCOMPLETE_ICON,
                DeleteTextFieldIcon = DELETE_ICON,
            });
        }
        private void InputCustomName(string name)
        {
            TextField.Add(new TextFieldModel()
            {
                Label = name + ":",
                Hint = name + "...",
                AutoCompleteIcon = AUTOCOMPLETE_ICON,
                DeleteTextFieldIcon = DELETE_ICON,
            });
            _windowInputName.Close();
        }
    }
}
