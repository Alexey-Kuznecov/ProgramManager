using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    public static class PackagesDialog
    {
        public static readonly DialogPackages _dialogPackages = new DialogPackages();

        public static void OpenPackageDialog()
        {
            _dialogPackages.Visibility = Visibility.Visible;
        }
        public static void ClosePackageDialog()
        {
            _dialogPackages.Visibility = Visibility.Hidden;
        }
    }
}
