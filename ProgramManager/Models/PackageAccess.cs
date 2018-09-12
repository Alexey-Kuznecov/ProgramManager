using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProgramManager.Model
{
    class PackageAccess
    {
        const string DOCUMENT_NAME = "packages.xml";
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PackageAccess()
        {
            if (!File.Exists(DOCUMENT_NAME))
                PackageAccess.FormatHeadXmlDoc();
        }
        /// <summary>
        /// Создает заголовок XML документа
        /// </summary>
        public static void FormatHeadXmlDoc()
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XDocumentType("Packages", null, "Packages.dtd", null),
                new XProcessingInstruction("PackageHandler", "out-of-print"),
                new XElement("Packages")
            );
            xDoc.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Вытягивает все узлы из файла xml по их id и формирует коллекцию из выбранных элементов
        /// </summary>
        /// <returns>Возращает коллекцию эелементов <list type="PackageModel"/></returns>
        internal static List<PackageModel> GetPackages()
        {
            XElement root = XElement.Load(DOCUMENT_NAME);
            List<PackageModel> package = new List<PackageModel>();
            int id = 0;

            IEnumerable<XElement> document = from element in root.Elements("Package")
                                        where (string)element.Attribute("Id") == id.ToString()
                                        select element;

            foreach (XElement element in document) {
                package.Add(
                    new PackageModel {
                        Name = element.Element("Name").Value,
                        Author = element.Element("Author").Value,
                        Category = element.Element("Category").Value,
                        Subcategory = element.Element("Subcategory").Value,
                        Version = element.Element("Version").Value,
                        Image = element.Element("Image").Value,
                        Description = element.Element("Description").Value
                    });                
                id++;
            }     
                        
            return package;
        }
        /// <summary>
        /// Добавляет новый пакет в xml хранилище
        /// </summary>
        /// <param name="data">Коллекция объектов данных которые нужно дабавить в хранилище.</param>
        public static void AddPackage(PackageModel data)
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
            int id = GetIdLastElement();

            xDoc.Element("Packages").Add(new XElement("Package",
                            new XAttribute("Id", ++id),
                            new XElement("Name", data.Name),
                            new XElement("Author", data.Author),
                            new XElement("Version", data.Version),
                            new XElement("Category", data.Category),
                            new XElement("Subcategory", data.Subcategory),
                            new XElement("Description", data.Description),
                            new XElement("Image", new XAttribute("Source", data.Image))));
            xDoc.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Обновляет данные по индексу и сохраняет документ 
        /// </summary>
        /// <param name="id">Индекс пакета который требуется изменить.</param>
        /// <param name="data">Коллекция объектов данных</param>
        public static void UpdatePackage(int id, PackageModel data)
        {
            XElement root = XElement.Load(DOCUMENT_NAME),
                     el = root.Elements("Package").ElementAt(id);

            el.SetElementValue("Name", data.Name);
            el.SetElementValue("Author", data.Author);
            el.SetElementValue("Category", data.Category);
            el.SetElementValue("Subcategory", data.Subcategory);
            el.SetElementValue("Version", data.Version);
            el.SetElementValue("Description", data.Description);
            el.Element("Image").SetAttributeValue("Source", data.Image);

            root.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Удаляет полностью весь пакет по индексу и сохраняет документ 
        /// </summary>
        /// <param name="id">Индекс пакета который требуется удалить.</param>
        public static void RemovePackage(int id)
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
            xDoc.Root.Nodes().ElementAt(id).Remove();
            xDoc.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Находит последний элемент корневого узла "Packages" — парсит строку, 
        /// затем извлекает значение атрибута id элемента "Package".
        /// </summary>
        /// <returns>Возращает id последного элемента</returns>
        public static int GetIdLastElement()
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
            int id = 0;

            try
            {
                string str = xDoc.Root.LastNode.ToString();
                XDocument node = XDocument.Parse(str);
                id = Convert.ToInt32(node.Root.Attribute("Id").Value);
            }
            catch (NullReferenceException)
            {
                return -1;
            }
            return id;
        }
    }
}
