using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace ProgramManager.ViewModels
{
    public class InputNameViewModelcs
    {
        public string Name { get; set; }
        public ICommand InputName => new RelayCommand(obj =>
        {
            Messenger.Default.Send(new InfoMessage { Name = Name });
        });      
    }
}
