using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ProgramManager.Enums;

namespace ProgramManager.Models
{
    public class PackageMutator
    {
        const string DocumentName = "packages.xml";
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PackageMutator()
        {
            if (!File.Exists(DocumentName))
                PackageMutator.FormatHeadXmlDoc();
        }
        /// <summary>
        /// Создает заголовок XML документа
        /// </summary>
        private static void FormatHeadXmlDoc()
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XDocumentType("Packages", null, "Packages.dtd", null),
                new XProcessingInstruction("PackageHandler", "out-of-print"),
                new XElement("Packages")
            );
            xDoc.Save(DocumentName);
        }
        /// <summary>
        /// Новый метод для добавления пакета в xml документ, полсностью автоматизированный,
        /// работает в паре с ConvertToDictionary который позволеят исключить элементы с пустыми значениями
        /// </summary>
        /// <param name="data">Объект данных</param>
        public static void AddPackage(object data)
        {      
            // Получает индекс последнего элемента в xml документе
            int id = GetIdLastElement();
            // Метод (ConvertToDictionary) исключает поля с пустыми значениями
            Dictionary<string, string> dictionary = ConvertToDictionary(data);

            XDocument xDoc = XDocument.Load(DocumentName);

            XElement package = new XElement(new XElement("Package", new XAttribute("Id", ++id)));

            foreach (var item in dictionary)
            {
                package.Add(new XElement(item.Key, item.Value));
            }
            xDoc.Element("Packages")?.Add(package);
            xDoc.Save(DocumentName);
        }
        /// <summary>
        /// Метод выполненяет фильтрацию объекта, выберает только те поля котрорые содержат данные, иные не ввойдут в выборку.
        /// Затем создает словарь для удобного добавления элемента и его значения в xml документ.
        /// </summary>
        /// <param name="data">Объект данных для выборки</param>
        /// <returns>Коллекцию словарей <TKey>PropertyName</TKey><TValue>PropertyValue</TValue></returns>
        public static Dictionary<string, string> ConvertToDictionary(object data)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            var anomymous = (IEnumerable)data;

            if (anomymous != null)
            {
                foreach (var items in anomymous)
                {
                    PropertyInfo[] properties = items.GetType().GetProperties();

                    var key = TFieldTypes.Other;

                    foreach (var property in properties)
                    {
                        var value = property.GetValue(items);

                        if (key == TFieldTypes.Other)
                        {
                            
                        }
                        if (value is TFieldTypes)
                        {
                            key = (TFieldTypes)value; continue;
                        }
                        if (value == null) continue;

                        dictionary.Add(key.ToString(), value.ToString()); break;
                    }
                }
            }
            return dictionary;
        }
        public static Dictionary<string, string> PackageFilter2(object data)
        {
            var anomymous = (IEnumerable)data;

            
            return null;
        }
        /// <summary>
        /// Обновляет данные по индексу и сохраняет документ 
        /// </summary>
        /// <remarks>Возможна переработка метода так как является не автоматизированным и требует ручнго расширения</remarks>
        /// <param name="id">Индекс пакета который требуется изменить.</param>
        /// <param name="data">Коллекция объектов данных</param>
        public static void UpdatePackage(int id, PackageModel data)
        {
            XElement root = XElement.Load(DocumentName),
                     el = root.Elements("Package").ElementAt(id);

            el.SetElementValue("Name", data.Name);
            el.SetElementValue("Author", data.Author);
            el.SetElementValue("Category", data.Category);
            el.SetElementValue("Tag", data.TagName);
            el.SetElementValue("Version", data.Version);
            el.SetElementValue("Description", data.Description);
            el.Element("Image")?.SetAttributeValue("Source", data.Image);

            root.Save(DocumentName);
        }
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
        private static int GetIdLastElement()
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
