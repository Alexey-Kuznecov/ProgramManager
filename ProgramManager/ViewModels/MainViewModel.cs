using System.Collections.Generic;
using ProgramManager.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using ProgramManager.Models.PackageDerives;

namespace ProgramManager.ViewModels
{
    public class MainViewModel : PropertiesChanged
    {
        #region Constructors

        public MainViewModel()
        {
            WrapPackage = BaseModel.GetPackages();
            Categories = CategoryModel.Categories;
        }

        #endregion

        #region Fields

        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private PackageBase _currentPackage;
        private CategoryModel _currentCategory;
        private List<CategoryModel> _categories;
        private ObservableCollection<WrapPackage> _wrapPackage;
        private int _indexTag;
        private int _indexPackage;
        private string _filterText;

        #endregion

        #region Properties

        public ObservableCollection<WrapPackage> WrapPackage
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
        public CategoryModel CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                SetProperty(ref _currentCategory, value, () => _currentCategory);
                // Запрос на изсенения категории пакетов
                if (_currentCategory != null && WrapPackage != null)
                    WrapPackage = BaseModel.GetPackages(_currentCategory);
                // Выбор первого пакета в списке после изменения категории
                if (_indexPackage < 0)
                    CurrentPackage = _wrapPackage[0].Packages[0];
                _filterText = null;
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
        public int IndexPackage
        {
            set
            {
                _indexPackage = value;

                if (_indexTag > -1 && _indexPackage > -1)
                    CurrentPackage = _wrapPackage[_indexTag].Packages[0];
            }
        }
        public int IndexTag
        {
            set
            {
                _indexTag = value;

                if (_indexTag > -1)
                    CurrentPackage = _wrapPackage[_indexTag].Packages[0];
            }
        }
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                SetProperty(ref _filterText, value, () => FilterText);

                // Эта инструкция для фильтрации данных.
                if (!string.IsNullOrEmpty(_filterText))
                {
                    List<PackageBase> result = new List<PackageBase>();

                    WrapPackage[_indexTag].Packages = Models.WrapPackage.AllPackages;

                    var filters = from pack in _wrapPackage[_indexTag]
                                  where pack.Name.ToLower().Contains(_filterText.ToLower())
                                  select pack;

                    foreach (var package in filters) result.Add(package);
                    // Добавления результатов фильтрации и обновления списка
                    WrapPackage[_indexTag].Packages = result;
                }
                else
                {
                    List<PackageBase> reset = new List<PackageBase>();
                    string tag =  WrapPackage.ElementAt(_indexTag).Name;

                    var query = from pack in WrapPackage[0].Packages
                        where pack?.TagOne == tag || pack.TagList.Contains(tag)
                        select pack;

                    // Возврат к предыдущему состоянию списка и выбор первого элемента списка
                    foreach (var packageBase in query) reset.Add(packageBase);
                    
                    // Сброс фильтрации:
                    WrapPackage[_indexTag].Packages = tag == "Все теги" ? Models.WrapPackage.AllPackages : reset;
                }

                if (_wrapPackage[_indexTag].Packages.Count > 0)
                {
                    CurrentPackage = _wrapPackage[_indexTag]?.Packages[0];
                }
                   
                OnPropertyChanged("WrapPackage");
            }
        }
        #endregion

        #region Commands
        
        public ICommand OpenDialogPackage => new RelayCommand(obj =>
        {
            PackagesDialogVisibility.OpenPackageDialog();
        });
        public ICommand TestCommand => new RelayCommand(obj =>
        {
            MessageBox.Show(CurrentPackage.GetType().ToString());
        });
        public ICommand Exit => new RelayCommand(obj =>
        {
            dispatcher.InvokeShutdown();
        });

        #endregion

    }
}
