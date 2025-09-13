using System;

namespace ProgramManager.Models.PackageModel
{
    public class DriverModel : PackageBase
    {
        protected override string CatName { get; }
        public string Vendor { get; set; }
        public string TypeDevice { get; set; }

        public DriverModel()
        {
            CatName = "Драйвера";
            LoadItem += LoadMenuItem;
        }
    }
}
