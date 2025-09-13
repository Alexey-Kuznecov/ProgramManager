<<<<<<< HEAD
﻿using System;

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
=======
﻿using System;

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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
