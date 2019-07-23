using System.Collections.Generic;
using System.Windows;
using ProgramManager.Test;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Services
{
    class CommonProperties : PropertiesChanged
    {
        public static List<string> _iconNames;
        /// <summary>
        /// Stores the collection icon names.
        /// </summary>
        public static List<string> IconNames
        {
            get { return _iconNames; }
            set
            {
                _iconNames = value;
            }
        }
    }
}
