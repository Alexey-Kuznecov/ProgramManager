using System.Collections.Generic;
using ProgramManager.Filters;
using System.Xml.Linq;
using ProgramManager.Converters;

namespace ProgramManager.Models.PackageModel
{
    public abstract class PackageBase : PackageDrtails
    {
        protected virtual string Status { get; }
        private readonly IDictionary<string, string> _fieldList = new Dictionary<string, string>();
        public delegate Dictionary<string, string> DelegateMenuItem();
        public DelegateMenuItem LoadItem { get; set; }
        public int Id { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string TagOne { get; set; }
        public List<string> TagList { get; set; }
        public IDictionary<string, string> FieldList
        {
            get { return _fieldList; }
            set
            {
                if (value != null)
                {
                    _fieldList.Add(value.Keys.ToString(), value.Values.ToString());
                }
            }
        }
        /// <summary>
        /// Это коллекция вбирает в себя все другие свойства для вывода их в панель деталей.
        /// При этом свойства не будут содержать пустые значения.
        /// </summary>
        // public List<PropertyNotIsNull> Datails { get; set; }
        public List<TextFieldModel> TextField { get; set; }
        public static Dictionary<string, string> MenuItem { get; set; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> LoadMenuItem()
        {
            XDocument root = XDocument.Load("../../Resources/User/ContextMenu.xml");
            XElement menuItem = new XElement("MenuItem");
            MenuItem.Clear();

            foreach (var menu in root.Elements().Elements())
            {
                if (menu.FirstAttribute.Value == Status || menu.FirstAttribute.Value == "Общие")
                {
                    menuItem = menu;

                    foreach (var item in menuItem.Elements())
                    {
                        if (!MenuItem.ContainsKey(item.Element("Key").Value))
                        {
                            MenuItem.Add(item.Element("Key").Value, item.Element("Value").Value);

                            if (!FieldConverter.Dictionary.ContainsKey(item.Element("Key").Value))
                            {
                                FieldConverter.Dictionary.Add(item.Element("Key").Value, item.Element("Value").Value);
                            }
                        }
                    }

                }
            }
            return MenuItem;
        }
    }
}
