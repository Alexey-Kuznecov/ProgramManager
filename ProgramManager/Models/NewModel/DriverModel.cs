using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Filters;

namespace ProgramManager.Models.NewModel
{
    public class DriverModel : PackageBase
    {
        public override void AddPackage()
        {
            throw new NotImplementedException();
        }

        public override List<ProgramModel> GetPackages(CategoryModel category)
        {
            throw new NotImplementedException();
        }

        public DriverModel()
        {
            Tag = new List<WrapPackages>()
            {
                new WrapPackages() { Name = "Звуковая карта" },
                new WrapPackages() { Name = "Аксессуары" },
                new WrapPackages() { Name = "Сетевой адапер" },
                new WrapPackages() { Name = "Видео карта" },
            };
        }
    }
    public class DriverAccess
    {
        private const string DocumentName = "DriverPackages.xml";

        public static List<DriverModel> GetPackages(CategoryModel category)
        {
            XElement root = XElement.Load(DocumentName);
            List<DriverModel> package = new List<DriverModel>();
            FilterField<DriverModel> propNotIsEmpty = new FilterField<DriverModel>();
            int index = 0;

            IEnumerable<XElement> document = from element in root.Elements("Package")
                where (string)element.Attribute("Category") == category.Name
                select element;

            foreach (XElement element in document)
            {
                package.Add(
                    new DriverModel
                    {
                        Name = element.Element("Name")?.Value,
                        Author = element.Element("Author")?.Value,
                        Version = element.Element("Version")?.Value,
                        Image = element.Element("Image")?.FirstAttribute.Value,
                        Description = element.Element("Description")?.Value,
                        Category = element?.LastAttribute.Value,
                    });
                package[index].Datails = propNotIsEmpty.Filter(package[index++]);
            }
            return package;
        }
    }
}
