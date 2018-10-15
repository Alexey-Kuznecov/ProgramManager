using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ProgramManager.Converters
{
    public class NumberToStringConverterExtension : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new NumberToStringConverterExtension();
            return _converter;
        }

        private static NumberToStringConverterExtension _converter = null;
    }
}
