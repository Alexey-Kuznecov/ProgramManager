using System;
using System.Collections.Generic;

namespace ProgramManager.Models.PackageDerives
{
    public class DriverModel : PackageBase
    {
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }
        public DriverModel()
        {
            Tag = new List<WrapPackage>()
            {
                new WrapPackage() { Name = "Звуковая карта" },
                new WrapPackage() { Name = "Аксессуары" },
                new WrapPackage() { Name = "Сетевой адапер" },
                new WrapPackage() { Name = "Видео карта" },
            };
        } 
    }
}
