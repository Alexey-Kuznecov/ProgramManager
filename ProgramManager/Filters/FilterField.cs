using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using ProgramManager.Models;

namespace ProgramManager.Filters
{
    public class FieldNotIsNull
    {
        public string Name { get; set; }
        public string Value { get; set; } 
    }
    public class FilterField<T>
    {
        /// <summary>
        /// Метод призван отсеиваеть поля которые равняеются null и не явлеются строковым типом,
        /// основное предназначения мотода это вывод информации в UI, где Name это имя свойства
        /// и где Value значение этого свойтва.
        /// </summary>
        /// <param name="obj">Принимет объект в котором необходимо осуществить фильтрацию.</param>
        /// <returns>Объект с полями которые содержать значения</returns>
        public List<FieldNotIsNull> Filter(T obj)
        {
            List<FieldNotIsNull> field = new List<FieldNotIsNull>();

            Type classType = typeof(T);

            PropertyInfo[] properties = classType.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(obj) != null && property.GetValue(obj) is string)
                {
                    field.Add(new FieldNotIsNull() { Name = property.Name, Value = obj.GetType().GetProperty(property.Name)?.GetValue(obj).ToString()});
                }
            }
            return field;
        }
    }
}
