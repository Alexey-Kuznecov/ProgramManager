using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using ProgramManager.Model;

namespace ProgramManager.ViewModel
{
    public partial class MainViewModel : PropertiesChanged
    {
        private PackageModel _currentPackage;
        private CategoryModel _currentSubcategory;
        private CategoryModel _currentCategory;
        private string _filterPackages;
        private ObservableCollection<PackageModel> _enstancePackage;
        private ObservableCollection<PackageModel> _packages;
        private ObservableCollection<PackageModel> _backupPackage;
        public ObservableCollection<CategoryModel> Subcategory { get; set; }
        public ObservableCollection<CategoryModel> Category { get; set; }
        /// <summary>
        /// Receives and transfers information on the current packet to view
        /// </summary>
        public ObservableCollection<PackageModel> Packages
        {
            get { return _packages; }
            set
            {
                _packages = value as ObservableCollection<PackageModel>;
                OnPropertyChanged("Packages");
            }
        }
        /// <summary>
        /// Receives and transfers information on the current category to view 
        /// </summary>
        public PackageModel CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                if (_currentPackage != value && value != null)
                {
                    _currentPackage = value;
                    OnPropertyChanged("CurrentPackage");
                }
            }
        }
        public CategoryModel CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                if (_currentCategory != value && value != null)
                {
                    _currentCategory = value;

                    // Request of these Subcategories
                    Subcategory = new ObservableCollection<CategoryModel>(CategoryAccess.GetSubcategories());
                    
                    // Filters subcategory depending on the selected category.
                    IEnumerable<CategoryModel> query = Subcategory.Where(package => package.CategoryName == _currentCategory.CategoryName);

                    // Creating new data
                    Subcategory = new ObservableCollection<CategoryModel>(query);
                    
                    // Adding special elements.
                    Subcategory.Insert(0, new CategoryModel() { SubcategoryName = "Все" });

                    // Updating properties Subcategory
                    OnPropertyChanged("Subcategory");
                }   
            }
        }
        public string FilterPackages
        {
            get { return _filterPackages; }
            set
            {
                if (_filterPackages == null || _filterPackages == "")
                    _backupPackage = Packages;

                _filterPackages = value;

                if (_filterPackages != null && _filterPackages != "")
                {
                    IEnumerable<PackageModel> filter = _enstancePackage.Where(package => package.Name.ToLower().Contains(_filterPackages.ToLower()));

                    _packages = new ObservableCollection<PackageModel>(filter);

                    OnPropertyChanged("Packages");
                }
                else
                    Packages = _backupPackage;

                if (Packages.Count > 0)
                {
                    // Selects the first element of the list for correct work of a data panel
                    _currentPackage = Packages[0];
                    // Updating of property of the current package
                    OnPropertyChanged("CurrentPackage");
                }
            }
        }
        public CategoryModel CurrentSubcategory
        {
            get { return _currentSubcategory; }
            set
            {
                if (_currentSubcategory != value && value != null)
                {
                    _currentSubcategory = value;

                    // Filters data depending on the selected subcategory if the special element "All" is selected, all data of subcategories will be selected
                    if (_currentSubcategory.SubcategoryName != "Все")
                    {
                        IEnumerable<PackageModel> query = _enstancePackage.Where(package => package.Subcategory == _currentSubcategory.SubcategoryName);
                        Packages = new ObservableCollection<PackageModel>(query);
                    }
                    // Filters data depending on the selected category and displays the list of all these subcategories
                    else
                    {
                        IEnumerable<PackageModel> query = _enstancePackage.Where(package => package.Category == _currentCategory.CategoryName);
                        Packages = new ObservableCollection<PackageModel>(query);
                    }

                    if (Packages.Count > 0)
                    {
                        // Selects the first element of the list for correct work of a data panel
                        _currentPackage = Packages[0];
                        // Updating of property of the current package
                        OnPropertyChanged("CurrentPackage");
                    }               
                }                          
            }
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            _enstancePackage = new ObservableCollection< PackageModel > (PackageAccess.GetPackages());
            Packages = _enstancePackage;
            Category = new ObservableCollection<CategoryModel>(CategoryAccess.GetCategories());
        }
    }
}
