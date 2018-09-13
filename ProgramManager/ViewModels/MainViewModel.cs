using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using ProgramManager.Model;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel : PropertiesChanged
    {
        #region Constuctor

        public MainViewModel()
        {
            Packages = _instancePackages;
            Category = new ObservableCollection<CategoryModel>(CategoryAccess.GetCategories());
        }

        #endregion

        #region Fields

        // The fields for properties
        private PackageModel _currentPackage;
        private CategoryModel _currentSubcategory;
        private CategoryModel _currentCategory;
        private ObservableCollection<PackageModel> _packages;
        private ObservableCollection<PackageModel> _storage;
        private ObservableCollection<PackageModel> _instancePackages
                        = new ObservableCollection<PackageModel>(PackageAccess.GetPackages());
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
        public ObservableCollection<CategoryModel> Subcategory { get; set; }
        public ObservableCollection<CategoryModel> Category { get; set; }        
        /// <summary>
        /// Receives and transfers information on the current category to view 
        /// </summary>       
        public PackageModel CurrentPackage
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
                SetProperty(ref _currentCategory, value, () => CurrentCategory);
                // When the SelectedItem attribute value changes, all subcategories are filtered depending on the selected category.
                // Request of these Subcategories
                Subcategory = new ObservableCollection<CategoryModel>(CategoryAccess.GetSubcategories());

                // Filters subcategory depending on the selected category.
                IEnumerable<CategoryModel> query = Subcategory.Where(category => category.CategoryName == _currentCategory.CategoryName);
                // Creating new data
                Subcategory = new ObservableCollection<CategoryModel>(query);

                // Adding special elements.
                Subcategory.Insert(0, new CategoryModel() { SubcategoryName = "Все" });
                // Updating properties Subcategory
                OnPropertyChanged("Subcategory"); 
            }
        }
        public CategoryModel CurrentSubcategory
        {
            get { return _currentSubcategory; }
            set
            {
                SetProperty(ref _currentSubcategory, value, () => CurrentSubcategory);

                // Filters data depending on the selected subcategory if the special element "All" is selected, all data of subcategories will be selected
                if (_currentSubcategory.SubcategoryName != "Все") {
                    IEnumerable<PackageModel> query = _instancePackages.Where(package => package.Subcategory.Contains(_currentSubcategory.SubcategoryName));
                    Packages = new ObservableCollection<PackageModel>(query);
                }
                // Filters data depending on the selected category and displays the list of all these subcategories
                else {
                    IEnumerable<PackageModel> query = _instancePackages.Where(package => package.Category == _currentCategory.CategoryName);
                    Packages = new ObservableCollection<PackageModel>(query);
                }
                if (Packages.Count > 0) {
                    // Selects the first element of the list for correct work of a data panel
                    _currentPackage = Packages[0];
                    // Updating of property of the current package
                    OnPropertyChanged("CurrentPackage");
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
                if (_filterPackages != "") {
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
                    // Updating of property of the current package
                    OnPropertyChanged("CurrentPackage");
                }
            }
        }

        #endregion
    }
}
