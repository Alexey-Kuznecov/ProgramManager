using System.Collections.Generic;
using ProgramManager.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using ProgramManager.Models.PackageModel;
using ProgramManager.Services;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace ProgramManager.ViewModels
{
    public class MainViewModel : PropertiesChanged
    {
        #region Constructors

        public MainViewModel()
        {
            _packagesManager = new PackagesManager();
            _dispatcher =  Dispatcher.CurrentDispatcher;
            Categories = CategoryModel.Categories;
            WrapPackage = PackagesManager.GetPackages();
            EventAggregate.LoadPackage += LoadPackage;
        }

        #endregion

        #region Fields

        private readonly Dispatcher _dispatcher;
        private PackageBase _currentPackage;
        private PackagesManager _packagesManager;
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
                // Запрос на изменения категории пакетов
                if (_currentCategory != null)
                    WrapPackage = PackagesManager.GetPackages(value);

                _currentCategory = value;
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
        
        public ICommand CmdCreatePackage => new RelayCommand(obj =>
        {
            PackagesDialogVisibility.CreatePackageDialog(_currentCategory);
        });
        public ICommand CmdTestCommand => new RelayCommand(obj =>
        {
            //MessageBox.Show(CurrentPackage.GetType().ToString());
            MessageBox.Show(_currentPackage.Id.ToString());
        });
        public ICommand CmdExit => new RelayCommand(obj =>
        {
            _dispatcher.InvokeShutdown();
        });
        public ICommand CmdUpdatePackage => new RelayCommand(obj => { UpdatePackage(); });
        public ICommand CmdRemovePackage => new RelayCommand(obj => { RemovePackage(); });

        #endregion

        #region Method

        private void LoadPackage(object message)
        {
            WrapPackage = PackagesManager.GetPackages(_currentCategory);

            if (_indexPackage < 0)
                CurrentPackage = _wrapPackage[0].Packages[0];
            _filterText = null;
        }
        private void UpdatePackage()
        {
            PackagesDialogVisibility.EditPackageDialog(_currentPackage);
        }
        private void RemovePackage()
        {
            EventAggregate e = new EventAggregate();
            e.OnRemovePackage(_currentPackage.Id);
        }

        #endregion
    }
}
