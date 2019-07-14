using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Resources;

namespace ProgramManager.Services
{
    static class Synchronizer
    {
        public static CancelChangeIcon IconLoad;
        public delegate void CancelChangeIcon(Icon obj);
    }
}
