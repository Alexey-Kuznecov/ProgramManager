using ProgramManager.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ProgramManager.Converters
{
    public class KeywordToTextConverter : IValueConverter
    {
        private static IDictionary<Keywords, string> _descriptions = new Dictionary<Keywords, string>
        {
            { Keywords.All, "Все" },
            { Keywords.Chosen, "Избранные" },
            { Keywords.Found, "Найденные" },
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Keywords))
                return null;
            var keyword = (Keywords)value;
            if (!_descriptions.ContainsKey(keyword))
                return null;
            return _descriptions[keyword];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
