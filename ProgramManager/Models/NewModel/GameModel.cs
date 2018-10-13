using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProgramManager.Models.NewModel;

namespace ProgramManager.Models
{
    public class GameModel : PackageBase
    {
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }
        public GameModel()
        {
            Tag = new List<WrapPackage>()
            {
                new WrapPackage() { Name = "RPG" },
                new WrapPackage() { Name = "Стратегии" },
                new WrapPackage() { Name = "Shooter" },
            };
        }
    }
}
