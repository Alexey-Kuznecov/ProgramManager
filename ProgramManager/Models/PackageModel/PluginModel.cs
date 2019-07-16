using System;

namespace ProgramManager.Models.PackageModel
{
    public class PluginModel : PackageBase
    {
        protected override string CatName { get; } = "Плагины";
        public string Appointment { get; set; }

        public PluginModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
