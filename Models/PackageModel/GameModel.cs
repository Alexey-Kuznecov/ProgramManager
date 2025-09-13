using System;

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
