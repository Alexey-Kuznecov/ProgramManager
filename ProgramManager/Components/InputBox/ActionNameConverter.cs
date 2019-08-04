using System;
using System.Globalization;
using System.Windows.Data;
using ProgramManager.Converters;

namespace ProgramManager.Components.InputBox
{
    public class ActionNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Associations.ActionsDictionary ad = new Associations.ActionsDictionary();
            if (value != null)
                return ad.GetValue((Enums.Actions)value);
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Associations.ActionsDictionary ad = new Associations.ActionsDictionary();
            if (value != null)
                return ad.GetKey((string)value);
            return null;
        }
    }
}
