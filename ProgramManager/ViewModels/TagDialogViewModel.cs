using System.Collections.Generic;
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
    }
}
