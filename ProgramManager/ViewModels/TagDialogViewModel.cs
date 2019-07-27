using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models;
using ProgramManager.Services;
using ProgramManager.Views.DialogPacks;
using ProgramManager.Models.PackageModel;
using System.Linq;
using ProgramManager.Plugins;
using ProgramManager.ViewModels.Base;

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
        public static List<string> List = new List<string>();

        public ICommand Checked => new RelayCommand(obj =>
        {
            if ((bool)obj)  List.Add(Name);
            else List.Remove(Name);              
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

        #region Commads

        public ICommand SendSelected => new RelayCommand(obj =>
        {
            Messenger.Default.Send(List);
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

        #endregion

        /// <summary>
        /// Получает данные(список тегов текущей категории).
        /// </summary>
        /// <param name="sender">Источник</param>
        /// <param name="packaArgs">Ожидается объект типа List&lt;TagDialogModel&gt; и его свойство Name</param>
        public static void DisplayTagList(object sender, BaseEventArgs packaArgs)
        {
            List<TagDialogModel> tagsList = InteractonTagEditor.TagList ?? (List<TagDialogModel>)packaArgs.Package;
            TagList = new ObservableCollection<TagDialogModel>();

            if (tagsList != null)
                foreach (var name in tagsList)
                    TagList.Add(new TagDialogModel { Name = name.Name });
        }
        /// <summary>
        /// Marks tags that contained in the package.
        /// <property cref="TagList">Property fire event.<event cref="EventAggregate.OnLoadTagsList"/></property>
        /// </summary>
        /// <param name="obj">Список тегов и одиночный тег.</param>
        private void InitialTagList(PackageBase obj)
        {
            List<string> list = obj.TagList;
            string single = InteractonTagEditor.TagSingle ?? obj.TagOne;

            foreach (var tag in TagList)
            {
                if (list.Count > 1)
                {
                    if (list.Any(n => n == tag.Name)) tag.IsChecked = true;
                    else tag.IsChecked = false;
                }
                else
                {
                    if (single == tag.Name) tag.IsChecked = true;
                    else tag.IsChecked = false;
                }
            }
        }
    }
}
