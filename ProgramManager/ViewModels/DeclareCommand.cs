using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel
    {
        public void FixStatePackage()
        {            
            if (_filterPackages == null || _filterPackages == "")
                _storage = Packages;
            else
                Packages = _storage;
        }
        private void Update() { }
        private void Insert() { }
    }
}
