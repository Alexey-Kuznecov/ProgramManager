using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Resources;

namespace ProgramManager.Services
{
    static class Singleton
    {
        public static object _back = null;
        public static int _count = 0;
        public static int _counter = 0;
        public static bool _status = false;
    }
}
