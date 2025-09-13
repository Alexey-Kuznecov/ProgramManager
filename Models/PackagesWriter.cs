
namespace ProgramManager.Models
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Converters;
    using Enums;
    using Func;
    using PackageModel;
    using Services;

    /// <summary>
    /// The packages writer.
    /// </summary>
    public class PackagesWriter
    {
        /// <summary>
        /// The document name.
        /// </summary>
        private const string DocumentName = "../../Resources/User/packages.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="PackagesWriter"/> class.
        /// </summary>
        public PackagesWriter()
        {
            if (!File.Exists(DocumentName))
            {
                PackagesWriter.FormatHeadXmlDoc();
            }
        }

        #region Functions changing data xml data
        
        /// <summary>
        /// The main method adds two attributes Id, Category and delegates the work to create a new package.
        /// </summary>
        /// <param name="currentPack"> Package data, an <c>object</c> of type PackageBase is expected. </param>
        /// <param name="category"> The <c>category</c> in the context of which the package will be created.</param>
        public static void AddPackage(PackageBase currentPack, string category)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            XElement package = FormatPackageXmlMarkup(currentPack);
            XElement packageS = new XElement("Package");
            
            // Gets the index of the last element in an xml document
            short id = BaseXml.GetIdLastElement();

            packageS.SetAttributeValue("Id", ++id);
            packageS.SetAttributeValue("Category", category);
            packageS.Add(package.Elements().OrderBy(p => p.Name.ToString().Substring(0, 2)));
            xDoc.Root?.Add(packageS);
            xDoc.Save(DocumentName);

            // Updating the list of packages.
            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("Update package list!");
        }

        /// <summary>
        /// Package update method.
        /// </summary>
        /// <param name="data"> Package <c>data</c>, an <c>object</c> of type PackageBase is expected. </param>
        public static void UpdatePackage(PackageBase data)
        {
            XElement root = XElement.Load(DocumentName),
                     newPackage = FormatPackageXmlMarkup(data);

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

            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("Updating the list of packages");
        }

        /// <summary>
        /// Deletes the entire package by index and saves the document.
        /// </summary>
        /// <param name="id"> The index of the package to be deleted. </param>
        public static void RemovePackage(int id)
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            var root = xDoc.Root?.Elements("Package");

            if (root != null)
            {
                foreach (var item in root)
                {
                    if (item.FirstAttribute.Value == id.ToString())
                    {
                        item.Remove();
                    }
                }
            }

            xDoc.Save(DocumentName);

            EventAggregate ins = new EventAggregate();
            ins.OnLoadPackage("Updating the list of packages");
        }

        #endregion

        #region Functions additional data processing

        /// <summary>
        /// The method adds these icons: geometry, color and background of the icon.
        /// </summary>
        /// <param name="currentPack"> An <c>object</c> derived from PackageBase is expected. </param>
        public static void AddIcon(PackageBase currentPack)
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
                    new XAttribute("Foreground", currentPack.Icon.ForegroundColor),
                    new XAttribute("Background", currentPack.Icon.BackgroundColor),
                    new XAttribute("Path", currentPack.Icon.Path.Data.ToString().Replace(',', '.').Replace(';', ','))));

                root.Save(iconData);
            }
        }

        /// <summary>
        /// The add image.
        /// </summary>
        /// <param name="currentPackXml"> The current pack xml. </param>
        /// <param name="currentPack"> The current pack. </param>
        public static void AddImage(XElement currentPackXml, PackageBase currentPack)
        {
            var imgcover = currentPack.Image;
            string uri = "../Resources/User/Images/" + imgcover?.Name + ".png";
            currentPackXml.Add(new XElement("Image", new XAttribute("Source", uri)));
        }

        /// <summary>
        /// This method creates xml markup using the values of the properties of an <c>object</c> derived from PackageBase.
        /// </summary>
        /// <param name="currentPack"> An <c>object</c> derived from PackageBase is expected. </param>
        /// <returns> Package in the form of xml markup. </returns>
        private static XElement FormatPackageXmlMarkup(PackageBase currentPack)
        {
            XElement package = new XElement("Package");
            var properties = currentPack.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(currentPack) == null)
                {
                    continue;
                }

                if (property.PropertyType.Name == "String")
                {
                    package.Add(new XElement(property.Name, property.GetValue(currentPack)));
                }

                if (property.Name == "FieldList")
                {
                    AddUserField(package, currentPack);
                }

                if (property.Name == "TagList")
                {
                    AddTag(package, currentPack);
                }

                if (property.Name == "Icon")
                {
                    AddIcon(currentPack);
                }

                if (property.Name == "Image")
                {
                    AddImage(package, currentPack);
                }
            }

            // Groups elements with the same name into one node and adds a "List" to the name of the new node.
            return package.CreatingNestedElements().PostfixElementName();
        }

        /// <summary>
        /// The method creates an xml element based on the elements of the tag list.
        /// </summary>
        /// <param name="currentPackXml"> The current package of the xml document. </param>
        /// <param name="currentPack"> An <c>object</c> derived from PackageBase is expected. </param>
        private static void AddTag(XElement currentPackXml, PackageBase currentPack)
        {
            foreach (var value in currentPack.TagList)
            {
                currentPackXml.Add(new XElement("Tag", value));
            }
        }

        /// <summary>
        /// The method generates xml elements based on user field data (Name, value).
        /// </summary>
        /// <param name="currentPackXml"> The current package of the xml document. </param>
        /// <param name="currentPack"> An <c>object</c> derived from PackageBase is expected. </param>
        private static void AddUserField(XElement currentPackXml, PackageBase currentPack)
        {
            foreach (var item in currentPack.FieldList)
            {
                currentPackXml?.Add(new XElement(FieldTypes.Userfield.ToString(), new XAttribute("Label", PackageFieldConverter.Dictionary.Single(p => p.Key == item.Key).Value), item.Value));
            }
        }

        #endregion

        /// <summary>
        /// Creates an XML document header
        /// </summary>
        private static void FormatHeadXmlDoc()
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XDocumentType("Packages", null, "Packages.dtd", null),
                new XProcessingInstruction("PackageHandler", "out-of-print"),
                new XElement("Packages"));
            xDoc.Save(DocumentName);
        }
    }
}
