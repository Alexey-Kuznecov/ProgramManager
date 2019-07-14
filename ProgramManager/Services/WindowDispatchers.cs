using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Views;

namespace ProgramManager.Services
{
    static class WindowDispatchers
    {
        public static IconsEditor IconsEditor;

        public static void IconsEditorClose()
        {
            IconsEditor.Close();
        }
    }
}
