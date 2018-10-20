using System;

namespace ProgramManager.Models.PackageModel
{
    public class DriverModel : PackageBase
    {
        public string Puma { get; set; }
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }
    }
}
