using System.Xml.Linq;

namespace ProgramManager.Models.PackageModel
{
    public class ProgramModel : PackageBase
    {
        public string License { get; set; }
        public string CompanySite { get; set; }
        public string SerialKey { get; set; }

        public override void AddPackage()
        {
            
        }
    }
}
