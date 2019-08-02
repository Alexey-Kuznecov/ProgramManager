using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace ProgramManager.Models
{
    [DebuggerStepThrough]
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
    }
}
