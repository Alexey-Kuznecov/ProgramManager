using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProgramManager.Resources
{
    class IconsOptionReader
    {
        private const string DocumentName = @"..\..\Resources\IconsOption.xml";
        private static IEnumerable<XElement> _elementIcon;
        private static IEnumerable<XElement> _elementCat;

        public static List<string> GetCategory()
        {
            List<string> categories = new List<string>();
            XElement root = XElement.Load(DocumentName);
            _elementCat = from element in root.Elements("Category") select element;

            foreach (var cat in _elementCat)
                categories.Add(cat.FirstAttribute.Value);
            return categories;
        }
        public static List<XElement> GetIcons()
        {
            _elementIcon = from icon in _elementCat.Elements("Icon") select icon;
            List<XElement> elementIcon = _elementIcon.ToList();
            var attrId = from id in elementIcon select id.Attribute("Id");
            var attrName = from name in elementIcon select name.Attribute("Name");
            var attrBackground = from background in elementIcon select background.Attribute("Background");
            var attrForeground = from foreground in elementIcon select foreground.Attribute("Foreground");
            return elementIcon;
        }
    }
}
