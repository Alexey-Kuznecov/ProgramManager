
namespace ProgramManager.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Models.PackageModel;

    /// <summary>
    /// The package field converter.
    /// </summary>
    public class PackageFieldConverter : BaseConverter<PackageFieldConverter>, IEnumerable
    {
        /// <summary>
        /// The _dictionary.
        /// </summary>
        private static readonly IDictionary<string, string> DictionaryAss = new Dictionary<string, string>
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

        /// <summary>
        /// Gets or sets the dictionary.
        /// </summary>
        public static IDictionary<string, string> Dictionary
        {
            get => DictionaryAss;
            set
            {
                if (value != null)
                {
                    DictionaryAss.Add(value.Keys.ToString(), value.Values.ToString());
                }               
            }
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value"> The <paramref name="value"/>. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TextFieldModel))
            {
                return null;
            }

            var field = (TextFieldModel)value;

            return !Dictionary.ContainsKey(field.Types) ? null : Dictionary[field.Types];
        }

        /// <summary>
        /// IEnumerator Interface Implementation.
        /// </summary>
        /// <returns> Returns the package. </returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var package in Dictionary)
            {
                yield return package.Key;
            }
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns> The <see cref="IEnumerator"/>. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
