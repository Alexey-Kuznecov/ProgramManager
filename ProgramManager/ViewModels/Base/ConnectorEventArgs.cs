using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Models;

namespace ProgramManager.ViewModels
{
    public class ConnectorEventArgs : EventArgs
    {
        public ConnectorEventArgs(PackageModel package)
        {
            Package = package;
        }
        public PackageModel Package { get; private set; }
    }
}
