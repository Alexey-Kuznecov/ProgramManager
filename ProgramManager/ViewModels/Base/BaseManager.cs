using System.Collections.ObjectModel;
using ProgramManager.Models;
using ProgramManager.Models.PackageModels;

namespace ProgramManager.ViewModels.Base
{
    public class BaseManager
    {
        private static ObservableCollection<WrapPackage> _wrapperPackages;
        private static string _categoryStatus;
        public BaseManager()
        {
            BaseConnector.PackageChanged += AddNewPackage;
            BaseConnector.TagListUpdate += TagDialogViewModel.DisplayTagList;
        }
        public void AddNewPackage(object sender, ConnectorEventArgs e)
        {
            PackageMutator.AddPackage(e.Package, _categoryStatus);
        }
        public static ObservableCollection<WrapPackage> GetPackages(CategoryModel category)
        {
            _categoryStatus = category.Name;

            if (category.PackageType is ProgramModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackageAccess<ProgramModel>.GetPackages(category)));
            }
            if (category.PackageType is DriverModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackageAccess<DriverModel>.GetPackages(category)));
            }
            if (category.PackageType is ModModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackageAccess<ModModel>.GetPackages(category)));
            }
            if (category.PackageType is GameModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackageAccess<GameModel>.GetPackages(category)));
            }
            if (category.PackageType is PluginModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackageAccess<PluginModel>.GetPackages(category)));
            }
            return _wrapperPackages;
        }
        public static ObservableCollection<WrapPackage> GetPackages()
        {
            _wrapperPackages = new ObservableCollection<WrapPackage>(
                list: WrapPackage.WrapPackageTag(PackageAccess<ProgramModel>.GetPackages(CategoryModel.Categories[0])));
            return _wrapperPackages;
        }

    }
}
