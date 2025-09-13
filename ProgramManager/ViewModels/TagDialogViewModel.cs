<<<<<<< HEAD
﻿
namespace ProgramManager.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using AlexLibWpf.Help;
    using Base;
    using InteractionLib;
    using InteractionLib.TagEditor;
    using Models;
    using Models.PackageModel;
    using Services;
    using Views;

    /// <summary>
    /// The tag dialog view model.
    /// </summary>
    public class TagDialogViewModel : TagDialogModel
    {
        /// <summary>
        /// Instance TagDialog class.
        /// </summary>
        private static TagDialog _tagDialog;
        
        /// <summary>
        /// The tag list.
        /// </summary>
        private static ObservableCollection<TagDialogModel> _tagList;

        /// <summary>
        /// The filter tags.
        /// </summary>
        private string _filterTags;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDialogViewModel"/> class.
        /// </summary>
        public TagDialogViewModel()
        {
            DataSync.PackageLoad += this.InitialTagList;
            this.List = new List<string>();
        }

        #region Properties
        
        /// <summary>
        /// Gets or sets list tags.
        /// </summary>
        public ObservableCollection<TagDialogModel> TagList
        {
            get => _tagList;
            set
            {
                _tagList = value;
                this.OnPropertyChanged("TagList");
            }
        }

        /// <summary>
        /// Gets or sets a value from a field to filter a tag list.
        /// </summary>
        public string FilterTags
        {
            get => this._filterTags;
            set
            {
                this._filterTags = value;

                if (string.IsNullOrEmpty(this._filterTags))
                {
                    this.TagList = Interaction.TagList.ToObservableCollection();
                }
                else
                {
                    this.TagList.Clear();

                    // Compares box text with text of Name property.
                    var query = from name in Interaction.TagList
                        where name.Name.ToLower().Contains(this._filterTags.ToLower())
                        select name;

                    // Removes tags from the collection.
                    // This way allows to keep tags tagged.
                    foreach (var tag in query)
                    {
                        this.TagList.Add(tag);
                    }
                }

                this.OnPropertyChanged("FilterTags");
                this.OnPropertyChanged("TagList");
            }
        }
        
        #endregion

        #region Commads
        
        /// <summary>
        /// Send tag list to package dialog.
        /// </summary>
        public ICommand SendSelected => new RelayCommand(obj =>
        {
            DataSync.TagLoad.Invoke(List);
            Cancel.Execute(obj);
        });
        
        /// <summary>
        /// Button to close the window. 
        /// </summary>
        public ICommand Cancel => new RelayCommand(obj =>
        {
            _tagDialog = obj as TagDialog;
            _tagDialog?.Close();
        });
        
        /// <summary>
        /// Button to add new tag if list has no tag yet..
        /// </summary>
        public ICommand AddTag => new RelayCommand(obj =>
        {
            if (FilterTags != null)
            {
                TagList.Add(new TagDialogModel { Name = FilterTags });
                Interaction.TagList.Add(new TagDialogModel { Name = FilterTags });
            }
        });
        
        /// <summary>
        /// Clear text field.
        /// </summary>
        public ICommand ClearBox => new RelayCommand(obj =>
        {
            FilterTags = null;
            OnPropertyChanged("FilterTags");
        });
        
        #endregion

        #region Functions
        
        /// <summary>
        /// Receives data (list tags of current categories) and repackage TagDialogModel in.
        /// </summary>
        /// <param name="sender">Object type <c>BaseEventArgs</c>.</param>
        /// <param name="package">Waiting a object type <c>TagDialogModel</c> and its Name property.</param>
        public void DisplayLoadTagList(object sender, BaseEventArgs package)
        {
            var tagsList = package.Package as List<WrapPackage>;
            this.TagList = new ObservableCollection<TagDialogModel>();

            if (tagsList != null)
            {
                foreach (var tag in tagsList)
                {
                    this.TagList.Add(new TagDialogModel { Name = tag.Name });
                }
            }   
        }
        
        /// <summary>
        /// Marks tags that contained in the package.
        /// <property cref="TagList">Property fire event.<event cref="EventAggregate.OnLoadTagsList"/></property>
        /// </summary>
        /// <param name="obj">List tags or single tag.</param>
        private void InitialTagList(object obj)
        {
            PackageBase packageBase = obj as PackageBase ?? throw new Exception("The tag list cannot be undefined.");

            List<string> list = packageBase.TagList;
            string single = packageBase.TagOne;

            foreach (var tag in this.TagList)
            {
                if (list.Count > 1)
                {
                    if (list.Any(n => n == tag.Name))
                    {
                        this.List.Add(tag.Name);
                        tag.IsChecked = true;
                    }
                }
                if (single == tag.Name)
                {
                    this.List.Add(tag.Name);
                    tag.IsChecked = single == tag.Name;
                }   
            }

            Interaction.TagList = this.TagList.ToList();
            this.List = this.List?.Distinct().ToList();
        }
        
        #endregion
=======
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models;
using ProgramManager.Services;
using ProgramManager.Views.DialogPacks;
using ProgramManager.Models.PackageModel;
using System.Linq;

namespace ProgramManager.ViewModels
{
    public class TagDialogModel : PropertiesChanged
    {
        protected bool _isChecked;
        public string Name { get; set; }
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        protected static List<string> _list = new List<string>();

        public ICommand Checked => new RelayCommand(obj =>
        {
            if ((bool)obj)  _list.Add(Name);
            else _list.Remove(Name);              
        });
    }
    public class TagDialogViewModel : TagDialogModel
    {
        public TagDialogViewModel()
        {
            Messenger.Default.Register<PackageBase>(this, obj => InitialTagList(obj));
        }
        private static TagDialog _tagDialog;
        public static ObservableCollection<TagDialogModel> TagList { get; set; }
        public ICommand SendSelected => new RelayCommand(obj =>
        {
            Messenger.Default.Send(_list);
            _tagDialog = obj as TagDialog;
            if (_tagDialog != null) _tagDialog.Close();
        });
        public ICommand Cancel => new RelayCommand(obj =>
        {
            _tagDialog = obj as TagDialog;
            if (_tagDialog != null) _tagDialog.Close();
        });
        public ICommand AddTag => new RelayCommand(obj =>
        {
            if (Name != null)
                TagList.Add(new TagDialogModel() { Name = Name });
        });
        /// <summary>
        /// Получает данные(список тегов текущей категории).
        /// </summary>
        /// <param name="sender">Источник</param>
        /// <param name="wrapPackage">Ожидается объект типа WrapPackage и его свойство Name</param>
        public static void DisplayTagList(object sender, BaseEventArgs wrapPackage)
        {
            List<WrapPackage> packs = wrapPackage.Package as List<WrapPackage>;

            if (packs != null)
            {
                TagList = new ObservableCollection<TagDialogModel>();

                foreach (var item in packs)
                    TagList.Add(new TagDialogModel() { Name = item.Name });
            }
        }
        /// <summary>
        /// Отмечает теги которые содержит пакет
        /// </summary>
        /// <param name="obj">Список тегов и одиночный тег.</param>
        private void InitialTagList(PackageBase obj)
        {
            foreach (var tag in TagList)
            {
                if (obj.TagList.Count > 1)
                {
                    if (obj.TagList.Any(n => n == tag.Name)) tag.IsChecked = true;
                    else tag.IsChecked = false;
                }
                else
                {
                    if (obj.TagOne == tag.Name) tag.IsChecked = true;
                    else tag.IsChecked = false;
                }
            }
        }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    }
}
