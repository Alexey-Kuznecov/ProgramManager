using System;
using System.Collections;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using ProgramManager.Models.NewModel;

namespace ProgramManager.ViewModels
{
    public class NewMainModel : PropertiesChanged
    {
        public NewMainModel()
        {
            Packages = BaseModel.GetPackages();

            Categories = CategoryModel.Categories;

            if (Packages.Count > 0)
            {
                CurrentPackage = Packages[0];
            }
        }
        private ObservableCollection<PackageBase> _packages;
        private PackageBase _currentPackage;
        private CategoryModel _currentCategory;
        private TagModel _currentTag;
        private List<CategoryModel> _categories;

        public ObservableCollection<PackageBase> Packages
        {
            get { return _packages; }
            set
            {              
                SetProperty(ref _packages, value, () => Packages);
            }
        }
        public List<CategoryModel> Categories
        {
            get { return _categories; }
            set
            {
                SetProperty(ref _categories, value, () => Categories);
            }
        }
        public PackageBase CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                SetProperty(ref _currentPackage, value, () => CurrentPackage);

                if (value != null)
                {
                    _packages[0].Datails = _currentPackage.Properties;
                    OnPropertyChanged("Packages");
                }       
            }
        }
        public CategoryModel CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                SetProperty(ref _currentCategory, value, () => _currentCategory);

                if (_currentCategory != null)
                    Packages = BaseModel.GetPackages(_currentCategory);

                if (Packages.Count > 0)
                    CurrentPackage = Packages[0];
            }
        }
        public ICommand OpenDialogPackage => new RelayCommand(obj =>
        {
            PackagesDialogVisibility.OpenPackageDialog();
        });

        public TagModel CurrentTag
        {
            get { return _currentTag; }
            set
            {
                if (_currentTag != null)
                {
                    Packages = BaseModel.GetPackages(_currentCategory);
                    IEnumerable<PackageBase> query = Packages.Where(pack => pack.TagContain == _currentTag.Name);
                    Packages = new ObservableCollection<PackageBase>(query);
                }

                _currentTag = value;
            }
        }
    }
}
