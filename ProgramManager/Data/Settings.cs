using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Plugins;

namespace ProgramManager.Data
{
    struct Settings
    {
        public static Themes CurrentTheme { get; set; }
        public static List<Plugin> Plugins { get; set; }
    }
}
