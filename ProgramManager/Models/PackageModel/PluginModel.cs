using System;

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
