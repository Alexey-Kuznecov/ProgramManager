using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Resources;

namespace ProgramManager.Services
{
    struct Singleton
    {
        public static object Back = null;
        public static int Count = 0;
        public static int Counter = 0;
        public static bool Status = false;
    }
}
