using System;

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
