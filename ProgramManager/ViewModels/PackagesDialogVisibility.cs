using System.Windows;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    /// <summary>
    /// Класс управления видимостью диалогового окна пакетов и его компанентами.
    /// </summary>
    public class PackagesDialogVisibility
    {
        private static PackagesDialog _packagesDialog = new PackagesDialog();

        public static void OpenPackageDialog()
        {
            lock (_packagesDialog)
            {
                // TODO Строка не удалена а просто закрыта комментарием, так как решает проблему с достпупом к дочерниму окну после закрытия.
                // Но, повторно инициальзирует конструктор что приводит к непресказуеммой работе програмы, оставил на будующие когда вернусь к этой проблме.
                _packagesDialog = new PackagesDialog();
                _packagesDialog.Visibility = Visibility.Visible;
            }        
        }
        public static void ClosePackageDialog()
        {
            lock (_packagesDialog)
            {
                //_packagesDialog.Close();
                _packagesDialog.Visibility = Visibility.Hidden;
            }           
        }
    }
}
