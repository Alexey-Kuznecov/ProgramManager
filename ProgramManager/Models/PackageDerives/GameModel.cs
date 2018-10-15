using System;
using System.Collections.Generic;

namespace ProgramManager.Models.PackageDerives
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
