using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Models.Func;
using ProgramManager.Models.PackageModel;
using ProgramManager.Services;

namespace ProgramManager.Models
{
    public class PackageAccess
    {
        const string DocumentName = "packages.xml";
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
        /// Новый метод для добавления пакета в xml документ, полсностью автоматизированный,
        /// работает в паре с ConvertToDictionary который позволеят исключить элементы с пустыми значениями
        /// </summary>
        /// <param name="data">Объект данных, ожидается объект типа PackageBase.</param>
        /// <param name="category">Категория в контексте которой будет создан пакет.</param>
        public static void AddPackage(object data, string category)
        {
            // Получает индекс последнего элемента в xml документе
            int id = BaseXml.GetIdLastElement();
            XDocument xDoc = XDocument.Load(DocumentName);
            XElement package = new XElement("Package");
            // Метод (ConvertToDictionary) формирует данные в соответствии словаря
            Dictionary<object, string> dictionary = data.ConvertToDictionary();

            foreach (var item in dictionary)
            {
                var properties = item.Key.GetType().GetProperties();
                string name = null;

                foreach (var property in properties)
                {
                    var key = property.GetValue(item.Key).ToString();

                    if (property.Name == "Name")
                        name = key;

                    if (properties.Length == 1)
                        package.Add(new XElement(key, item.Value));           

                    // Объявление атрибутов тегов, так же незабудь указать свойство анонимного типа в методе FuncHelper.ConvertToDictionary
                    if (property.Name == "Id")
                        if (name != null) package.Add(new XElement(name, new XAttribute("Id", key), item.Value));
                    if (property.Name == "FieldName")
                        if (name != null) package.Element(name)?.SetAttributeValue("FieldName", key);
                }
            }
            // 1. Удалиние цифр кототрые содержат имена элементов (необходые для уникальности элемента).
            // 2. Группировака элементов с одинаковыми именами в один элемент.
            var newPack = package.TrimElementName().CreatingNestedElements().PostfixElementName();
            newPack.SetAttributeValue("Id", ++id);
            newPack.SetAttributeValue("Category", category);

            xDoc.Element("Packages")?.Add(newPack);         
            xDoc.Save(DocumentName);
            EventAggregate connector = new EventAggregate();
            connector.OnLoadPackage("Xml updated");
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
