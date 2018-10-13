using System;
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
        /// Метод призван отсеиваеть свойства которые равняеются null и не явлеются строковым типом,
        /// основное предназначения метода это вывод информации в UI свойств содеражащих значенеие,
        /// где Name это имя свойства и где Value значение этого свойтва.
        /// </summary>
        /// <param name="obj">Принимет объект в котором необходимо осуществить фильтрацию.</param>
        /// <returns>Объект с полями которые содержать значения</returns>
        public List<PropertyNotIsNull> Filter(T obj)
        {
            List<PropertyNotIsNull> field = new List<PropertyNotIsNull>();

            Type classType = typeof(T);

            PropertyInfo[] properties = classType.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(obj) != null && property.GetValue(obj) is string)
                {
                    field.Add(new PropertyNotIsNull() { Name = property.Name, Value = obj.GetType().GetProperty(property.Name)?.GetValue(obj).ToString()});
                }
            }
            return field;
        }
    }
}
