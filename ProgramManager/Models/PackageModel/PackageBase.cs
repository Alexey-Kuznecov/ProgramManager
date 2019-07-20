using System.Collections.Generic;
using System.Xml.Linq;
using ProgramManager.Converters;
using ProgramManager.Resources;

namespace ProgramManager.Models.PackageModel
{
    public abstract class PackageBase : PackageDrtails
    {
        protected virtual string CatName { get; }
        private readonly IDictionary<string, string> _fieldList = new Dictionary<string, string>();
        public delegate Dictionary<string, string> DelegateMenuItem();
        public DelegateMenuItem LoadItem { get; set; }
        public int Id { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string TagOne { get; set; }
        public IconModel Icon { get; set; }
        public List<string> TagList { get; set; }
        public IDictionary<string, string> FieldList
        {
            get { return _fieldList; }
            set
            {
                if (value != null)
                    _fieldList.Add(value.Keys.ToString(), value.Values.ToString());
            }
        }
        public List<TextFieldModel> TextField { get; set; }
        public static Dictionary<string, string> MenuItem { get; set; }
        public virtual Dictionary<string, string> LoadMenuItem()
        {
            MenuItem = new Dictionary<string, string>();
            XDocument root = XDocument.Load("../../Resources/User/ContextMenu.xml");
            XElement menuItem = new XElement("MenuItem");
            MenuItem.Clear();

            foreach (var menu in root.Elements().Elements())
            {
                if (menu.FirstAttribute.Value == CatName || menu.FirstAttribute.Value == "Общие")
                {
                    menuItem = menu;

                    foreach (var item in menuItem.Elements())
                    {
                        XElement key = item.Element("Key"),
                            value = item.Element("Value");

                        if (key == null || value == null) continue;
                        if (!MenuItem.ContainsKey(key.Value))
                        {
                            MenuItem.Add(key.Value, value.Value);
                            if (!PackageFieldConverter.Dictionary.ContainsKey(key.Value))
                                PackageFieldConverter.Dictionary.Add(key.Value, value.Value);
                        }
                    }
                }
            } return MenuItem;
        }
    }
}
