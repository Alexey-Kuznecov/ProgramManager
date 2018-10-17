using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Enums;

namespace ProgramManager.ViewModels
{
    public class InputNameViewModel
    {
        public string Name { get; set; }
        public ICommand InputName => new RelayCommand(obj =>
        {
            Messenger.Default.Send(new InputNameViewModel { Name = Name });
        });      
    }
}
