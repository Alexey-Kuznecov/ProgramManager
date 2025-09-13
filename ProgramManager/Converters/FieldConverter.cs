using System;
using System.Collections.Generic;
using System.Globalization;
using ProgramManager.MarkupExtensions;
using ProgramManager.Models;
using ProgramManager.Models.PackageModel;
using System.Collections;

namespace ProgramManager.Converters
{
    public class FieldConverter : ConverterBase<FieldConverter>, IEnumerable
    {
        private static readonly IDictionary<string, string> _dictionary = new Dictionary<string, string>
        {
            { "Author", "Автор" },
            { "Name", "Имя" },
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

        public static IDictionary<string, string> Dictionary
        {
            get { return _dictionary; }
            set
            {
                if (value != null)
                {
                    _dictionary.Add(value.Keys.ToString(), value.Values.ToString());
                }               
            }
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TextFieldModel))
                return null;
            var field = (TextFieldModel)value;
            if (!Dictionary.ContainsKey(field.Types))
                return null;
            return Dictionary[field.Types];
        }
        /// <summary>
        /// Реализалация интерфеса IEnumerator
        /// </summary>
        /// <returns>Возвращает пакет</returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var package in Dictionary)
            {
                yield return package.Key;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
