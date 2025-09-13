<<<<<<< HEAD
﻿
namespace ProgramManager.Services
{
    using System.Collections.ObjectModel;
    using Models;
    using Models.PackageModel;
    using ViewModels;

    /// <summary>
    /// The packages manager.
    /// </summary>
    public class PackagesManager
    {
        /// <summary>
        /// The wrapper packages.
        /// </summary>
        private static ObservableCollection<WrapPackage> _wrapperPackages;

        /// <summary>
        /// The current category of packages.
        /// </summary>
        private static string _currentCategory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackagesManager"/> class.
        /// </summary>
        public PackagesManager()
        {
            EventAggregate.NewPackage += this.AddNewPackage;
            EventAggregate.PackageChanged += this.ChangePackage;
            EventAggregate.RemovePackage += this.RemovePackage;
            EventAggregate.LoadTagList += new TagDialogViewModel().DisplayLoadTagList;
        }

        /// <summary>
        /// Gets packages for the current Category.
        /// </summary>
        /// <param name="category"> The current Category. </param>
        /// <returns> The collection of packages for the current Category. </returns>
        public static ObservableCollection<WrapPackage> GetPackages(CategoryModel category)
        {
            _currentCategory = category.Name;
            PackagesDialogViewModel.Category = category;
=======
﻿using System.Collections.ObjectModel;
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
            EventAggregate.NewPackage += AddNewPackage;
            EventAggregate.TagListUpdate += TagDialogViewModel.DisplayTagList;
            EventAggregate.PackageChanged += ChangePackage;
            EventAggregate.RemovePackage += RemovePackage;
        }

        private void RemovePackage(object sender, BaseEventArgs e)
        {
            PackageAccess.RemovePackage((int)e.Package);
        }
        private void ChangePackage(object sender, BaseEventArgs e)
        {
            PackageBase package = e.Package as PackageBase;
            PackageAccess.UpdatePackage(package);
        }
        private void AddNewPackage(object sender, BaseEventArgs e)
        {
            PackageBase package = e.Package as PackageBase;
            PackageAccess.AddPackage(package, _categoryStatus);
        }
        public static ObservableCollection<WrapPackage> GetPackages(CategoryModel category)
        {
            _categoryStatus = category.Name;
            PackagesDialogViewModel._category = category;
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11

            if (category.PackageType is ProgramModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<ProgramModel>.GetPackages(category)));
            }
<<<<<<< HEAD

=======
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            if (category.PackageType is DriverModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<DriverModel>.GetPackages(category)));
            }
<<<<<<< HEAD

=======
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            if (category.PackageType is ModModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<ModModel>.GetPackages(category)));
            }
<<<<<<< HEAD

=======
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            if (category.PackageType is GameModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<GameModel>.GetPackages(category)));
            }
<<<<<<< HEAD

=======
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            if (category.PackageType is PluginModel)
            {
                _wrapperPackages = new ObservableCollection<WrapPackage>(
                    WrapPackage.WrapPackageTag(PackagesReader<PluginModel>.GetPackages(category)));
            }
<<<<<<< HEAD

            return _wrapperPackages;
        }

        /// <summary>
        /// Gets packages for the default Category.
        /// </summary>
        /// <returns>
        /// The collection of packages for the default category.
        /// </returns>
        public static ObservableCollection<WrapPackage> GetPackages()
        {
            _currentCategory = CategoryModel.Categories[0].Name;
            _wrapperPackages = new ObservableCollection<WrapPackage>(
                list: WrapPackage.WrapPackageTag(PackagesReader<ProgramModel>.GetPackages(CategoryModel.Categories[0])));

            return _wrapperPackages;
        }

        /// <summary>
        /// The method to remove package.
        /// </summary>
        /// <param name="sender"> The Sender. </param>
        /// <param name="e"> Expected the package to remove. </param>
        private void RemovePackage(object sender, BaseEventArgs e)
        {
            PackagesWriter.RemovePackage((int)e.Package);
        }

        /// <summary>
        /// The method to change package.
        /// </summary>
        /// <param name="sender"> The Sender. </param>
        /// <param name="e"> Expected the package to change. </param>
        private void ChangePackage(object sender, BaseEventArgs e)
        {
            PackageBase package = e.Package as PackageBase;
            PackagesWriter.UpdatePackage(package);
        }

        /// <summary>
        /// The method to add new package.
        /// </summary>
        /// <param name="sender"> The Sender. </param>
        /// <param name="e"> Expected the package to add. </param>
        private void AddNewPackage(object sender, BaseEventArgs e)
        {
            PackageBase package = e.Package as PackageBase;
            PackagesWriter.AddPackage(package, _currentCategory);
        }
=======
            return _wrapperPackages;
        }
        public static ObservableCollection<WrapPackage> GetPackages()
        {
            _categoryStatus = CategoryModel.Categories[0].Name;
            _wrapperPackages = new ObservableCollection<WrapPackage>(
                list: WrapPackage.WrapPackageTag(PackagesReader<ProgramModel>.GetPackages(CategoryModel.Categories[0])));
            return _wrapperPackages;
        }

>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    }
}
