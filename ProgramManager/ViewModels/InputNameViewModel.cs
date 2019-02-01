using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.ViewModels
{
    public class InputNameViewModel
    {
        private static InputName _inputName;
        public string Name { get; set; }
        public ICommand InputName => new RelayCommand(obj => Messenger.Default.Send(new InputNameViewModel { Name = Name }));
        public ICommand Cancel => new RelayCommand(obj =>
        {
            _inputName = obj as InputName;
            if (_inputName != null) _inputName.Close();
        });
    }
}
