
namespace ProgramManager.Services
{
    using System.Linq;
    using System.Windows;
    using Converters;
    using Enums;
    using GalaSoft.MvvmLight.Messaging;
    using Models.PackageModel;
    using Plugins.Exceptions;
    using Views;

    /// <summary>
    /// The visibility control class for the package dialog box and its components.
    /// </summary>
    public class PackagesDialogVisibility
    {
        /// <summary>
        /// The _packages dialog.
        /// </summary>
        private static PackagesDialog _packagesDialog = new PackagesDialog();

        /// <summary>
        /// The edit package dialog.
        /// </summary>
        /// <param name="currentPackage"> The current package. </param>
        public static void EditPackageDialog(PackageBase currentPackage)
        {
            PackagesDialog packagesDialog = new PackagesDialog();
            try
            {
                Messenger.Default.Send(currentPackage);
                packagesDialog.SaveAndEdit.Content = "Изменить";
                packagesDialog.Title = "Редактирование пакета";
                packagesDialog.ShowDialog();
            }
            catch (PluginMissingException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (PluginTypeDoubleDefinedException e)
            {
                MessageBox.Show(e.Message);
            }

            // Performing additional actions after closing the edit window
            if (!packagesDialog.IsActive)
            {
                var dictionary = PackageFieldConverter.Dictionary;

                // Clearing the dictionary from custom fields
                for (int i = 0; i <= dictionary.Count; i++)
                {
                    var item = PackageFieldConverter.Dictionary.Select(p => p.Key.Substring(0, 4) == FieldTypes.Userfield.ToString().Substring(0, 4));
                    // dictionary.Remove(item);
                }
            }          
        }

        /// <summary>
        /// The create package dialog.
        /// </summary>
        /// <param name="current"> The current package. </param>
        public static void CreatePackageDialog(CategoryModel current)
        {
            // TODO The line is not deleted but simply closed by a comment, as it solves the problem of accessing the child window after closing.
            // But, the constructor re-initializes, which leads to unpredictable work of the program, left for future when I return to this problem.
            _packagesDialog = new PackagesDialog();
            _packagesDialog.ShowDialog();
        }

        /// <summary>
        /// The close package dialog.
        /// </summary>
        public static void ClosePackageDialog()
        {
            _packagesDialog.Close();         
        }
    }
}
