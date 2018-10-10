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
        public override List<ProgramModel> GetPackages(CategoryModel category)
        {
            XElement root = XElement.Load(DOCUMENT_NAME);
            List<ProgramModel> package = new List<ProgramModel>();

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
                        Image = element.Element("Image")?.Value,
                        Source = element.Element("Source")?.Value,
                        Description = element.Element("Description")?.Value,
                        TagContain = element.Element("Tag")?.Value,
                    });
            }
            return package;
        }
        public PluginModel()
        {
            Tag = new List<TagModel>()
            {
                new TagModel() { Name = "Встраиваемые" },
                new TagModel() { Name = "Для фотошопа" },
                new TagModel() { Name = "Просмоторщики" },
                new TagModel() { Name = "Для сжатия" },
                new TagModel() { Name = "Для Sublime Text" },
                new TagModel() { Name = "Для Total Commander" },
            };
        }
    }
}
