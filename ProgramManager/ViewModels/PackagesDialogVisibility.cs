using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    public static class PackagesDialogVisibility
    {
        public static readonly PackagesDialog PackagesDialog = new PackagesDialog();

        public static void OpenPackageDialog()
        {
            PackagesDialog.Visibility = Visibility.Visible;
            PackagesDialog.ShowDialog();
        }
        public static void ClosePackageDialog()
        {
            PackagesDialog.Visibility = Visibility.Hidden;
        }
    }
}
