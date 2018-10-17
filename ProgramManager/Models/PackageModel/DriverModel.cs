using System;
using System.Collections.Generic;
using ProgramManager.Models.PackageModels;

namespace ProgramManager.Models.PackageModels
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
