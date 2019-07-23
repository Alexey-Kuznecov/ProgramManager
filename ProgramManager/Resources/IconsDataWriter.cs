using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace ProgramManager.Resources
{
    class IconsDataWriter : IDisposable
    {
        private const string DocumentName = @"..\..\Resources\IconsData.xml";

        public void LoadDocument()
        {
            XElement root = XElement.Load(DocumentName);
        }
        //[DebuggerStepperBoundary]
        public static void Save(IconModel iconModel)
        {
            XElement root = XElement.Load(DocumentName);
            var queryCategory = from name in root.Elements().Attributes("Name")
                where name.Value == iconModel.Category
                select name.Parent;
           
            foreach (var element in queryCategory)
            {
                element.Add(
                    new XElement("Icon", 
                        new XAttribute("Id", iconModel.Id),
                        new XAttribute("Name", iconModel.Name),
                        new XAttribute("Scale", iconModel.Scale),
                        new XAttribute("Background", iconModel.BgroundColor),
                        new XAttribute("Foreground", iconModel.FgroundColor), 
                        new XElement("Path", iconModel.StringPath)));
            }
            root.Save(DocumentName);
        }
        public static void SetName(string oldName, string newName)
        {
            XElement root = XElement.Load(DocumentName);
            var queryName = from icon in root.Elements().Elements()
                where icon.Attribute("Name")?.Value == oldName
                            select icon;
            foreach (var icon in queryName)
                if (icon != null)
                    // ReSharper disable once PossibleNullReferenceException
                    icon.Attribute(name: "Name").Value = newName;
            root.Save(DocumentName);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
