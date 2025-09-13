<<<<<<< HEAD
﻿

namespace ProgramManager.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

=======
﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ProgramManager.Converters
{
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    public class ConverterExtension : MarkupExtension, IValueConverter
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
                _converter = new ConverterExtension();
            return _converter;
        }

        private static ConverterExtension _converter = null;
    }
}
