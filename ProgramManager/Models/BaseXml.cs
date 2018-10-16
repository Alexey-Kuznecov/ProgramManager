using System;
using System.Linq;
using System.Xml.Linq;

namespace ProgramManager.Models
{
    public class BaseXml
    {
        const string DocumentName = "packages.xml";
        /// <summary>
        /// Удаляет полностью весь узел(package) по индексу и сохраняет документ 
        /// </summary>
        /// <param name="id">Индекс пакета который требуется удалить.</param>
        public static void RemovePackage(int id)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            xDoc.Root?.Nodes().ElementAt(id).Remove();
            xDoc.Save(DocumentName);
        }
        /// <summary>
        /// Находит последний элемент корневого узла "Packages" — парсит строку, 
        /// затем извлекает значение атрибута id элемента "Package".
        /// </summary>
        /// <returns>Возращает индекс последного элемента(пакета)</returns>
        public static int GetIdLastElement()
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            int id = 0;

            try
            {
                if (xDoc.Root != null)
                {
                    string str = xDoc.Root.LastNode.ToString();
                    XDocument node = XDocument.Parse(str);
                    if (node.Root != null) id = Convert.ToInt32(node.Root.Attribute("Id")?.Value);
                }
            }
            catch (NullReferenceException)
            {
                return -1;
            }
            return id;
        }
    }
}
