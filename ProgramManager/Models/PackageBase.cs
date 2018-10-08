using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProgramManager.Models
{
    public abstract class PackageBase
    {
        protected const string DOCUMENT_NAME = "packages.xml";

        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Source { get; set; }
        public string HashSumm { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string Image { get; set; }

        public virtual void AddPackage()
        {
            XDocument xDoc = XDocument.Load(DOCUMENT_NAME);
                xDoc.Element("Packages")
                    ?.Add(new XElement("Package",
                        new XElement("Name", Name ?? ""),
                        new XElement("Author", Author ?? ""),
                        new XElement("Version", Version ?? ""),
                        new XElement("CompanySite", Copyright ?? ""),
                        new XElement("Source", Source ?? ""),
                        new XElement("HashSumm", HashSumm ?? ""),
                        new XElement("Description", Description ?? ""),
                        new XElement("Image", new XAttribute("Source", Image ?? ""))));
            xDoc.Save(DOCUMENT_NAME);
        }
        public virtual void GetPackages() { }
        public virtual void UpdatePackage () { }
        public virtual void RemovePackage () { }
    }
}
