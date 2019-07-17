using System;

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
