using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel : PropertiesChanged
    {
        #region Constuctor

        public MainViewModel()
        {
            Packages = _instancePackages;
            Category = _instanceCategories;
            CalculateByCategory();
        }
        #endregion

        #region Fields

        // The fields for properties
        private dynamic _currentPackage;
        private CategoryModel _currentTag;
        private CategoryModel _currentCategory;
        private DialogPackages _dialog = new DialogPackages();
        private ObservableCollection<PackageModel> _packages;
        private ObservableCollection<PackageModel> _storage;
        private ObservableCollection<PackageModel> _instancePackages = new BaseModel().GetPackages();
        private ObservableCollection<CategoryModel> _instanceCategories = new BaseModel().GetCategories();
        private int _indexPackage;
        private string _filterPackages;

        #endregion

        #region Properties

        /// <summary>
        /// Receives and transfers information on the current package to view
        /// </summary>
        public ObservableCollection<PackageModel> Packages
        {
            get { return _packages; }
            set { SetProperty(ref _packages, value, () => Packages); }
        }
        public ObservableCollection<CategoryModel> Tags { get; set; }
        public ObservableCollection<CategoryModel> Category { get; set; }
        public int IndexPackage
        {
            get { return _indexPackage; }
            set { SetProperty(ref _indexPackage, value, () => IndexPackage); }
        }
        /// <summary>
        /// Receives and transfers information on the current category to view 
        /// </summary>
        public dynamic CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                _currentPackage = value;
                      
                if (value != null)
                    SetCurrentPackage(value);

                OnPropertyChanged("CurrentPackage");  
            }
        }
        public CategoryModel CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                SetProperty(ref _currentCategory, value, () => CurrentCategory);
                // When the SelectedItem attribute value changes, all Tags are filtered depending on the selected category.
                // Request of these Tags
                CalculateByTag();
                // Filters Tags depending on the selected category.
                IEnumerable<CategoryModel> query = Tags.Where(category =>
                                                   category.CategoryName == _currentCategory.CategoryName);

                Tags = new ObservableCollection<CategoryModel>(query); // Creating new data
                Tags.Insert(0, new CategoryModel() { TagName = "Все", Count = CurrentCategory.Count }); // Adding special elements.        
                OnPropertyChanged("Tags"); // Updating properties Tags
            }
        }
        public CategoryModel CurrentTag
        {
            get { return _currentTag; }
            set
            {
                SetProperty(ref _currentTag, value, () => CurrentTag);

                // Filters data depending on the selected Tag if the special element "All" is selected, all data of Tags will be selected
                if (_currentTag.TagName != "Все") {
                    IEnumerable<PackageModel> query = _instancePackages.Where(package => 
                                                      package.TagName.Contains(_currentTag.TagName));
                    Packages = new ObservableCollection<PackageModel>(query);
                }
                // Filters data depending on the selected category and displays the list of all these Tags
                else {
                    IEnumerable<PackageModel> query = _instancePackages.Where(package => 
                                                      package.Category == _currentCategory.CategoryName);
                    Packages = new ObservableCollection<PackageModel>(query);
                }
                if (Packages.Count > 0) {
                    // Selects the first element of the list for correct work of a data panel
                    CurrentPackage = Packages[0];
                    IndexPackage = 0;
                }                                      
            }
        }
        public string FilterPackages
        {
            get { return _filterPackages; }
            set
            {
                // This method to save a list current state before will begin search. 
                // If search box is empty that method restores a former list state on 117 line.
                FixStatePackage();

                SetProperty(ref _filterPackages, value, () => FilterPackages);
                // This instruction to filter data.
                if (!string.IsNullOrEmpty(_filterPackages) ) {
                    IEnumerable<PackageModel> filter = _instancePackages.Where(package
                        => package.Name.ToLower().Contains(_filterPackages.ToLower()));
                    _packages = new ObservableCollection<PackageModel>(filter);
                    OnPropertyChanged("Packages");
                }
                // Method to restore a former list state.
                else FixStatePackage();

                if (Packages.Count > 0) {
                    // Selects the first element of the list for correct work of a data panel
                    _currentPackage = Packages[0];
                    IndexPackage = 0;
                }
            }
        }

        public ICommand OpenDialogPackage => new RelayCommand(obj =>
        {
            PackagesDialog.OpenPackageDialog();
        });
        #endregion
    }
}
