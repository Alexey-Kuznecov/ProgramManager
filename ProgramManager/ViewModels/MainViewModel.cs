<<<<<<< HEAD
﻿

namespace ProgramManager.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Base;
    using InteractionLib;
    using Models;
    using Models.PackageModel;
    using Services;
    using Views;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : PropertiesChanged
    {
        #region Fields

        /// <summary>
        /// The dispatcher.
        /// </summary>
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// The current package.
        /// </summary>
        private PackageBase _currentPackage;

        /// <summary>
        /// The current category.
        /// </summary>
        private CategoryModel _currentCategory;

        /// <summary>
        /// The categories list.
        /// </summary>
        private List<CategoryModel> _categories;

        /// <summary>
        /// The wrap package.
        /// </summary>
        private ObservableCollection<WrapPackage> _wrapPackage;

        /// <summary>
        /// The index tag.
        /// </summary>
        private int _indexTag;

        /// <summary>
        /// The index package.
        /// </summary>
        private int _indexPackage;

        /// <summary>
        /// The filter text.
        /// </summary>
=======
﻿using System.Collections.Generic;
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
            _eventAggregate = new EventAggregate();
            EventAggregate.LoadPackage += LoadPackage;
        }

        #endregion

        #region Fields

        private EventAggregate _eventAggregate;
        private readonly Dispatcher _dispatcher;
        private PackageBase _currentPackage;
        private PackagesManager _packagesManager;
        private CategoryModel _currentCategory;
        private List<CategoryModel> _categories;
        private ObservableCollection<WrapPackage> _wrapPackage;
        private int _indexTag;
        private int _indexPackage;
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        private string _filterText;

        #endregion

<<<<<<< HEAD
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            var packagesManager = new PackagesManager();
            var eventAggregate = new EventAggregate();
            this._dispatcher = Dispatcher.CurrentDispatcher;
            this.Categories = CategoryModel.Categories;
            this.WrapPackage = PackagesManager.GetPackages();
            EventAggregate.LoadPackage += this.LoadPackages;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the wrap package.
        /// </summary>
        public ObservableCollection<WrapPackage> WrapPackage
        {
            get => this._wrapPackage;
            set
            {
                this.SetProperty(ref this._wrapPackage, value, () => this.WrapPackage);
            }
        }

        /// <summary>
        /// Gets or sets the categories list.
        /// </summary>
        public List<CategoryModel> Categories
        {
            get => this._categories;
            set
            {
                this.SetProperty(ref this._categories, value, () => this.Categories);
            }
        }

        /// <summary>
        /// Gets or sets the current category.
        /// </summary>
        public CategoryModel CurrentCategory
        {
            get => this._currentCategory;
            set
            {
                // Request for package category changes.
                if (this._currentCategory != null)
                {
                    this.WrapPackage = PackagesManager.GetPackages(value);
                }

                this._currentCategory = value;

                // Selecting the first package in the list after changing the category
                if (this._indexPackage < 0)
                {
                    this.CurrentPackage = this._wrapPackage[0].Packages[0];
                }

                this._filterText = null;

                // If the category has been changed this line notify all subscribers
                // _eventAggregate.OnCategoryChanged(_currentCategory);
            }
        }

        /// <summary>
        /// Gets or sets the current package.
        /// </summary>
        public PackageBase CurrentPackage
        {
            get => this._currentPackage;
            set
            {
                this.SetProperty(ref this._currentPackage, value, () => this.CurrentPackage);
            }
        }

        /// <summary>
        /// Sets the index package.
        /// </summary>
=======
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

                //Если категория была изменена эта строка уведомлит всех подписчиков
                 //_eventAggregate.OnCategoryChanged(_currentCategory);
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        public int IndexPackage
        {
            set
            {
<<<<<<< HEAD
                this._indexPackage = value;

                if (this._indexTag > -1 && this._indexPackage > -1)
                {
                    this.CurrentPackage = this._wrapPackage[this._indexTag].Packages[0];
                }
            }
        }

        /// <summary>
        /// Sets the index tag.
        /// </summary>
=======
                _indexPackage = value;

                if (_indexTag > -1 && _indexPackage > -1)
                    CurrentPackage = _wrapPackage[_indexTag].Packages[0];
            }
        }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        public int IndexTag
        {
            set
            {
<<<<<<< HEAD
                this._indexTag = value;

                if (this._indexTag > -1)
                {
                    this.CurrentPackage = this._wrapPackage[this._indexTag].Packages[0];
                }
            }
        }

        /// <summary>
        /// Gets or sets the filter text.
        /// </summary>
        public string FilterText
        {
            get => this._filterText;
            set
            {
                this.SetProperty(ref this._filterText, value, () => this.FilterText);

                // This instruction is for filtering data.
                if (!string.IsNullOrEmpty(this._filterText))
                {
                    List<PackageBase> result = new List<PackageBase>();

                    this.WrapPackage[this._indexTag].Packages = Models.WrapPackage.AllPackages;

                    var filters = from pack in this._wrapPackage[this._indexTag]
                                  where pack.Name.ToLower().Contains(this._filterText.ToLower())
                                  select pack;

                    // Add filter results and list updates
                    foreach (var package in filters)
                    {
                        result.Add(package);
                    }

                    this.WrapPackage[this._indexTag].Packages = result;
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                }
                else
                {
                    List<PackageBase> reset = new List<PackageBase>();
<<<<<<< HEAD
                    string tag = this.WrapPackage.ElementAt(this._indexTag).Name;

                    var query = from pack in this.WrapPackage[0].Packages
                        where pack?.TagOne == tag || pack.TagList.Contains(tag)
                        select pack;

                    // Return to previous list state and select first list item
                    foreach (var packageBase in query)
                    {
                        reset.Add(packageBase);
                    }

                    // Reset the filter result:
                    this.WrapPackage[this._indexTag].Packages = tag == "Все теги" ? Models.WrapPackage.AllPackages : reset;
                }

                // Expand first pachage data in the detail panel.
                if (this._wrapPackage[this._indexTag].Packages.Count > 0)
                {
                    this.CurrentPackage = this._wrapPackage[this._indexTag]?.Packages[0];
                }

                this.OnPropertyChanged("WrapPackage");
            }
        }
        
        #endregion

        #region Commands

        /// <summary>
        /// The command create package.
        /// </summary>
        public ICommand CommandCreatePackage => new RelayCommand(obj =>
        {
            PackagesDialogVisibility.CreatePackageDialog(_currentCategory);
        });

        /// <summary>
        /// The command test command.
        /// </summary>
        public ICommand CommandTestCommand => new RelayCommand(obj =>
        {
            // MessageBox.Show(CurrentPackage.GetType().ToString());
            MessageBox.Show(_currentPackage.Id.ToString());
        });

        /// <summary>
        /// The command exit.
        /// </summary>
        public ICommand CommandExit => new RelayCommand(obj =>
        {
            _dispatcher.InvokeShutdown();
        });

        /// <summary>
        /// The command update package.
        /// </summary>
        public ICommand CommandUpdatePackage => new RelayCommand(obj => this.UpdatePackage());

        /// <summary>
        /// The command remove package.
        /// </summary>
        public ICommand CommandRemovePackage => new RelayCommand(obj => this.RemovePackage());

        /// <summary>
        /// The command open icon editor.
        /// </summary>
        public ICommand CommandOpenIconEditor => new RelayCommand(obj =>
        {
           // Process.Start(@"..\..\..\IconMaker\bin\Debug\IconMaker.exe");
            PluginManager.Plugin plugin = PluginManager.Execute(PluginType.TagEditor);
            plugin.ExecuteAction(null);
        });

        /// <summary>
        /// The command open settings.
        /// </summary>
        public ICommand CommandOpenSettings => new RelayCommand(obj =>
        {
            SettingsView settings = new SettingsView();
            settings.ShowDialog();
        });

        #endregion

        #region Methods

        /// <summary>
        /// The load packages of the current category.
        /// </summary>
        /// <param name="message"> The message. </param>
        private void LoadPackages(object message)
        {
            this.WrapPackage = PackagesManager.GetPackages(this._currentCategory);

            if (this._indexPackage < 0)
            {
                this.CurrentPackage = this._wrapPackage[0].Packages[0];
            }

            this._filterText = null;
        }

        /// <summary>
        /// Update the selected package.
        /// </summary>
        private void UpdatePackage()
        {
            PackagesDialogVisibility.EditPackageDialog(this._currentPackage);
        }

        /// <summary>
        /// Removes the selected package.
        /// </summary>
        private void RemovePackage()
        {
            EventAggregate e = new EventAggregate();
            e.OnRemovePackage(this._currentPackage.Id);
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        }

        #endregion
    }
}
