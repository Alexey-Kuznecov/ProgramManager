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
        public ConnectorEventArgs(object package)
        {
            Package = package;
        }
        public object Package { get; private set; }
    }
}
