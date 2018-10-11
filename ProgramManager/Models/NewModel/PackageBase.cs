using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ProgramManager.Filters;
using ProgramManager.Models.NewModel;

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
        public string Image { get; set; }
        public string Category { get; set; }
        public string TagOne { get; set; }
        public List<string> TagList { get; set; }
        public List<WrapPackages> Tag { get; set; }
        public List<FieldNotIsNull> Datails { get; set; }

        public abstract void AddPackage();
        public abstract List<ProgramModel> GetPackages(CategoryModel category);
        public virtual void UpdatePackage() { }
        public virtual void RemovePackage() { }
    }
}
