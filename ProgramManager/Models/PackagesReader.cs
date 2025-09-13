<<<<<<< HEAD
﻿

namespace ProgramManager.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Associations;
    using Converters;
    using Enums;
    using PackageModel;
    using Plugins;

    /// <summary>
    /// This class is generic, therefore each derived class from the <c>base</c> class
    /// PackageBase will be compatible with <c>this</c> class, the properties of
    /// derived classes will be guaranteed to be initialized with values from
    /// the xml elements of the document if the property names match the names
    /// of the xml elements. According to the results of the class, a collection
    /// of objects of type List T is created, the properties of which will be
    /// initialized with the values of the xml elements of the document.
    /// </summary>
    /// <typeparam name="T">
    /// Expected the derived class from of class <c>base</c> PackageBase. For example:
    /// (ProgramModel, GameModel, DriverModel)
    /// </typeparam>
    public class PackagesReader<T> where T : PackageBase, new()
    {
        /// <summary>
        /// The document name.
        /// </summary>
        private const string DocumentName = "../../Resources/User/packages.xml";

        /// <summary>
        /// File location the collection of icons.
        /// </summary>
        private const string CollectionIcons = "../../Resources/User/packageIcons.xml";

        /// <summary>
        /// The main method filters and extracts data from the xml document, forms a new <c>object</c> based on the data.
        /// Calls some helper methods for more fine-grained data and <c>object</c> processing.
        /// </summary>
        /// <param name="category">Accepts an <c>object</c> whose (Name) property equals the current <c>category</c> necessary for filtering conditions.</param>
        /// <returns>Returns a collection of objects derived from the <c>base</c> class PackageBase.</returns>
        public static List<T> GetPackages(CategoryModel category)
        {
            List<T> packages = new List<T>();
            XElement root = XElement.Load(DocumentName);
            int index = 0;
            
            // Query with data filtering.
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ProgramManager.Enums;
using ProgramManager.Models.PackageModel;
using ProgramManager.Converters;
using ProgramManager.Dict;

namespace ProgramManager.Models
{
    /// Этот класс является обобщенным поэтому каждый потомок класса PackageBase будет совместим с данным классом, свойства производных классов
    /// будут гарантированно проинициализированы значениями из элементов xml документа, если имена свойств совпадают с именами xml элементов.
    /// По результатам работы класса создается коллекция объектов типа List Т, свойства объектов которых будут инициализированы значениями элементов xml документа.
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
            // FilterProperties<T> propNotIsEmpty = new FilterProperties<T>();
            XElement root = XElement.Load(DocumentName);
            int index = 0;

            // Запрос с фильтрацией данных. 
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            IEnumerable<XElement> document = from element in root.Elements("Package")
                                             where (string)element.Attribute("Category") == category.Name
                                             select element;

<<<<<<< HEAD
            // Formation of a new object based on xml document data.
=======
            // Формирования нового объекта на основе данных xml документа.
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            foreach (XElement element in document)
            {
                string image = element.Element(FieldTypes.Image.ToString())?.FirstAttribute.Value;

<<<<<<< HEAD
                // Initialization of properties from the base class
=======
                // Инициализация свойств из базового класса
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                packages.Add(new T
                {
                    Id = int.Parse(element.FirstAttribute.Value),
                    Name = element.Element(FieldTypes.Name.ToString())?.Value,
                    Author = element.Element(FieldTypes.Author.ToString())?.Value,
                    Version = element.Element(FieldTypes.Version.ToString())?.Value,
                    Description = element.Element(FieldTypes.Description.ToString())?.Value,
                    TagOne = element.Element(FieldTypes.Tag.ToString())?.Value,
                    HashSumm = element.Element(FieldTypes.HashSumm.ToString())?.Value,
<<<<<<< HEAD
                    Image = new ImageCover(element.Element("Image")?.Attribute("Source")?.Value),

                    // Call a method to create a tag collection if the package has more than one tag
                    TagList = GetTagsList(element),
                    Category = element.LastAttribute.Value,
                    TextField = SetFieldValue(element),
                    
                    // If user don't set custom icon then to the package be asigned a icon default.
                    Icon = GetIcons(element.FirstAttribute.Value) 
                           ?? new IconModel(SetIcons(element.LastAttribute.Value), InteractonPackageEditor.IconForeDefault, InteractonPackageEditor.IconBackDefault)
                });

                // A method call to initialize the properties of a derived class.
                SetValueDeclaredProperties(packages, element, index);
            }

            return packages;
        }

        /// <summary>
        /// Sets values to properties declared in a derived class.
        /// </summary>
        /// <param name="packages">Collection of objects (<c>packages</c>).</param>
        /// <param name="element">The context of the current parent xml <c>element</c> of the document.</param>
        /// <param name="index">The current <c>index</c> of the collection item.</param>
=======
                    Image = image == null ? SetIcons(image, element.LastAttribute.Value) : image,
                    // Вызов метода для создания коллекции тегов, если пакет имеет более одного тега
                    TagList = GetTagsList(element),
                    Category = element.LastAttribute.Value,
                    TextField = SetFieldValue(element)
                });
                // Вызов метода для инициализации свойств производного класса.
                SetValueDeclaredProperties(packages, element, index);
                // Вызов метода фильтрации полей с пустыми значениями данного объекта.
                // packages[index].Datails = propNotIsEmpty.Filter(packages[index++]);             
            }
            return packages;
        }
        private static string SetIcons(string iconPath, string category)
        {
            CategoryDict cateDict = new CategoryDict();
            string uri = @"..\Resources\User\Images\";

            if (category == cateDict.GetValue(Categories.Programs))
                return uri + Categories.Programs.ToString() + ".png";
            else if (category == cateDict.GetValue(Categories.Drivers))
                return uri + Categories.Drivers.ToString() + ".png";
            else if (category == cateDict.GetValue(Categories.Mods))
                return uri + Categories.Mods.ToString() + ".png";
            else if (category == cateDict.GetValue(Categories.Plugins))
                return uri + Categories.Plugins.ToString() + ".png";
            else
                return uri + Categories.Games.ToString() + ".png";
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        private static void SetValueDeclaredProperties(List<T> packages, XElement element, int index)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {
                property.SetValue(packages[index], element.Element(property.Name)?.Value);
            }
        }
<<<<<<< HEAD

        /// <summary>
        /// A helper method for getting an array of tags (text), since packages can have more than one tag.
        /// The method finds the <TagList> </TagList> element and forms an array based on the contents of <c>this</c> element.
        /// </summary>
        /// <param name="node"> The context of the current parent. </param>
        /// <returns> A collection of string elements (tags) belonging to the current package. </returns>
        private static List<string> GetTagsList(XElement node)
        {
            IEnumerable<XElement> elements = node.Elements("TagList");
            List<string> tags = new List<string>();

            foreach (var element in elements.Elements(FieldTypes.Tag.ToString()))
            {
                tags.Add(element.Value);
            }

            return tags;
        }

        #region Methods for handling package icons.

        /// <summary>
        /// Loads custom icons for package. 
        /// </summary>
        /// <param name="id">Package <c>id</c> to search a icon.</param>
        /// <returns> Returns an <c>object</c> of type <see cref="IconModel"/>. </returns>
        private static IconModel GetIcons(string id)
        {
            var root = XElement.Load(CollectionIcons);
            var queryIcons = from icon in root.Elements() where icon.FirstAttribute.Value == id select icon;

            foreach (var icon in queryIcons)
            {
                var iconModel = new IconModel(
                    icon.Attribute("Name")?.Value,
                    icon.Attribute("Foreground")?.Value,
                    icon.Attribute("Background")?.Value,
                    icon.LastAttribute.Value);

                return iconModel;
            }

            return null;
        }

        /// <summary>
        /// Sets the icon depending on the <c>category</c>.
        /// </summary>
        /// <param name="category">Gets the current <c>category</c>.</param>
        /// <returns>Returns the name of the icon.</returns>
        private static string SetIcons(string category)
        {
            CategoryDictionary cateDict = new CategoryDictionary();

            if (category == cateDict.GetValue(Categories.Programs))
            {
                return "ProgramIcon";
            }

            if (category == cateDict.GetValue(Categories.Drivers))
            {
                return "DriversIcon";
            }

            if (category == cateDict.GetValue(Categories.Mods))
            {
                return "ModsIcon";
            }

            if (category == cateDict.GetValue(Categories.Plugins))
            {
                return "PluginsIcon";
            }

            return "GamesIcon";
        }
        
        #endregion

        #region Methods for handling package fields.
        
=======
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        /// <summary>
        /// Adding user field to property of collection. 
        /// </summary>
        /// <param name="node">Accents a current item.</param>
<<<<<<< HEAD
        /// <returns>Return the collection of <c>object</c> is TextFieldModel type.</returns>
        private static List<TextFieldModel> SetFieldValue(XElement node)
        {
            List<TextFieldModel> textField = new List<TextFieldModel>();
=======
        /// <returns>Return the collection of object is TextFieldModel type.</returns>
        private static List<TextFieldModel> SetFieldValue(XElement node)
        {
            List<TextFieldModel> textField = new List<TextFieldModel>();

>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
            IEnumerable<XElement> elements = node.Elements();

            foreach (var element in elements)
            {
                sbyte count = 0;
<<<<<<< HEAD
                // This part of the method will work if the package contains more than one user field.
                if (element.HasElements && element.Name == FieldTypes.Userfield + "List")
                    foreach (var child in element.Elements())
                    {
                        count++;
                        textField.Add(new TextFieldModel
                        {
                            FieldValue = child.Value,
                            Types = child.Name.ToString() + count,
=======

                if (element.HasElements && element.Name == FieldTypes.Userfield.ToString() + "List")
                {
                    foreach (var child in element.Elements())
                    {
                        count++;

                        textField.Add(new TextFieldModel()
                        {
                            FieldValue = child.Value,
                            Types = child.Name.ToString() + count.ToString(),
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                            Label = child.FirstAttribute.Value,
                            Hint = child.FirstAttribute.Value
                        });
                    }
<<<<<<< HEAD
                // This part of the method will work if the package contains single user field.
                if (!element.HasElements && element.Name == FieldTypes.Userfield.ToString())
                    textField.Add(new TextFieldModel
=======
                }
                if (!element.HasElements && element.Name == FieldTypes.Userfield.ToString())
                {
                    textField.Add(new TextFieldModel()
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                    {
                        FieldValue = element.Value,
                        Types = element.Name.ToString(),
                        Label = element.LastAttribute.Value,
                        Hint = element.LastAttribute.Value
                    });
<<<<<<< HEAD
                // TODO: Найти элегантное решение взамен этого куска кода.
                // Добавление всех полей кроме основных и пользовательских.
                if (!element.HasElements && element.Name != "Name" && element.Name != "Description"
                    && element.Name != "Tag" && element.Name != "Image" && element.Name != "Userfield" && element.Name != "Icon")
                    textField.Add(new TextFieldModel
=======
                }
                // Добавление всех полей кроме основных и пользовательских.
                if (!element.HasElements && element.Name != "Name" && element.Name != "Description" && element.Name != "Tag" && element.Name != "Image" && element.Name != "Userfield")
                {
                    textField.Add(new TextFieldModel()
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                    {
                        FieldValue = element.Value,
                        Types = element.Name.ToString()
                    });
<<<<<<< HEAD
            }
            return SetFieldLabel(textField);
        }

        /// <summary>
        /// The method initializes the Label fields of the TextFieldModel <c>object</c> with the values of the <see cref="PackageFieldConverter.Dictionary"/> dictionary
        /// to display them in the package information panel.
        /// </summary>
        /// <param name="textFields"> List of fields of the selected package. </param>
        /// <returns> List of objects of type TextFieldModel with initialized Label fields. </returns>
        private static List<TextFieldModel> SetFieldLabel(List<TextFieldModel> textFields)
        {
            foreach (var t in textFields)
            {
                foreach (var item in PackageFieldConverter.Dictionary.Where(d => d.Key == t.Types))
                {
                    t.Label = item.Value;
                }
            }

            return textFields;
        }
        
        #endregion
=======
                }
            }
            return SetFieldLabel(textField);
        }
        /// <summary>
        /// Метод инициализирует поля Label объекта TextFieldModel значениями словаря FieldConverter.Dictionary 
        /// для вывода их в панель информации о пакете.
        /// </summary>
        /// <param name="textFields">Список полей выбранного пакета.</param>
        /// <returns>Список объектов типа TextFieldModel с проинициализованными полями Label.</returns>
        private static List<TextFieldModel> SetFieldLabel(List<TextFieldModel> textFields)
        {
            for (var i = 0; i < textFields.Count; i++)
                foreach (var item in FieldConverter.Dictionary.Where(d => d.Key == textFields[i].Types))
                    textFields[i].Label = item.Value;

            return textFields;
        }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
    }
}
