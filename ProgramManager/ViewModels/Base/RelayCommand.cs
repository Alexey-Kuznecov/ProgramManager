using System;
using System.Windows;
using System.Windows.Input;

namespace ProgramManager.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool> _canExecute2;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action command, Func<bool> canExecute = null)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            _canExecute2 = canExecute;
            _command = command;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;

            if (_canExecute != null)
                MessageBox.Show(_canExecute.ToString());

        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        //public void Execute(object parameter)
        //{
        //    _command();
        //}

        //public bool CanExecute(object parameter)
        //{
        //    return _canExecute == null || _canExecute();
        //}

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
