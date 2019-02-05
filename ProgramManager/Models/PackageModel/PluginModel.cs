using System;

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
