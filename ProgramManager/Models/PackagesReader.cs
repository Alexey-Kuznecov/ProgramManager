using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using ProgramManager.Enums;
using ProgramManager.Models.PackageModel;
using ProgramManager.Converters;
using ProgramManager.Associations;
using ProgramManager.Plugins;
using ProgramManager.Services;

namespace ProgramManager.Models
{
    /// Этот класс является обобщенным поэтому каждый потомок класса PackageBase будет совместим с данным классом, свойства производных классов
    /// будут гарантированно проинициализированы значениями из элементов xml документа, если имена свойств совпадают с именами xml элементов.
    /// По результатам работы класса, создается коллекция объектов типа List Т, свойства объектов которых будут инициализированы значениями элементов xml документа.
    public class PackagesReader<T> where T : PackageBase, new()
    {
        private const string DocumentName = "../../Resources/User/packages.xml";
        /// <summary>
        /// Базовый метод фильтрует и извлекает данные из xml документа, на основе данных формирует новый объект.
        /// Вызывает некоторые вспомогательные методы для боллее тонкой обработки данных и объекта. 
        /// </summary>
        /// <param name="category">Принимает объект, свойство(Name) которого равняется текущей категории, необходимой для усвловия фильтрации.</param>
        /// <returns>Возвращает коллекцию объектов производных от базового класса PackageBase.</returns>
        public static List<T> GetPackages(CategoryModel category)
        {
            List<T> packages = new List<T>();
            XElement root = XElement.Load(DocumentName);
            int index = 0;

            // Запрос с фильтрацией данных. 
            IEnumerable<XElement> document = from element in root.Elements("Package")
                                             where (string)element.Attribute("Category") == category.Name
                                             select element;

            // Формирования нового объекта на основе данных xml документа.
            foreach (XElement element in document)
            {
                string image = element.Element(FieldTypes.Image.ToString())?.FirstAttribute.Value;
                
                // Инициализация свойств из базового класса
                packages.Add(new T
                {
                    Id = int.Parse(element.FirstAttribute.Value),
                    Name = element.Element(FieldTypes.Name.ToString())?.Value,
                    Author = element.Element(FieldTypes.Author.ToString())?.Value,
                    Version = element.Element(FieldTypes.Version.ToString())?.Value,
                    Description = element.Element(FieldTypes.Description.ToString())?.Value,
                    TagOne = element.Element(FieldTypes.Tag.ToString())?.Value,
                    HashSumm = element.Element(FieldTypes.HashSumm.ToString())?.Value,
                    // Вызов метода для создания коллекции тегов, если пакет имеет более одного тега
                    TagList = GetTagsList(element),
                    Category = element.LastAttribute.Value,
                    TextField = SetFieldValue(element),
                    Icon = GetIcons(element, element.FirstAttribute.Value)
                });
                // Вызов метода для инициализации свойств производного класса.
                SetValueDeclaredProperties(packages, element, index);
                // Вызов метода фильтрации полей с пустыми значениями данного объекта.
                // packages[index].Datails = propNotIsEmpty.Filter(packages[index++]);             
            }
            return packages;
        }
        private static IconModel GetIcons(XElement element, string id)
        {
            var root = XElement.Load("../../Resources/User/packageIcons.xml");
            var queryIcons = from icon in root.Elements() where icon.FirstAttribute.Value == id select icon;

            foreach (var icon in queryIcons)
            {
                var iconModel = new IconModel(
                    icon.Attribute("Name")?.Value, 
                    icon.Attribute("Foreground")?.Value,
                    icon.Attribute("Background")?.Value,
                    icon.LastAttribute.Value
                );
                return iconModel;
            }
            // If user don't set custom icon then to the package be asigned a icon default.
            return new IconModel(SetIcons(element.LastAttribute.Value), DataPackageEditor.IconForeDefault,
                DataPackageEditor.IconBackDefault);
        }
        /// <summary>
        /// Устанавливает иконку взависемости от категории.
        /// </summary>
        /// <param name="category">Получает текущею категорию.</param>
        /// <returns>Возвращает имя иконки.</returns>
        private static string SetIcons(string category)
        {
            CategoryDictionary cateDict = new CategoryDictionary();

            if (category == cateDict.GetValue(Categories.Programs))
                return "ProgramIcon";
            if (category == cateDict.GetValue(Categories.Drivers))
                return "DriversIcon";
            if (category == cateDict.GetValue(Categories.Mods))
                return "ModsIcon";
            if (category == cateDict.GetValue(Categories.Plugins))
                return "PluginsIcon";
            return "GamesIcon";
        }
        /// <summary>
        /// Вспомогательный метод для получения массива тегов(текст), так как пакеты могут иметь больше одного тега.
        /// Метод находит элемент <TagList></TagList> и формирует массив на основе содержимого данного элемента.
        /// </summary>
        /// <param name="node">Контекст текущего родительского элемента.</param>
        /// <returns>Коллекцию строковых элеменов(тегов), принадлежащих текущему пакету</returns>
        private static List<string> GetTagsList(XElement node)
        {
            IEnumerable<XElement> elements = node.Elements("TagList");
            List<string> tags = new List<string>();

            foreach (var element in elements.Elements(FieldTypes.Tag.ToString())) tags.Add(element.Value);

            return tags;
        }
        /// <summary>
        /// Устанавливает значения свойствам объявленным в производном классе.
        /// </summary>
        /// <param name="packages">Коллекция объектов(пактов)</param>
        /// <param name="element">Контекст текущего родительского элемента xml документа.</param>
        /// <param name="index">Текущий индекс элемента коллекции.</param>
        private static void SetValueDeclaredProperties(List<T> packages, XElement element, int index)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {
                property.SetValue(packages[index], element.Element(property.Name)?.Value);
            }
        }
        /// <summary>
        /// Adding user field to property of collection. 
        /// </summary>
        /// <param name="node">Accents a current item.</param>
        /// <returns>Return the collection of object is TextFieldModel type.</returns>
        private static List<TextFieldModel> SetFieldValue(XElement node)
        {
            List<TextFieldModel> textField = new List<TextFieldModel>();

            IEnumerable<XElement> elements = node.Elements();

            foreach (var element in elements)
            {
                sbyte count = 0;

                if (element.HasElements && element.Name == FieldTypes.Userfield + "List")
                {
                    foreach (var child in element.Elements())
                    {
                        count++;

                        textField.Add(new TextFieldModel
                        {
                            FieldValue = child.Value,
                            Types = child.Name.ToString() + count,
                            Label = child.FirstAttribute.Value,
                            Hint = child.FirstAttribute.Value
                        });
                    }
                }
                if (!element.HasElements && element.Name == FieldTypes.Userfield.ToString())
                {
                    textField.Add(new TextFieldModel
                    {
                        FieldValue = element.Value,
                        Types = element.Name.ToString(),
                        Label = element.LastAttribute.Value,
                        Hint = element.LastAttribute.Value
                    });
                }
                // TODO: Найти элегантное решение взамен этого куска кода.
                // Добавление всех полей кроме основных и пользовательских.
                if (!element.HasElements && element.Name != "Name" && element.Name != "Description" 
                    && element.Name != "Tag" && element.Name != "Image" && element.Name != "Userfield" && element.Name != "Icon")
                {
                    textField.Add(new TextFieldModel
                    {
                        FieldValue = element.Value,
                        Types = element.Name.ToString()
                    });
                }
            }
            return SetFieldLabel(textField);
        }
        /// <summary>
        /// Метод инициализирует поля Label объекта TextFieldModel значениями словаря PackageFieldConverter.Dictionary 
        /// для вывода их в панель информации о пакете.
        /// </summary>
        /// <param name="textFields">Список полей выбранного пакета.</param>
        /// <returns>Список объектов типа TextFieldModel с проинициализованными полями Label.</returns>
        private static List<TextFieldModel> SetFieldLabel(List<TextFieldModel> textFields)
        {
            for (var i = 0; i < textFields.Count; i++)
                foreach (var item in PackageFieldConverter.Dictionary.Where(d => d.Key == textFields[i].Types))
                    textFields[i].Label = item.Value;

            return textFields;
        }
    }
}
