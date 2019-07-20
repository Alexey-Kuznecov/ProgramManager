using System;
using System.Collections.Generic;
using System.Globalization;
using ProgramManager.Services;

namespace ProgramManager.Converters
{
    class ResourceNameValidation : BaseConverter<ResourceNameValidation>
    {
        private static string _temp;
        public static List<string> Store { get; set; }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isBoolean = value is bool;
            bool isString = value is string;

            #region Эта часть кода отвечает за доступность кнопки Action...

            if (isBoolean)
            {
                if (Store != null && !Store.Contains(_temp))
                {
                    return true;
                }
                return false;
            }

            #endregion

            #region Эта текст который пользователь вводит в поля Name...

            if (isString)
            {
                if (Store == null)
                {
                    Store = new List<string> { (string) value };
                    _temp = (string)value;
                }
                return value;
            }

            #endregion

            return value;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _temp = (string)value;
            return value;
        }
    }
}
