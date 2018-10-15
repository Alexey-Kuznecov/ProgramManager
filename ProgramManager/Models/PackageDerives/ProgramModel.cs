using System.Xml.Linq;

namespace ProgramManager.Models.PackageDerives
{
    public class ProgramModel : PackageBase
    {
        public string License { get; set; }
        public string CompanySite { get; set; }
        public string SerialKey { get; set; }

        public override void AddPackage()
        {
            XDocument xDoc = XDocument.Load(DocumentName);
            xDoc.Element("Packages")
                ?.Add(new XElement("Package", 
                    new XAttribute("Id", Category),
                    new XElement("Name", Name ?? ""),
                    new XElement("Author", Author ?? ""),
                    new XElement("Version", Version ?? ""),
                    new XElement("Source", Source ?? ""),
                    new XElement("License", License ?? ""),
                    new XElement("CompanySite", CompanySite ?? ""),
                    new XElement("SerialKey", SerialKey ?? ""),
                    new XElement("HashSumm", HashSumm ?? ""),
                    new XElement("Description", Description ?? ""),
                    new XElement("Image", new XAttribute("Source", Image ?? ""))));
            xDoc.Save(DocumentName);
        }
    }
}
