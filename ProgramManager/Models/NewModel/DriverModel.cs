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
            Tag = new List<TagModel>()
            {
                new TagModel() { Name = "Звуковая карта" },
                new TagModel() { Name = "Аксессуары" },
                new TagModel() { Name = "Звуковая карта" },
            };
        }
    }
    public class DriverAccess
    {
        private const string DOCUMENT_NAME = "packages.xml";

        public static List<DriverModel> GetPackages(CategoryModel category)
        {
            XElement root = XElement.Load(DOCUMENT_NAME);
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
                        IndexCategory = 1
                    });
                package[index].Datails = package[index].Properties = propNotIsEmpty.Filter(package[index++]);
            }
            return package;
        }
    }
}
