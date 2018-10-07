using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Models;

namespace ProgramManager.Contracts
{
    public class PackageEventArgs : EventArgs
    {
        public PackageEventArgs(PackageModel package)
        {
            Package = package;
        }
        public PackageModel Package { get; private set; }
    }
}
