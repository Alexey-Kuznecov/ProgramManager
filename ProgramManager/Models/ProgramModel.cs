using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Models
{
    public class ProgramModel : PackageBase
    {
        public string License { get; set; }
        public string CompanySite { get; set; }
        public string SerialKey { get; set; }

        public override void GetPackages()
        {
            base.GetPackages();
        }
    }
}
