using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using ProgramManager.Enums;
using ProgramManager.MarkupExtensions;
using ProgramManager.Models;

namespace ProgramManager.Converters
{
    public class FieldConverter : ConvertorBase<FieldConverter>
    {
        private static readonly IDictionary<string, string> _descriptions = new Dictionary<string, string>
        {
            { "Author", "Автор" },
            { "Version", "Версия" },
            { "Title", "Имя" },
            { "Description", "Описание" },
            { "License", "Лицензия" },
            { "Source", "Источник" },
            { "SerialKey", "Лицензионный ключ" },
            { "CompanySite", "Официальный сайт" },
            { "Copyright", "Авторские права" },
            { "HashSumm", "Хеш-сумма" },
        };

        public static IDictionary<string, string> Descriptions
        {
            get { return _descriptions; }
            set
            {
                if (value != null)
                {
                    _descriptions.Add(value.Keys.ToString(), value.Values.ToString());
                }               
            }
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TextFieldModel))
                return null;
            var field = (TextFieldModel)value;
            if (!Descriptions.ContainsKey(field.Types))
                return null;
            return Descriptions[field.Types];
        }
    }
}
