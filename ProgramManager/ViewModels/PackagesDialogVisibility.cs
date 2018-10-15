using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    public class PackagesDialogVisibility
    {
        private static PackagesDialog _packagesDialog = new PackagesDialog();

        public static void OpenPackageDialog()
        {
            lock (_packagesDialog)
            {
                _packagesDialog = new PackagesDialog();
                _packagesDialog.ShowDialog();
            }        
        }
        public static void ClosePackageDialog()
        {
            lock (_packagesDialog)
            {
                _packagesDialog.Close();
            }           
        }
    }
}
