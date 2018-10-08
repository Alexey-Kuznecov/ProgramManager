using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ProgramManager.Models
{

    public class PackageAccess
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
        private static void FormatHeadXmlDoc()
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
        /// <remarks>Возможна переработка метода так как является не автоматизированным и требует ручнго расширения</remarks>
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
                        Name = element.Element("Name")?.Value,
                        Author = element.Element("Author")?.Value,
                        Category = element.Element("Category")?.Value,
                        TagName = element.Element("Tag")?.Value,
                        Version = element.Element("Version")?.Value,
                        Image = element.Element("Image")?.Value,
                        Description = element.Element("Description")?.Value
                    });                
                id++;
            }                             
            return package;
        }
        /// <summary>
        /// Новый метод для добавления пакета в xml документ, полсностью автоматизированный,
        /// работает в паре с PackageFilter который позволеят исключить элементы с пустыми значениями
        /// </summary>
        /// <param name="data">Объект данных</param>
        public void AddPackageObsolete(PackageModel data)
        {
            // Получает индекс последнего элемента в xml документе
            int id = GetIdLastElement();
            // Метод (PackageFilter) исключает поля с пустыми значениями
            Dictionary<string, string> dictionary = PackageFilter(data);

            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);

            XElement package = new XElement(new XElement("Package", new XAttribute("Id", ++id)));

            foreach (var item in dictionary)
            {
                package.Add(new XElement(item.Key, item.Value));
            }

            xDoc.Element("Packages")?.Add(package);
            xDoc.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Метод выполненяет фильтрацию объекта, выберает только те поля котрорые содержат данные, иные не ввойдут в выборку.
        /// Затем создает словарь для удобного добавления елемента и его значения в xml документ.
        /// </summary>
        /// <param name="data">Объект данных для выборки</param>
        /// <returns>Коллекцию словарей <TKey>PropertyName</TKey><TValue>PropertyValue</TValue></returns>
        public Dictionary<string, string> PackageFilter(PackageModel data)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Type classType = typeof(PackageModel);

            PropertyInfo[] properties = classType.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(data) != null)
                {
                    dictionary.Add(property.Name, data.GetType().GetProperty(property.Name)?.GetValue(data).ToString());
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
        public static void UpdatePackage(int id, PackageModel data)
        {
            XElement root = XElement.Load(DOCUMENT_NAME),
                     el = root.Elements("Package").ElementAt(id);

            el.SetElementValue("Name", data.Name);
            el.SetElementValue("Author", data.Author);
            el.SetElementValue("Category", data.Category);
            el.SetElementValue("Tag", data.TagName);
            el.SetElementValue("Version", data.Version);
            el.SetElementValue("Description", data.Description);
            el.Element("Image")?.SetAttributeValue("Source", data.Image);

            root.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Удаляет полностью весь узел(package) по индексу и сохраняет документ 
        /// </summary>
        /// <param name="id">Индекс пакета который требуется удалить.</param>
        public static void RemovePackage(int id)
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
            xDoc.Root.Nodes().ElementAt(id).Remove();
            xDoc.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Добавляет новый пакет в xml хранилище, не идеален так может содержать пустые значения элементов
        /// в будущем возможно переработка данного метода.
        /// </summary>
        /// <remarks>Метод не используется программой, оставил на крайний случай если вдруг пригодится</remarks>
        /// <param name="data">Коллекция объектов данных которые нужно дабавить в хранилище.</param>
        public void AddPackage(PackageModel data)
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
            int id = GetIdLastElement();
            if (data != null)
                xDoc.Element("Packages")
                    ?.Add(new XElement("Package",
                        new XAttribute("Id", ++id),
                        new XElement("Name", data.Name ?? ""),
                        new XElement("Author", data.Author ?? ""),
                        new XElement("Version", data.Version ?? ""),
                        new XElement("Category", data.Category ?? ""),
                        new XElement("Tag", data.TagName ?? ""),
                        new XElement("License", data.License ?? ""),
                        new XElement("CompanySite", data.CompanySite ?? ""),
                        new XElement("CompanySite", data.Copyright ?? ""),
                        new XElement("SerialKey", data.SerialKey ?? ""),
                        new XElement("Source", data.Source ?? ""),
                        new XElement("HashSumm", data.HashSumm ?? ""),
                        new XElement("Description", data.Description ?? ""),
                        new XElement("Image", new XAttribute("Source", data.Image ?? ""))));
            xDoc.Save(DOCUMENT_NAME);
        }
        /// <summary>
        /// Находит последний элемент корневого узла "Packages" — парсит строку, 
        /// затем извлекает значение атрибута id элемента "Package".
        /// </summary>
        /// <returns>Возращает индекс последного элемента(пакета)</returns>
        private static int GetIdLastElement()
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
