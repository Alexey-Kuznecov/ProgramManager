using ProgramManager.Converters;
using ProgramManager.Models.PackageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Services
{
    public class ProcessManager
    {
        public void InitialModule()
        {
            CategoryModel catr = new CategoryModel();
            catr.SetMenuItem();
        }
    }
}
