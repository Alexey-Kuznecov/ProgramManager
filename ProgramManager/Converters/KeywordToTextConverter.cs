using ProgramManager.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
<<<<<<< HEAD

namespace ProgramManager.Converters
{
    public class KeywordToTextConverter : BaseConverter<KeywordToTextConverter>
=======
using ProgramManager.MarkupExtensions;

namespace ProgramManager.Converters
{
    public class KeywordToTextConverter : ConverterBase<KeywordToTextConverter>
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    {
        private static readonly IDictionary<Keywords, string> Descriptions = new Dictionary<Keywords, string>
        {
            { Keywords.All, "Все" },
            { Keywords.Chosen, "Избранные" },
            { Keywords.Found, "Найденные" },
        };

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Keywords))
                return null;
            var keyword = (Keywords)value;
            if (!Descriptions.ContainsKey(keyword))
                return null;
            return Descriptions[keyword];
        }
<<<<<<< HEAD
=======

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    }
}
