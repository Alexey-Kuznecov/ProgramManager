using System.Collections.ObjectModel;
using ProgramManager.Models;
using ProgramManager.Models.PackageModel;
using ProgramManager.ViewModels;

namespace ProgramManager.Services
{
    public class PackagesManager
    {
        private static ObservableCollection<WrapPackage> _wrapperPackages;
        private static string _categoryStatus;
        public PackagesManager()
        {
            EventAggregate.NewPackage += EventAggregate_AddNewPackage;
            EventAggregate.TagListUpdate += TagDialogViewModel.DisplayTagList;
            EventAggregate.PackageChanged += EventAggregate_ChangePackage;
        }

        private void EventAggregate_ChangePackage(object sender, BaseEventArgs e)
        {
            PackageBase package = e.Package as PackageBase;
            PackageAccess.UpdatePackage(package.Id, package);
        }
        public void EventAggregate_AddNewPackage(object sender, BaseEventArgs e)
        {
            PackageAccess.AddPackage(e.Package, _categoryStatus);
        }
        public static ObservableCollection<WrapPackage> GetPackages(CategoryModel category)
        {
            _categoryStatus = category.Name;

            if (category.PackageType is ProgramModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<ProgramModel>.GetPackages(category)));
            }
            if (category.PackageType is DriverModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<DriverModel>.GetPackages(category)));
            }
            if (category.PackageType is ModModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<ModModel>.GetPackages(category)));
            }
            if (category.PackageType is GameModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<GameModel>.GetPackages(category)));
            }
            if (category.PackageType is PluginModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<PluginModel>.GetPackages(category)));
            }
            return _wrapperPackages;
        }
        public static ObservableCollection<WrapPackage> GetPackages()
        {
            _categoryStatus = CategoryModel.Categories[0].Name;
            _wrapperPackages = new ObservableCollection<WrapPackage>(
                list: WrapPackage.WrapPackageTag(PackagesReader<ProgramModel>.GetPackages(CategoryModel.Categories[0])));
            return _wrapperPackages;
        }

    }
}
