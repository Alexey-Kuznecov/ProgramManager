using System;

namespace ProgramManager.Models.PackageModel
{
    public class DriverModel : PackageBase
    {
        protected override string Status { get; } = "Драйвера";
        public string Vendor { get; set; }
        public string TypeDevice { get; set; }

        public DriverModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
