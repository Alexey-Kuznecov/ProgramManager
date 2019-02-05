using System;

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
