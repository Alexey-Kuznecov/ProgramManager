using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProgramManager.Models;
using System.Windows;

namespace ProgramManager.ViewModels
{
    public class TextFieldViewModel : PropertiesChanged
    {
        public TextFieldViewModel()
        {
            AddTextField = new RelayCommand(obj => AddNewTextField());
            RemoveTextField = new RelayCommand(obj => TextField.Remove(obj as TextFieldModel));
        }
        private const string AUTOCOMPLETE_ICON = "../../Resources/Icons/Businessman_48px.png";
        private const string DELETE_ICON = "../../Resources/Icons/Delete_48px.png";

        private void AddNewTextField()
        {
            TextField.Add(new TextFieldModel()
            {
                LabelField = "Автор: ",
                HintField = "Имя...",
                AutoCompleteIcon = AUTOCOMPLETE_ICON,
                DeleteTextFieldIcon = DELETE_ICON
            });
        }

        public ObservableCollection<TextFieldModel> TextField { get; } = new ObservableCollection<TextFieldModel>() {
            new TextFieldModel() { LabelField = "Автор: ", HintField = "Имя...", AutoCompleteIcon = AUTOCOMPLETE_ICON, DeleteTextFieldIcon = DELETE_ICON },
            new TextFieldModel() { LabelField = "Версия", HintField = "Версия...", AutoCompleteIcon = AUTOCOMPLETE_ICON, DeleteTextFieldIcon = DELETE_ICON } };

        public ICommand AddTextField { get; }
        public ICommand RemoveTextField { get; }
    }
}
