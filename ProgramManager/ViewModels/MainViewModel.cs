#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        // Поля для свойств
        private dynamic _currentPackage;
        private CategoryModelOb _currentTag;
        private CategoryModelOb _currentCategory;
        private ObservableCollection<PackageModel> _packages;
        private ObservableCollection<PackageModel> _storage;
        private ObservableCollection<PackageModel> _instancePackages = null;
        private ObservableCollection<CategoryModelOb> _instanceCategories = null;
        private int _indexPackage;
        private string _filterPackages;

        #endregion

        #region Properties

        /// <summary>
        /// Получает и передает информацию о текущем пакете для просмотра
        /// </summary>
        public ObservableCollection<PackageModel> Packages
        {
            get { return _packages; }
            set { SetProperty(ref _packages, value, () => Packages); }
        }
        public ObservableCollection<CategoryModelOb> Tags { get; set; }
        public ObservableCollection<CategoryModelOb> Category { get; set; }
        public int IndexPackage
        {
            get { return _indexPackage; }
            set { SetProperty(ref _indexPackage, value, () => IndexPackage); }
        }
        /// <summary>
        /// Получает и передает информацию о текущей категории для просмотра
        /// </summary>
        public dynamic CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                _currentPackage = value;
                      
                if (value != null)
                    //SetCurrentPackage(value);

                OnPropertyChanged("CurrentPackage");  
            }
        }
        public CategoryModelOb CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                SetProperty(ref _currentCategory, value, () => CurrentCategory);
                // При изменении значения атрибута SelectedItem все теги фильтруются в зависимости от выбранной категории.
                // Запрос этих тегов.
                CalculateByTag();
                // Фильтрует теги в зависимости от выбранной категории.
                IEnumerable<CategoryModelOb> query = Tags.Where(category =>
                                                   category.CategoryName == _currentCategory.CategoryName);

                Tags = new ObservableCollection<CategoryModelOb>(query); // Создание новых данных.
                Tags.Insert(0, new CategoryModelOb() { TagName = "Все", Count = CurrentCategory.Count }); // Добавление специальных элементов.    
                OnPropertyChanged("Tags"); // Обновление тегов свойств, добавление специальных элементов.
            }
        }
        public CategoryModelOb CurrentTag
        {
            get { return _currentTag; }
            set
            {
                SetProperty(ref _currentTag, value, () => CurrentTag);

                // Фильтрация данных в зависимости от выбранного тега если выбран специальный элемент "все", будут выбраны все данные тегов
                if (_currentTag.TagName != "Все") {
                    IEnumerable<PackageModel> query = _instancePackages.Where(package => 
                                                      package.TagName.Contains(_currentTag.TagName));
                    Packages = new ObservableCollection<PackageModel>(query);
                }
                // Фильтрует данные в зависимости от выбранной категории и отображает список всех этих тегов
                else {
                    IEnumerable<PackageModel> query = _instancePackages.Where(package => 
                                                      package.Category == _currentCategory.CategoryName);
                    Packages = new ObservableCollection<PackageModel>(query);
                }
                if (Packages.Count > 0) {
                    // Выбирает первый элемент списка для корректной работы панели данных
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
                // Этот метод для сохранения текущего состояния списка перед началом поиска. 
                // Если поле поиска пустое, то метод восстанавливает прежнее состояние списка на 133 строке.
                FixStatePackage();

                SetProperty(ref _filterPackages, value, () => FilterPackages);
                // Эта инструкция для фильтрации данных.
                if (!string.IsNullOrEmpty(_filterPackages) ) {
                    IEnumerable<PackageModel> filter = _instancePackages.Where(package
                        => package.Name.ToLower().Contains(_filterPackages.ToLower()));
                    _packages = new ObservableCollection<PackageModel>(filter);
                    OnPropertyChanged("Packages");
                }
                // Метод восстановления прежнего состояния списка.
                else FixStatePackage();

                if (Packages.Count > 0) {
                    // Выбирает первый элемент списка для корректной работы панели данных
                    _currentPackage = Packages[0];
                    IndexPackage = 0;
                }
            }
        }
        #endregion
    }
}
