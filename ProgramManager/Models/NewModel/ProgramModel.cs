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
    }
    public class ProgramAccess<T> where T : PackageBase, new()
    {
        private const string DocumentName = "ProgramPackages.xml";

        public static List<T> GetPackages(CategoryModel category)
        {
            FilterField<T> propNotIsEmpty = new FilterField<T>();
            List<T> packages = new List<T>();
            XElement root = XElement.Load(DocumentName);
            int index = 0;
            
            // Запрос с фильтрацией данных. 
            IEnumerable<XElement> document = from element in root.Elements("Package")
                where (string)element.Attribute("Category") == category.Name
                select element;
            // Формирования нового объекта на основе данных xml документа.
            foreach (XElement element in document)
            {
                packages.Add( new T {
                        Name = element.Element("Name")?.Value,
                        Author = element.Element("Author")?.Value,
                        Version = element.Element("Version")?.Value,
                        Image = element.Element("Image")?.FirstAttribute.Value,
                        Description = element.Element("Description")?.Value,
                        TagOne = element.Element("Tag")?.Value,
                        // Вызов метода для создания коллекции тегов, если пакет имеет более одного тега
                        TagList = GetTags(element),
                        Category = element?.LastAttribute.Value });
                // Вызом тетода фильтрации полей с пустыми значениями данного объекта.
                packages[index].Datails = propNotIsEmpty.Filter(packages[index++]);
            }
            return packages;
        }
        /// <summary>
        /// Вспомогательный метод для получения массива тегов(текст), так как пакеты могут иметь больше одного тега.
        /// Метод находит елемент <TagList></TagList> и формирует массив на основе содержимого данного элемента.
        /// </summary>
        /// <param name="node">Контекст текущего родительсткго элемента.</param>
        /// <returns>Коллекцию строковых элеменов(тегов), пренадлижащих текущему пакету</returns>
        private static List<string> GetTags(XElement node)
        {
            IEnumerable<XElement> elements = node.Elements("TagList");
            List<string> tags = new List<string>();

            foreach (var element in elements.Elements("Tag")) tags.Add(element.Value);

            return tags;
        }
    }
}
