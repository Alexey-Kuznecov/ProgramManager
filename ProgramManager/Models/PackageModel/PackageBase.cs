using System.Collections.Generic;
using ProgramManager.Filters;

namespace ProgramManager.Models.PackageModel
{
    public abstract class PackageBase
    {
        private readonly IDictionary<string, string> _fieldList = new Dictionary<string, string>();
        public int Id { get; set; }
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
        public List<PropertyNotIsNull> Datails { get; set; }
        public List<TextFieldModel> TextField { get; set; }
        public abstract void AddPackage();
        public virtual void UpdatePackage() { }
        public virtual void RemovePackage() { }
    }
}
