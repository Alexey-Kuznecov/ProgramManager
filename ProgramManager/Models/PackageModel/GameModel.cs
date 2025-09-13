<<<<<<< HEAD
﻿using System;

namespace ProgramManager.Models.PackageModel
{
    public class GameModel : PackageBase
    {
        protected override string CatName { get; }
        public string CheatCode { get; set; }

        public GameModel()
        {
            CatName = "Игры";
            LoadItem += LoadMenuItem;
        }
    }
}
=======
﻿using System;

namespace ProgramManager.Models.PackageModel
{
    public class GameModel : PackageBase
    {
        protected override string Status { get; } = "Игры";
        public string CheatCode { get; set; }

        public GameModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
