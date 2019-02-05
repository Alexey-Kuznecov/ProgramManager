using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ProgramManager.Models.PackageModel
{
    public class ProgramModel : PackageBase
    {
        protected override string Status { get; } = "Программы";
        public string License { get; set; }
        public string CompanySite { get; set; }
        public string SerialKey { get; set; }
        public string Copyright { get; set; }

        public ProgramModel()
        {
            LoadItem += LoadMenuItem;
        }
    }
}
