using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Models;
using ProgramManager.Services;

namespace ProgramManager.Plugins.IconsEditor.Data
{
    class IconsDataWriter : IDisposable
    {
        private const string DocumentName = @"..\..\Plugins\IconsEditor\Data\IconsData.xml";
        private static int Id;
        public void LoadDocument()
        {
            XElement root = XElement.Load(DocumentName);
        }
        /// <summary>
        /// Create xml element the Icon on base IconModel object. 
        /// </summary>
        /// <param name="iconModel">Icon object.</param>
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
                        new XAttribute("Id", GetId().ToString()),
                        new XAttribute("Name", iconModel.Name),
                        new XAttribute("Scale", iconModel.Scale),
                        new XAttribute("Background", iconModel.BgroundColor),
                        new XAttribute("Foreground", iconModel.FgroundColor)));
                SetMultiPath(element, iconModel);
            }
            root.Save(DocumentName);
        }
        /// <summary>
        /// Find element with the most id value and increment on one.
        /// </summary>
        /// <returns></returns>
        private static int GetId()
        {
            ArrayList arrayList = new ArrayList();

            XElement root = XElement.Load(DocumentName);
            IEnumerable<XElement> queryCollecElements = from collect in root.Elements() select collect,
                                  queryIconElements = from icon in queryCollecElements.Elements() select icon;

            foreach (var list in queryIconElements.Attributes())
                if (list.Name == "Id")
                    arrayList.Add(value: int.Parse(list.Value));
            Id = arrayList.MaxValue() + 1;
            return Id;
        }
        /// <summary>
        /// Create xml path element on base collection of paths.
        /// </summary>
        /// <param name="element">Currnet xml collection.</param>
        /// <param name="iconModel">Wait pathList property of object IconModel.</param>
        public static void SetMultiPath(XElement element, IconModel iconModel)
        {
            var query = from icon in element.Elements()
                where icon.Attribute("Id")?.Value == Id.ToString()
                select icon;

            foreach (var icon in query)
                foreach (var path in iconModel.PathList)
                    icon.Add(new XElement("Path", new XAttribute("Fill", path.Fill), path.Data.ToString().Substring(2).Replace(',', '.').Replace(';', ','))); 
        }
        /// <summary>
        /// Replace icon name on new name.
        /// </summary>
        /// <param name="oldName">Old name icons for finding it in a collection.</param>
        /// <param name="newName">New name.</param>
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
        /// <summary>
        /// Adds new icons collection in the xml file.
        /// </summary>
        /// <param name="name">Name new collection.</param>
        public static void AddNewCollection(string name)
        {
            XElement root = XElement.Load(DocumentName);
            root.Add(new XElement("Collection", new XAttribute("Name", name)));
            root.Save(DocumentName);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
