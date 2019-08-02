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
    public class PackagesWriter
    {
        const string DocumentName = "../../Resources/User/packages.xml";
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PackagesWriter()
        {
            if (!File.Exists(DocumentName))
                PackagesWriter.FormatHeadXmlDoc();
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

        #region Functions changing data xml data
        /// <summary>
        /// Простой метод добавляет два атрибута Id, Catergory и делегирует работу для создания нового пакета.
        /// </summary>
        /// <param name="currentPack">Данные пакета, ожидается объект типа PackageBase.</param>
        /// <param name="category">Категория в контексте которой будет создан пакет.</param>
        public static void AddPackage(PackageBase currentPack, string category)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            XElement package = FormatPackage(currentPack);
            XElement packageS = new XElement("Package");
            // Получает индекс последнего элемента в xml документе
            short id = BaseXml.GetIdLastElement();

            packageS.SetAttributeValue("Id", ++id);
            packageS.SetAttributeValue("Category", category);
            packageS.Add(package.Elements().OrderBy(p => p.Name.ToString().Substring(0, 2)));
            xDoc.Root?.Add(packageS);
            xDoc.Save(DocumentName);

            // Обновление списка пакетов
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("Update package list!");
        }
        /// <summary>
        /// Метод делегирует работу для обновления пакета.
        /// </summary>
        /// <param name="id">Уникальный номер пакета, который необходимо обновить</param>
        /// <param name="data">Данные пакета, ожидается объект типа PackageBase.</param>
        public static void UpdatePackage(PackageBase data)
        {
            XElement root = XElement.Load(DocumentName),
                     newPackage = FormatPackage(data);

            foreach (var item in root.Elements("Package"))
            {
                if (item.FirstAttribute.Value == data.Id.ToString())
                {
                    item.Elements().Remove();
                    item.Add(newPackage.Elements().OrderBy(p => p.Name.ToString().Substring(0, 2)));
                    break;
                }
            }
            root.Save(DocumentName);

            // Обновление списка пакетов
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("");
        }
        /// <summary>
        /// Удаляет полностью весь узел(package) по индексу и сохраняет документ 
        /// </summary>
        /// <param name="id">Индекс пакета который требуется удалить.</param>
        public static void RemovePackage(int id)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            var root = xDoc.Root?.Elements("Package");

            foreach (var item in root)
                if (item.FirstAttribute.Value == id.ToString())
                    item.Remove();

            xDoc.Save(DocumentName);

            // Обновление списка пакетов`
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("");
        }
        #endregion

        #region Functions additional data processing
        /// <summary>
        /// Данный метод формирует пакет на основе данных, которые содержат свойства объекта. 
        /// </summary>
        /// <param name="currentPack">Объект данных, ожидается объект типа PackageBase.</param>
        /// <returns>Возвращает готовый пакет в виде xml элементов.</returns>
        private static XElement FormatPackage(PackageBase currentPack)
        {
            XElement package = new XElement("Package");
            var properties = currentPack.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(currentPack) == null) continue;
                if (property.PropertyType.Name == "String")
                    package.Add(new XElement(property.Name, property.GetValue(currentPack)));
                if (property.Name == "FieldList")
                    AddUserfield(package, currentPack);
                if (property.Name == "TagList")
                    AddTag(package, currentPack);
                if (property.Name == "Icon")
                    AddIcon(package, currentPack);
                if (property.Name == "Image")
                    AddImage(package, currentPack);
            }
            // Группирует элементы с одинаковыми именами в один узел и добавляет "List" к имени нового узла.
            return package.CreatingNestedElements().PostfixElementName();
        }
        /// <summary>
        /// Метод добавляет данные иконки: геометрия, цвет и фон иконки.
        /// </summary>
        /// <param name="currentPackXml">Пакет в который будут добавлены новые данные.</param>
        /// <param name="currentPack">Объект данных, ожидается объект типа PackageBase.</param>
        public static void AddIcon(XElement currentPackXml, PackageBase currentPack)
        {
            if (currentPack.Icon.Name != null)
            {
                var iconData = "../../Resources/User/packageIcons.xml";
                var root = XElement.Load(iconData);

                var check = from icon in root.Elements()
                    where icon.Attribute("Id")?.Value == currentPack.Id.ToString()
                    select icon;
                check.Remove();

                root.Add(new XElement("Icon", new XAttribute("Id", currentPack.Id),
                    new XAttribute("Name", currentPack.Icon.Name),
                    new XAttribute("Foreground", currentPack.Icon.FgroundColor),
                    new XAttribute("Background", currentPack.Icon.BgroundColor),
                    new XAttribute("Path", currentPack.Icon.Path.Data.ToString().Replace(',', '.').Replace(';', ','))));

                root.Save(iconData);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPackXml"></param>
        /// <param name="currentPack"></param>
        public static void AddImage(XElement currentPackXml, PackageBase currentPack)
        {
            var imgcover = currentPack.Image;
            string uri = "../Resources/User/Images/" + imgcover?.Name + ".png";
            currentPackXml.Add(new XElement("Image", new XAttribute("Source", uri)));
        }
        /// <summary>
        /// Метод формирует xml элементы на основе данных пользовательских полей (Имя, значение). 
        /// </summary>
        /// <param name="currentPackXml">Текущий пакет.</param>
        /// <param name="currentPack">Объект данных, ожидается объект типа PackageBase.</param>
        private static void AddUserfield(XElement currentPackXml, PackageBase currentPack)
        {
            foreach (var item in currentPack.FieldList)
            {
                currentPackXml?.Add(new XElement(FieldTypes.Userfield.ToString(),
                    new XAttribute("Label", PackageFieldConverter.Dictionary.Single(p => p.Key == item.Key).Value), item.Value));
            }
        }
        /// <summary>
        /// Метод создает xml элемент на основе элементов списка тегов. 
        /// </summary>
        /// <param name="currentPackXml">Текущий пакет.</param>
        /// <param name="currentPack">Объект данных, ожидается объект типа PackageBase.</param>
        private static void AddTag(XElement currentPackXml, PackageBase currentPack)
        {
            foreach (var value in currentPack.TagList)
                currentPackXml.Add(new XElement("Tag", value));
        }
        #endregion
    }
}
