using System;

namespace ProgramManager.Models.PackageModel
{
    public class GameModel : PackageBase
    {
        protected override string CatName { get; } = "Игры";
        public string CheatCode { get; set; }

        public GameModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
