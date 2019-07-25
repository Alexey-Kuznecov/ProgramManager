using System;
using System.Collections.Generic;
using System.Globalization;
using ProgramManager.Plugins;

namespace ProgramManager.Converters
{
    //[DebuggerStepThrough]
    class ResourceNameValidation : BaseConverter<ResourceNameValidation>
    {
        private static string _temp;
        private static List<string> _storeName;
        /// <summary>
        /// Store collection names.
        /// </summary>
        public static List<string> StoreName
        {
            get { return _storeName; }
            set
            {
                _storeName = value;
            }
        }
        /// <summary>
        /// If the StoreName property contains name, converter returns false, 
        /// then the action button to be disabled.
        /// </summary>
        /// <param name="value">Name or isEnabled property value of action button.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameters transfer command.</param>
        /// <param name="culture">Provides information about language and params</param>
        /// <returns>
        /// Returns true. If value is boolean and store not contain given name.This boolean value transfer to action button for it isEnable property.
        /// If value is string just returns name. This a string get in the field window.
        /// </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isBoolean = value is bool;
            bool isString = value is string;

            #region This code part responsible for accessibility of action button...

            if (isBoolean)
            {
                if (StoreName != null && !StoreName.Contains(_temp))
                {
                    return true;
                }
                return false;
            }

            #endregion

            #region This part of code is responsible for enter name in field ...

            if (isString)
            {
                if (StoreName == null)
                {
                    StoreName = new List<string> { (string) value };
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
            // Load full list icon names.
            if (CommonProperties.IconNames != null)
                _storeName = CommonProperties.IconNames;
            return value;
        }
    }
}
