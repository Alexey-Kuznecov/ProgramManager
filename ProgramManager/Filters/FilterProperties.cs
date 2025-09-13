using ProgramManager.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ProgramManager.Filters
{
    public class PropertyNotIsNull
    {
        public string Name { get; set; }
        public string Value { get; set; } 
    }
    public class FilterProperties<T>
    {
        /// <summary>
        /// Метод призван отсеивать свойства которые равняется null и не являются строковым типом,
        /// основное предназначение метода это вывод информации в UI свойств содержащих значение,
        /// где Name это имя свойства и где Value значение этого свойства.
        /// </summary>
        /// <param name="obj">Принимает объект в котором необходимо осуществить фильтрацию.</param>
        /// <returns>Объект с полями которые содержат значения</returns>
        public List<PropertyNotIsNull> Filter(T obj)
        {
            List<PropertyNotIsNull> field = new List<PropertyNotIsNull>();
            Type classType = typeof(T);
            PropertyInfo[] properties = classType.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(obj) != null && property.GetValue(obj) is string)
                {
                    field.Add(new PropertyNotIsNull()
                    {
                        Name = FieldConverter.Dictionary.SingleOrDefault(k => k.Key == property.Name).Value,
                        Value = obj.GetType().GetProperty(property.Name)?.GetValue(obj).ToString()
                    });
                }
            }
            return field;
        }
    }
}
