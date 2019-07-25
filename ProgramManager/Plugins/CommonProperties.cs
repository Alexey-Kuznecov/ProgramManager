using System.Collections.Generic;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Plugins
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
