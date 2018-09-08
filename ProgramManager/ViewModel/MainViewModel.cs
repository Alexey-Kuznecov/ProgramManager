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
        private ObservableCollection<PackageModel> _package;
        private ObservableCollection<CategoryModel> _category;
        private ObservableCollection<CategoryModel> _subcategory;
        private CategoryModel _currentCategory;
        public ObservableCollection<CategoryModel> Subcategory { get; set; }
        public ObservableCollection<CategoryModel> Category { get; set; }
        public ObservableCollection<PackageModel> Packages
        {
            get { return _package; }
            set
            {
                _package = value as ObservableCollection<PackageModel>;
                OnPropertyChanged("Packages");
            }
        }
        //public ICommand InsertCommand { get; }
        //public ICommand UpdateCommand { get; }
        /// <summary>
        /// Получает и передает в view информацию о текущем пакете
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

                    Subcategory = new ObservableCollection<CategoryModel>(CategoryAccess.GetSubcategories());

                    IEnumerable<CategoryModel> query = Subcategory.Where(package => package.CategoryName == _currentCategory.CategoryName);

                    Subcategory = new ObservableCollection<CategoryModel>(query);

                    Subcategory.Insert(0, new CategoryModel() { SubcategoryName = "Все" });

                    OnPropertyChanged("Subcategory");
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
                    
                    // TODO: Оптимизировать эту строку(Эта строка повтроно запрашивает данные что может повлиять на производительность программы)
                    Packages = new ObservableCollection<PackageModel>(PackageAccess.GetPackages());
                    
                    // Фильтрует данные взависимости от выбранной подкотегории
                    if (_currentSubcategory.SubcategoryName != "Все" && _currentCategory.CategoryName == _currentSubcategory.CategoryName)
                    {
                        IEnumerable<PackageModel> query = Packages.Where(package => package.Subcategory == _currentSubcategory.SubcategoryName);
                        Packages = new ObservableCollection<PackageModel>(query);
                    }
                    
                    if (Packages.Count > 0)
                    {
                        // Указатель на перевый элемент списка для корректной работы информационной панели 
                        _currentPackage = Packages[0];
                        // Обновление свойства текущего пакета
                        OnPropertyChanged("CurrentPackage");
                    }
                    OnPropertyChanged("CurrentSubcategory");                 
                }                          
            }
        }
        /// <summary>
        /// Конструктор поумолчанию
        /// </summary>
        public MainViewModel()
        {
            //UpdateCommand = new RelayCommand(this, Update);
            //InsertCommand = new RelayCommand(this, Insert);

            //Packages = new ObservableCollection<PackageModel>(PackageAccess.GetPackages());
            //Subcategory = new ObservableCollection<CategoryModel>(CategoryAccess.GetSubcategories());
            Category = new ObservableCollection<CategoryModel>(CategoryAccess.GetCategories());
        }
    }
}
