<<<<<<< HEAD
﻿using System;

namespace ProgramManager.Models.PackageModel
{
    public class ModModel : PackageBase
    {
        protected override string CatName { get; }
        public string Association { get; set; }

        public ModModel()
        {
            CatName = "Моды";
            LoadItem += LoadMenuItem;
        }
    }
}
=======
﻿using System;

namespace ProgramManager.Models.PackageModel
{
    public class ModModel : PackageBase
    {
        protected override string Status { get; } = "Моды";
        public string Association { get; set; }

        public ModModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
