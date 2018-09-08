using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
