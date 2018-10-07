using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Models;

namespace ProgramManager.Contracts
{
    public class BaseConnector
    {
        public BaseConnector()
        {
            
        }
        public void OnPackageChanged(PackageModel package)
        {
            PackageChanged?.Invoke(this, new PackageEventArgs(package));
        }
        public EventHandler<PackageEventArgs> PackageChanged { get; set; }
    }
}
