using System;
using System.Linq;
using System.Xml.Linq;

namespace ProgramManager.Models
{
    public class BaseXml
    {
        const string DocumentName = "../../Resources/User/packages.xml";
        /// <summary>
        /// Находит последний элемент корневого узла "Packages" — парсит строку, 
        /// затем извлекает значение атрибута id элемента "Package".
        /// </summary>
        /// <returns>Возращает индекс последного элемента(пакета)</returns>
        public static short GetIdLastElement()
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            short id = 0;

            try
            {
                if (xDoc.Root != null)
                {
                    string str = xDoc.Root.LastNode.ToString();
                    XDocument node = XDocument.Parse(str);
                    if (node.Root != null) id = Convert.ToInt16(node.Root.Attribute("Id")?.Value);
                }
            }
            catch (NullReferenceException)
            {
                return -1;
            }
            return id;
        }
        /// <summary>
        /// Обновляет данные по индексу и сохраняет документ 
        /// </summary>
        /// <remarks>Возможна переработка метода так как является не автоматизированным и требует ручнго расширения</remarks>
        /// <param name="id">Индекс пакета который требуется изменить.</param>
        /// <param name="data">Коллекция объектов данных</param>
        //public static void UpdatePackage2(int id, PackageBase data)
        //{
        //    XElement root = XElement.Load(DocumentName),
        //             el = root.Elements("Package").ElementAt(id),
        //             newField = new XElement(el.Element("UserfieldList"));

        //    el?.Element("UserfieldList").RemoveAll();


        //    el.SetElementValue("Name", data.Name);
        //    el.SetElementValue("Author", data.Author);
        //    el.SetElementValue("Category", data.Category);
        //    el.SetElementValue("Tag", data.TagOne);
        //    el.SetElementValue("Version", data.Version);
        //    el.SetElementValue("Description", data.Description);
        //    el.Element("Image")?.SetAttributeValue("Source", data.Image);

        //    root.Save(DocumentName);
        //}
    }
}
