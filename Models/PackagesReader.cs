

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
            IEnumerable<XElement> document = from element in root.Elements("Package")
                                             where (string)element.Attribute("Category") == category.Name
                                             select element;

            // Formation of a new object based on xml document data.
            foreach (XElement element in document)
            {
                string image = element.Element(FieldTypes.Image.ToString())?.FirstAttribute.Value;

                // Initialization of properties from the base class
                packages.Add(new T
                {
                    Id = int.Parse(element.FirstAttribute.Value),
                    Name = element.Element(FieldTypes.Name.ToString())?.Value,
                    Author = element.Element(FieldTypes.Author.ToString())?.Value,
                    Version = element.Element(FieldTypes.Version.ToString())?.Value,
                    Description = element.Element(FieldTypes.Description.ToString())?.Value,
                    TagOne = element.Element(FieldTypes.Tag.ToString())?.Value,
                    HashSumm = element.Element(FieldTypes.HashSumm.ToString())?.Value,
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
        private static void SetValueDeclaredProperties(List<T> packages, XElement element, int index)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {
                property.SetValue(packages[index], element.Element(property.Name)?.Value);
            }
        }

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
        
        /// <summary>
        /// Adding user field to property of collection. 
        /// </summary>
        /// <param name="node">Accents a current item.</param>
        /// <returns>Return the collection of <c>object</c> is TextFieldModel type.</returns>
        private static List<TextFieldModel> SetFieldValue(XElement node)
        {
            List<TextFieldModel> textField = new List<TextFieldModel>();
            IEnumerable<XElement> elements = node.Elements();

            foreach (var element in elements)
            {
                sbyte count = 0;
                // This part of the method will work if the package contains more than one user field.
                if (element.HasElements && element.Name == FieldTypes.Userfield + "List")
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
                // This part of the method will work if the package contains single user field.
                if (!element.HasElements && element.Name == FieldTypes.Userfield.ToString())
                    textField.Add(new TextFieldModel
                    {
                        FieldValue = element.Value,
                        Types = element.Name.ToString(),
                        Label = element.LastAttribute.Value,
                        Hint = element.LastAttribute.Value
                    });
                // TODO: Найти элегантное решение взамен этого куска кода.
                // Добавление всех полей кроме основных и пользовательских.
                if (!element.HasElements && element.Name != "Name" && element.Name != "Description"
                    && element.Name != "Tag" && element.Name != "Image" && element.Name != "Userfield" && element.Name != "Icon")
                    textField.Add(new TextFieldModel
                    {
                        FieldValue = element.Value,
                        Types = element.Name.ToString()
                    });
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
    }
}
