using System;
using System.Windows;
using System.Windows.Input;

namespace ProgramManager.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _action;

        public RelayCommand(Action command)
        {
            _action = command;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            _action();
        }
    }
}
