using System.Collections.Generic;
<<<<<<< HEAD
=======
using ProgramManager.Filters;
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
using System.Xml.Linq;
using ProgramManager.Converters;

namespace ProgramManager.Models.PackageModel
{
    public abstract class PackageBase : PackageDrtails
    {
<<<<<<< HEAD
        protected virtual string CatName { get; }
=======
        protected virtual string Status { get; }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        private readonly IDictionary<string, string> _fieldList = new Dictionary<string, string>();
        public delegate Dictionary<string, string> DelegateMenuItem();
        public DelegateMenuItem LoadItem { get; set; }
        public int Id { get; set; }
<<<<<<< HEAD
        public ImageCover Image { get; set; }
        public string Category { get; set; }
        public string TagOne { get; set; }
        public IconModel Icon { get; set; }
=======
        public string Image { get; set; }
        public string Category { get; set; }
        public string TagOne { get; set; }
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        public List<string> TagList { get; set; }
        public IDictionary<string, string> FieldList
        {
            get { return _fieldList; }
            set
            {
                if (value != null)
<<<<<<< HEAD
                    _fieldList.Add(value.Keys.ToString(), value.Values.ToString());
            }
        }
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
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
<<<<<<< HEAD
                if (menu.FirstAttribute.Value == CatName || menu.FirstAttribute.Value == "Общие")
=======
                if (menu.FirstAttribute.Value == Status || menu.FirstAttribute.Value == "Общие")
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
                {
                    menuItem = menu;

                    foreach (var item in menuItem.Elements())
                    {
<<<<<<< HEAD
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
=======
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
        }
    }
}
