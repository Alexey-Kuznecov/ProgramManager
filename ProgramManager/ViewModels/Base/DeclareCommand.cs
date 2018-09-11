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
        private void FixPackState()
        {
            List<object> pack = new List<object>();
            pack.Add(_packages);        
        }
        private void Update() { }
        private void Insert() { }
    }
}
