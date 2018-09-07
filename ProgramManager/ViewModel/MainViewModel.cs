using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using ProgramManager.Model;

namespace ProgramManager.ViewModel
{
    public partial class MainViewModel : PropertiesChanged
    {
        private PackageModel _currentPackage;
        private CategoryModel _currentCategory;
        private ObservableCollection<PackageModel> _package;

        public ObservableCollection<PackageModel> Packages
        {
            get { return _package; }
            set
            {               
                _package = value as ObservableCollection<PackageModel>;
                OnPropertyChanged("Packages");
            }
        }
        public ObservableCollection<CategoryModel> Category { set; get; }
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
                    
                    // TODO: Оптимизировать эту строку(Эта строка повтроно запрашивает данные что может повлиять на производительность программы)
                    Packages = new ObservableCollection<PackageModel>(PackageAccess.GetPackages());
                    
                    // Фильтрует данные взависимости от выбранной подкотегории
                    if (_currentCategory.SubcategoryName != "Все")
                    {
                        IEnumerable<PackageModel> query = Packages.Where(package => package.Subcategory == _currentCategory.SubcategoryName);
                        Packages = new ObservableCollection<PackageModel>(query);
                    }
                    
                    if (Packages.Count > 0)
                    {
                        // Указатель на перевый элемент списка для корректной работы информационной панели 
                        _currentPackage = Packages[0];
                        // Обновление свойства текущего пакета
                        OnPropertyChanged("CurrentPackage");
                    }
                    OnPropertyChanged("CurrentCategory");                 
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

            Category = new ObservableCollection<CategoryModel>(CategoryAccess.GetCategories());
        }
    }
}
