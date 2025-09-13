using System;
<<<<<<< HEAD
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ProgramManager.ViewModels.Base
{
    [DebuggerStepThrough]
=======
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.ViewModels
{
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    public class PropertiesChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> action)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(action);
            return true;
        }
<<<<<<< HEAD
=======

>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            RaisePropertyChanged(propertyName);
        }
<<<<<<< HEAD
=======

>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }
<<<<<<< HEAD
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
=======

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        }
    }
}