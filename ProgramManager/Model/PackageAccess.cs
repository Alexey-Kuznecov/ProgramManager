using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;

namespace ProgramManager.Model
{
    class PackageAccess
    {
        const string DOCUMENT_NAME = "packages.xml";

        public IEnumerable<PackageModel> Packages { get; private set; }

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
        /// Вытягивает все узлы из файла xml по их id и формирует коллекцю из выбранных элементов
        /// </summary>
        /// <returns>Возращает коллекцию эелементов <list type="PackageModel"/></returns>
        internal static List<PackageModel> GetPackages()
        {
            XElement root = XElement.Load(DOCUMENT_NAME);
            List<PackageModel> package = new List<PackageModel>();
            int id = 0;

            IEnumerable<XElement> doc = from el in root.Elements("Package")
                                        where (string)el.Attribute("Id") == id.ToString()
                                        select el;

            foreach (XElement el in doc)
            {
                package.Add(
                    new PackageModel
                    {
                        Name = el.Element("Name").Value,
                        Author = el.Element("Author").Value,
                        Category = el.Element("Category").Value,
                        Subcategory = el.Element("Subcategory").Value,
                        Version = el.Element("Version").Value,
                        Image = el.Element("Image").Value,
                        Description = el.Element("Description").Value
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
            catch (NullReferenceException ex)
            {
                return -1;
            }
            return id;
        }

        public PackageAccess()
        {
            if (!File.Exists("packages.xml"))
                PackageAccess.FormatHeadXmlDoc();

            foreach (PackageModel pack in Packages)
            {
                PackageModel newData = new PackageModel()
                {
                    Name = pack.Name,
                    Image = "User/Images/19572.jpg",
                    Author = pack.Author,
                    Category = pack.Category,
                    Subcategory = pack.Subcategory,
                    Version = pack.Version,
                    Description = pack.Description
                };

                //PackageAccess.AddPackage(newData);
            }
        }
    }
}
