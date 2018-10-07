using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Models;

namespace ProgramManager.Contracts
{
    interface IConnector
    {
        void PackageChanged(PackageModel obj);     
    }
}
