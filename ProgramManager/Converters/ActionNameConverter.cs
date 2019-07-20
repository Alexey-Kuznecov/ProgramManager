using System;
using System.Globalization;
using ProgramManager.Associations;
using ProgramManager.Enums;

namespace ProgramManager.Converters
{
    class ActionNameConverter : BaseConverter<ActionNameConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ActionsDictionary ad = new ActionsDictionary();
            if (value != null)
                return ad.GetValue((Actions)value);
            return null;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ActionsDictionary ad = new ActionsDictionary();
            if (value != null)
                return ad.GetKey((string)value);
            return null;
        }
    }
}
