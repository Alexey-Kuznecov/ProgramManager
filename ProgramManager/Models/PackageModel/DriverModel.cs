using System;

namespace ProgramManager.Models.PackageModel
{
    public class DriverModel : PackageBase
    {
        public string Vender { get; set; }
        public string TypeDevice { get; set; }
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }

    }
}
