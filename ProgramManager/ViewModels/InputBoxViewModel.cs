using System.Windows;
using System.Windows.Input;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    class InputBoxViewModel : PropertiesChanged
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
        public ICommand Action { get; set; }
        public ICommand Cancel => new RelayCommand(obj =>
        {
            Window win = (Window) obj;
            win.Close();
        });
    }
}
