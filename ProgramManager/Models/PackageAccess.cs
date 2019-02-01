using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Enums;
using ProgramManager.Models.Func;
using ProgramManager.Models.PackageModel;
using ProgramManager.Services;
using ProgramManager.Converters;

namespace ProgramManager.Models
{
    public class PackageAccess
    {
        const string DocumentName = "../../Resources/User/packages.xml";
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PackageAccess()
        {
            if (!File.Exists(DocumentName))
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
            xDoc.Save(DocumentName);
        }
        /// <summary>
        /// Простой метод добавляет два атрибута Id, Catergory и делегирует работу для создания нового пакета.
        /// </summary>
        /// <param name="data">Объект данных, ожидается объект типа PackageBase.</param>
        /// <param name="category">Категория в контексте которой будет создан пакет.</param>
        public static void AddPackage(PackageBase data, string category)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            XElement package = FormatPackage(data);
            // Получает индекс последнего элемента в xml документе
            short id = BaseXml.GetIdLastElement(); 

            package.SetAttributeValue("Id", ++id);
            package.SetAttributeValue("Category", category);
            xDoc.Element("Packages")?.Add(package);         
            xDoc.Save(DocumentName);
            
            // Обновление списка пакетов
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("");
        }
        /// <summary>
        /// Метод делегирует работу для обновления пакета.
        /// </summary>
        /// <param name="id">Уникальный номер пакета, который необходимо обновить</param>
        /// <param name="data">Объект данных, ожидается объект типа PackageBase.</param>
        public static void UpdatePackage(PackageBase data)
        {
            XElement root = XElement.Load(DocumentName),
                     package = root.Elements("Package").ElementAt(data.Id),
                     newPackage = FormatPackage(data);
            
            package.Elements().Remove();
            package.Add(newPackage.Elements());
            root.Save(DocumentName);
            
            // Обновление списка пакетов
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("");

        }
        /// <summary>
        /// Данный метод формирует пакет на основе данных, которые содержат свойства объекта. 
        /// </summary>
        /// <param name="data">Объект данных, ожидается объект типа PackageBase.</param>
        /// <returns>Возвращает готовый пакет в виде xml элементов.</returns>
        private static XElement FormatPackage(PackageBase data)
        {
            XElement package = new XElement("Package");
            var properties = data.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(data) == null) continue;

                if (property.PropertyType.Name == "String")
                    package.Add(new XElement(property.Name, property.GetValue(data)));

                if (property.Name == "FieldList")
                    AddUserfield(package, data);

                if (property.Name == "TagList")
                    AddTag(package, data);
            }
            // Группирует элементы с одинаковыми именами в один узел и добавляет "List" к имени нового узла.
            return package.CreatingNestedElements().PostfixElementName();
        }
        /// <summary>
        /// Метод формирует xml элементы на основе данных пользовательских полей (Имя, значение). 
        /// </summary>
        /// <param name="currentPack">Текущий пакет.</param>
        /// <param name="data">Объект данных, ожидается объект типа PackageBase.</param>
        private static void AddUserfield(XElement currentPack, PackageBase data)
        {
            foreach (var item in data.FieldList)
            {
                currentPack?.Add(new XElement(FieldTypes.Userfield.ToString(),
                    new XAttribute("Label", FieldConverter.Dictionary.Single(p => p.Key == item.Key).Value), item.Value));
            }           
        }
        /// <summary>
        /// Метод создает xml элемент на основе элементов списка тегов. 
        /// </summary>
        /// <param name="currentPack">Текущий пакет.</param>
        /// <param name="data">Объект данных, ожидается объект типа PackageBase.</param>
        private static void AddTag(XElement currentPack, PackageBase data)
        {
            foreach (var value in data.TagList)
                currentPack.Add(new XElement("Tag", value));
        }
        /// <summary>
        /// Удаляет полностью весь узел(package) по индексу и сохраняет документ 
        /// </summary>
        /// <param name="id">Индекс пакета который требуется удалить.</param>
        public static void RemovePackage(int id)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            var root = xDoc.Root.Elements("Package");

            foreach (var item in root)
                if (item.FirstAttribute.Value == id.ToString())
                    item.Remove();

            xDoc.Save(DocumentName);

            // Обновление списка пакетов`
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("");
        }
    }
}
