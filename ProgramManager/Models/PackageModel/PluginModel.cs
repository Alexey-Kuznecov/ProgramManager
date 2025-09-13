<<<<<<< HEAD
﻿using System;

namespace ProgramManager.Models.PackageModel
{
    public class PluginModel : PackageBase
    {
        protected override string CatName { get; }
        public string Appointment { get; set; }

        public PluginModel()
        {
            CatName = "Плагины";
            LoadItem += LoadMenuItem;
        }
    }
}
=======
﻿using System;

namespace ProgramManager.Models.PackageModel
{
    public class PluginModel : PackageBase
    {
        protected override string Status { get; } = "Плагины";
        public string Appointment { get; set; }

        public PluginModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
