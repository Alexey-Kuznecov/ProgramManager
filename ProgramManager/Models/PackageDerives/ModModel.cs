using System;
using System.Collections.Generic;

namespace ProgramManager.Models.PackageDerives
{
    public class ModModel : PackageBase
    {
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }
        public ModModel()
        {
            Tag = new List<WrapPackage>()
            {
                new WrapPackage() { Name = "Все" },
                new WrapPackage() { Name = "Оружие" },
                new WrapPackage() { Name = "Персонажи" },
                new WrapPackage() { Name = "Прически" },
                new WrapPackage() { Name = "Ретекстур" },
                new WrapPackage() { Name = "DLC" },
                new WrapPackage() { Name = "Ивентарь" },
                new WrapPackage() { Name = "Разное" },
            };
        }
    }
}
