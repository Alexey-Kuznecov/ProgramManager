using System.Collections.ObjectModel;
using ProgramManager.Models.PackageDerives;
using ProgramManager.ViewModels;

namespace ProgramManager.Models
{
    public class BaseModel
    {
        private static ObservableCollection<WrapPackage> _wrapperPackages;

        public void AddNewPackage(object sender, ConnectorEventArgs e)
        {
            PackageMutator.AddPackage(e.Package);
        }
        public static ObservableCollection<WrapPackage> GetPackages(CategoryModel category)
        {
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
