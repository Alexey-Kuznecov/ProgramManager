using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Models.PackageModels;

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
        /// <param name="category">Категория в контексте которой будет создан пакет.</param>
        public static void AddPackage(object data, string category)
        {      
            // Получает индекс последнего элемента в xml документе
            int id = BaseXml.GetIdLastElement();
            // Метод (ConvertToDictionary) формирует данные в соответствии словаря
            Dictionary<string, string> dictionary = ConvertToDictionary(data);
            XDocument xDoc = XDocument.Load(DocumentName);
            // Имя поля пользователя
            string fieldName = FieldTypes.Userfield.ToString();
        
            XElement package = new XElement(new XElement("Package", new XAttribute("Id", ++id), new XAttribute("Category", category)));
            XElement userfield = new XElement(fieldName);

            foreach (var item in dictionary)
            {
                if (item.Key.Contains(fieldName))
                {
                    userfield.Add(new XElement(item.Key, new XAttribute("Name", FieldConverter.Dictionary.Single(p => p.Key == item.Key).Value), item.Value)); continue;
                }
                package.Add(new XElement(item.Key, item.Value));
            }
            package.Add(userfield);
            xDoc.Element("Packages")?.Add(package);           
            xDoc.Save(DocumentName);
        }
        /// <summary>
        /// Метод создает словарь для удобного добавления элемента и его значения в xml документ.
        /// </summary>
        /// <param name="data">Объект данных, должен содержать коллекцию объектов, где первое свойство
        /// будет играть роль ключа — а второе значения.</param>
        /// <returns>Коллекцию словарей <TKey>FirstProperty</TKey><TValue>SecondProperty</TValue></returns>
        public static Dictionary<string, string> ConvertToDictionary(object data)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            var anomymous = (IEnumerable)data;

            foreach (var items in anomymous)
            {
                PropertyInfo[] properties = items.GetType().GetProperties();

                string key = null;

                foreach (var property in properties)
                {
                    var value = property.GetValue(items);

                    if (properties[0].Name == property.Name)
                    {
                        key = value.ToString(); continue;
                    }
                    if (value == null || key == null) continue;

                    dictionary.Add(key, value.ToString()); break;
                }
            }
            return dictionary;
        }
        /// <summary>
        /// Обновляет данные по индексу и сохраняет документ 
        /// </summary>
        /// <remarks>Возможна переработка метода так как является не автоматизированным и требует ручнго расширения</remarks>
        /// <param name="id">Индекс пакета который требуется изменить.</param>
        /// <param name="data">Коллекция объектов данных</param>
        public static void UpdatePackage(int id, PackageBase data)
        {
            XElement root = XElement.Load(DocumentName),
                     el = root.Elements("Package").ElementAt(id);

            el.SetElementValue("Name", data.Name);
            el.SetElementValue("Author", data.Author);
            el.SetElementValue("Category", data.Category);
            el.SetElementValue("Tag", data.TagOne);
            el.SetElementValue("Version", data.Version);
            el.SetElementValue("Description", data.Description);
            el.Element("Image")?.SetAttributeValue("Source", data.Image);

            root.Save(DocumentName);
        }
    }
}
