using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ProgramManager.Models;
using ProgramManager.Models.PackageModel;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.Plugins.TagsEditor
{
    public class TagDialogModel : PropertiesChanged
    {
        public static List<string> List = new List<string>();
        // ReSharper disable once InconsistentNaming
        protected bool _isChecked;
        public string Name { get; set; }

        #region Properties
        /// <summary>
        /// Property binding with property IsChecked of Checkbox.
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        /// <summary>
        /// Marks tag as select.
        /// </summary>
        public ICommand Checked => new RelayCommand(obj =>
        {
            if ((bool)obj) List.Add(Name);
            else List.Remove(Name);
        });
        #endregion
    }
    public class TagDialogViewModel : TagDialogModel
    {
        public TagDialogViewModel()
        {
            DataSync.PackageLoad += InitialTagList;
        }
        private static TagDialog _tagDialog;
        private static ObservableCollection<TagDialogModel> _tagList;
        private string _filterTags;

        #region Properties
        /// <summary>
        /// Property contains list tags.
        /// </summary>
        public ObservableCollection<TagDialogModel> TagList
        {
            get { return _tagList; }
            set
            {
                _tagList = value;
                OnPropertyChanged("TagList");
            }
        }
        /// <summary>
        /// Compares box text with text of Name property
        /// and replaces tags on filter result.
        /// </summary>
        public string FilterTags
        {
            get { return _filterTags; }
            set
            {
                _filterTags = value;

                if (string.IsNullOrEmpty(_filterTags))
                   TagList = InteractonTagEditor.TagList.ToObservableCollection();
                else
                {
                    TagList.Clear();
                    #region Filter body
                    // Compares box text with text of Name property.
                    var query = from name in InteractonTagEditor.TagList
                        where name.Name.ToLower().Contains(_filterTags.ToLower())
                        select name;
                    // Removes tags from the collection.
                    // This way allows to keep tags tagged.
                    foreach (var tag in query)
                        TagList.Add(tag);

                    #endregion
                }
                OnPropertyChanged("FilterTags");
                OnPropertyChanged("TagList");
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
                InteractonTagEditor.TagList.Add(new TagDialogModel { Name = FilterTags });
            }
        });
        /// <summary>
        /// Clear textbox.
        /// </summary>
        public ICommand ClearBox => new RelayCommand(obj =>
        {
            FilterTags = null;
            OnPropertyChanged("FilterTags");
        });
        #endregion

        #region Functions
        /// <summary>
        /// Receives data (list tags of current categories) and repackage List&lt;TagDialogModel&gt; in .
        /// </summary>
        /// <param name="sender">Object type BaseEventArgs.</param>
        /// <param name="packaArgs">Waiting object type TagDialogModul and its Name property.</param>
        public void DisplayLoadTagList(object sender, BaseEventArgs packaArgs)
        {
            List<WrapPackage> tagsList = packaArgs.Package as List<WrapPackage>;
            TagList = new ObservableCollection<TagDialogModel>();

            if (tagsList != null)
                foreach (var tag in tagsList)
                    TagList.Add(new TagDialogModel { Name = tag.Name });

        }
        /// <summary>
        /// Marks tags that contained in the package.
        /// <property cref="TagList">Property fire event.<event cref="EventAggregate.OnLoadTagsList"/></property>
        /// </summary>
        /// <param name="obj">List tags or single tag.</param>
        private void InitialTagList(PackageBase obj)
        {
            List<string> list = obj.TagList;
            string single = obj.TagOne;

            foreach (var tag in TagList)
            {
                if (list.Count > 1)
                {
                    if (list.Any(n => n == tag.Name))
                    {
                        List.Add(tag.Name);
                        tag.IsChecked = true;
                    }
                }
                if (single == tag.Name)
                {
                    List.Add(tag.Name);
                    tag.IsChecked = single == tag.Name;
                }   
            }
            InteractonTagEditor.TagList = TagList.ToList();
            List = List?.Distinct().ToList();
        }
        #endregion
    }
}
