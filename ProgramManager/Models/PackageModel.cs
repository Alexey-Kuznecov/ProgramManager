using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Models
{
    public class PackageModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Label { get; set; } 
        public string Field { get; set; }
    }
}
