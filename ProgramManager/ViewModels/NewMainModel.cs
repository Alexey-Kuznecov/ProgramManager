using System;
using System.Collections;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            WrapPackage = BaseModel.GetPackages();
            Categories = CategoryModel.Categories;
        }
        private PackageBase _currentPackage;
        private CategoryModel _currentCategory;
        private List<CategoryModel> _categories;
        private ObservableCollection<WrapPackages> _wrapPackage;
        private int _indexTag;
        private int _indexPackage;

        public ObservableCollection<WrapPackages> WrapPackage
        {
            get { return _wrapPackage; }
            set
            {
                SetProperty(ref _wrapPackage, value, () => WrapPackage);
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
        public int IndexPackage
        {
            set
            {
                _indexPackage = value;

                if (_indexTag > -1)
                {
                    CurrentPackage = _wrapPackage[_indexTag].Packages[_indexPackage];
                }
            }
        }
        public int IndexTag
        {
            set
            {
                _indexTag = value;

                if (_indexTag > -1)
                {
                    CurrentPackage = _wrapPackage[_indexTag].Packages[0];
                }
            }
        }
        public PackageBase CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                SetProperty(ref _currentPackage, value, () => CurrentPackage);
            }
        }
        public CategoryModel CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                SetProperty(ref _currentCategory, value, () => _currentCategory);

                if (_currentCategory != null)
                    WrapPackage = BaseModel.GetPackages();
            }
        }
        public ICommand OpenDialogPackage => new RelayCommand(obj =>
        {
            PackagesDialogVisibility.OpenPackageDialog();
        });

        public ICommand TestCommand => new RelayCommand(obj =>
        {
            MessageBox.Show(CurrentPackage.GetHashCode().ToString());
        });
    }
}
