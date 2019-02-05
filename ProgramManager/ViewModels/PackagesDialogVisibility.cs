using System.Windows;
using ProgramManager.Models.PackageModel;
using ProgramManager.Views;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Converters;
using ProgramManager.Enums;
using System.Linq;

namespace ProgramManager.ViewModels
{
    /// <summary>
    /// Класс управления видимостью диалогового окна пакетов и его компанентами.
    /// </summary>
    public class PackagesDialogVisibility
    {
        private static PackagesDialog _packagesDialog = new PackagesDialog();

        public static void EditPackageDialog(PackageBase currPackage)
        {
            PackagesDialog packagesDialog = new PackagesDialog();

            Messenger.Default.Send(currPackage);
            packagesDialog.SaveAndEdit.Content = "Изменить";
            packagesDialog.Title = "Редактирование пакета";
            packagesDialog.ShowDialog();
            
            //Выполнение дополнительных действий после закрытия окна редактирования
            if (!packagesDialog.IsActive)
            {
                var dictionary = FieldConverter.Dictionary;

                // Чистка словоря от пользовательских полей 
                for (int i = 0; i <= dictionary.Count; i++)
                {
                    var item = FieldConverter.Dictionary.Select(p => p.Key.Substring(0, 4) == FieldTypes.Userfield.ToString().Substring(0, 4));
                    //dictionary.Remove(item);
                }
            }          
        }
        public static void CreatePackageDialog(CategoryModel current)
        {
            // TODO Строка не удалена а просто закрыта комментарием, так как решает проблему с доступом к дочерниму окну после закрытия.
            // Но, повторно инициальзирует конструктор что приводит к непресказуеммой работе программы, оставил на будующие когда вернусь к этой проблеме.
            _packagesDialog = new PackagesDialog();
            _packagesDialog.ShowDialog();
        }
        public static void ClosePackageDialog()
        {
            _packagesDialog.Close();         
        }
    }
}
