using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Models;
using ProgramManager.Services;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.ViewModels
{
    public class TagDialogModel
    {
        public string Name { get; set; }

        protected static List<string> _list = new List<string>();

        public ICommand Checked => new RelayCommand(obj =>
        {
            if ((bool)obj)  _list.Add(Name);

            else _list.Remove(Name);              
        });
    }
    public class TagDialogViewModel : TagDialogModel
    {
        private static TagDialog _tagDialog;
        public static ObservableCollection<TagDialogModel> TagList { get; set; }
        /// <summary>
        /// Получает данные(список тегов) от издателя события и преобразует в удобный формат данных для их вывода.
        /// </summary>
        /// <param name="sender">Источник</param>
        /// <param name="obj">Ожидается объект типа WrapPackage и его свойство Name</param>
        public static void DisplayTagList(object sender, BaseEventArgs tags)
        {
            List<WrapPackage> packs = tags.Package as List<WrapPackage>;
            
            if (packs != null)
            {
                TagList = new ObservableCollection<TagDialogModel>();

                foreach (var item in packs)
                    TagList.Add(new TagDialogModel() { Name = item.Name });
            }
        }
        public ICommand SendSelected => new RelayCommand(obj =>
        {
            Messenger.Default.Send(_list);
            _tagDialog = obj as TagDialog;
            if (_tagDialog != null) _tagDialog.Visibility = Visibility.Hidden;
        });
        public ICommand Cancel => new RelayCommand(obj =>
        {
            _tagDialog = obj as TagDialog;
            if (_tagDialog != null) _tagDialog.Visibility = Visibility.Hidden;
        });
        public ICommand AddTag => new RelayCommand(obj =>
        {
            if (Name != null)
                TagList.Add(new TagDialogModel() { Name = Name });
        });
    }
}
