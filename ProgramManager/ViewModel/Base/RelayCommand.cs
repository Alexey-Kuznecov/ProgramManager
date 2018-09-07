using System;
using System.Windows;
using System.Windows.Input;

namespace ProgramManager.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly MainViewModel _viewModel;
        private readonly Action _action;

        public RelayCommand(MainViewModel viewModel, Action command)
        {
            _viewModel = viewModel;
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
