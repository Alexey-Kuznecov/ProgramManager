using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Models.NewModel;
using ProgramManager.Filters;

namespace ProgramManager.Models
{
    public class ProgramModel : PackageBase
    {
        public string License { get; set; }
        public string CompanySite { get; set; }
        public string SerialKey { get; set; }

        public override void AddPackage()
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
            xDoc.Element("Packages")
                ?.Add(new XElement("Package", 
                    new XAttribute("Id", Category),
                    new XElement("Name", Name ?? ""),
                    new XElement("Author", Author ?? ""),
                    new XElement("Version", Version ?? ""),
                    new XElement("Source", Source ?? ""),
                    new XElement("License", License ?? ""),
                    new XElement("CompanySite", CompanySite ?? ""),
                    new XElement("SerialKey", SerialKey ?? ""),
                    new XElement("HashSumm", HashSumm ?? ""),
                    new XElement("Description", Description ?? ""),
                    new XElement("Image", new XAttribute("Source", Image ?? ""))));
            xDoc.Save(DOCUMENT_NAME);
        }

        public override List<ProgramModel> GetPackages(CategoryModel category)
        {
            throw new NotImplementedException();
        }
        public ProgramModel()
        {
            Tag = new List<TagModel>()
            {
                new TagModel() { Name = "3D Редакторы"  },
                new TagModel() { Name = "IDE" },
                new TagModel() { Name = "Антивирусы" },
                new TagModel() { Name = "Браузеры" },
                new TagModel() { Name = "Меседжеры" },
                new TagModel() { Name = "Редакторы кода" },
                new TagModel() { Name = "Инструменты разработчика" },
                new TagModel() { Name = "Офисные" },
                new TagModel() { Name = "Оптимизатры" },
                new TagModel() { Name = "Органазеры" },
            };
        }
    }
    public class ProgramAccess
    {
        private const string DOCUMENT_NAME = "packages.xml";

        public static List<ProgramModel> GetPackages(CategoryModel category)
        {
            XElement root = XElement.Load(DOCUMENT_NAME);
            List<ProgramModel> package = new List<ProgramModel>();
            FilterField<ProgramModel> propNotIsEmpty = new FilterField<ProgramModel>();
            int index = 0;

            IEnumerable<XElement> document = from element in root.Elements("Package")
                where (string)element.Attribute("Category") == category.Name
                select element;

            foreach (XElement element in document)
            {
                package.Add(
                    new ProgramModel
                    {
                        Name = element.Element("Name")?.Value,
                        Author = element.Element("Author")?.Value,
                        Version = element.Element("Version")?.Value,
                        Image = element.Element("Image")?.FirstAttribute.Value,
                        Description = element.Element("Description")?.Value,
                        License = element.Element("License")?.Value,
                        TagContain = element.Element("Tag")?.Value,
                        SerialKey = element.Element("SerialKey")?.Value,
                        CompanySite = element.Element("CompanySite")?.Value,
                        Category = element?.LastAttribute.Value,
                        IndexCategory = 0
            });
                package[index].Datails = package[index].Properties = propNotIsEmpty.Filter(package[index++]);


            }
            return package;
        }
    }
}
