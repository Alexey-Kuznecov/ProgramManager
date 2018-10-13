using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProgramManager.Models.NewModel;

namespace ProgramManager.Models
{
    public class PluginModel : PackageBase
    {
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }
        public PluginModel()
        {
            //Tag = new List<WrapPackages>()
            //{
            //    new WrapPackages() { Name = "Встраиваемые" },
            //    new WrapPackages() { Name = "Для фотошопа" },
            //    new WrapPackages() { Name = "Просмоторщики" },
            //    new WrapPackages() { Name = "Для сжатия" },
            //    new WrapPackages() { Name = "Для Sublime Text" },
            //    new WrapPackages() { Name = "Для Total Commander" },
            //};
        }
    }
}
