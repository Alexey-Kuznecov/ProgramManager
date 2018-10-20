using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ProgramManager.Converters;

namespace ProgramManager.Models.Func
{
    public static class FuncHelper 
    {
        private static XElement _document;
        /// <summary>
        /// Метод добавляет постфикс списка, если элемент содержит другие элементы.
        /// </summary>
        /// <param name="xElement">Дочерние элементы родительского узла.</param>
        /// <returns>Возвращает родительские элементы в с постфиксом</returns>
        public static XElement PostfixElementName(this XElement xElement)
        {
            string str = xElement.ToString();
            _document = new XElement("Package");
            XDocument node = XDocument.Parse(str);

            var names = node.Elements().Elements().Select(e => e);

            foreach (var name in names)
            {
                if (name.HasElements)
                {
                    name.Name = name.Name + "List";
                }
                _document.Add(name);
            }
            return _document;
        }
        /// <summary>
        /// Метод удаляет символы которые не должны содеражаться в имени элемента, так как могут использоваться в другом месте,
        /// но не в чистом xml. Например для того чтобы придать уникальсноть к имени ключа для словаря. 
        /// </summary>
        /// <param name="xElement">Дочерние элементы родительского узла.</param>
        /// <returns>Возвращает очищенные элементы от символов перечисленных в массиве.</returns>
        public static XElement TrimElementName(this XElement xElement)
        {
            char[] symbol = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            _document = new XElement("Package");
            string str = xElement.ToString();
            XDocument node = XDocument.Parse(str);

            var names = node.Elements().Elements().Select(e => e);

            foreach (var name in names)
            {
                name.Name = name.Name.ToString().TrimEnd(symbol);
                _document.Add(name);
            }                    
            return _document;
        }
        /// <summary>
        /// Метод создает словарь для удобного добавления элемента и его значения в xml документ.
        /// Метод способен преобразовывать не только простые типы, но и коллекции такие как List.
        /// Если объект содержит коллекцию словарей, метод не запнется и так же его успешно обработает.
        /// Стоит отметить что в качесве ключа, метод добавляет анонимный тип, что позовляет хранить
        /// дополнительную информацию например: xml тег, атрибуты и т.д.
        /// </summary>
        /// <param name="data">Объект данных, который может содержать свойства следующих типов (Колекции, простые типы такие как string).</param>
        /// <returns>Коллекцию словарей <TKey>Анонимный тип</TKey><TValue>Значение</TValue></returns>
        public static Dictionary<object, string> ConvertToDictionary(this object data)
        {
            Dictionary<object, string> dictionary = new Dictionary<object, string>();
            PropertyInfo[] properties = data.GetType().GetProperties();
            int count = 0;

            foreach (var property in properties)
            {
                if (property.GetValue(data) == null) continue;

                // Код для преобразования свойств типа List.
                if (property.PropertyType.Name.Contains("List"))
                {
                    var tagsList = property.GetValue(data);
                    

                    foreach (var tag in (List<string>)tagsList)
                    {
                        // Ананимный тип кроме ключа играет роль носителя дополнительной информации.
                        var key = new { Name = "Tag" , Id = count++ };
                        dictionary.Add(key, tag);
                    }
                }
                // Код для преобразования свойств типа String.
                if (property.PropertyType.Name.Contains("String"))
                {
                    var key = new { property.Name };
                    dictionary.Add(key, property.GetValue(data).ToString());
                }
                // Если объект содержит свойства типа IDictionary, этот кусок
                // кода извлечет и добавит в Dictionary.
                if (property.PropertyType.Name.Contains("IDictionary"))
                {
                    count = 0;

                    foreach (DictionaryEntry entry in (IDictionary)property.GetValue(data))
                    {
                        var key = new
                        {
                            Name = entry.Key,
                            Id = count++,
                            FieldName = FieldConverter.Dictionary.Single(p => p.Key.ToString() == entry.Key.ToString()).Value
                        };

                        dictionary.Add(key, entry.Value.ToString());
                    }
                }
            }
            return dictionary;
        }
        /// <summary>
        /// Этот метод группирует элементы похожие по смыслу в один независимый узел. 
        /// </summary>
        /// <param name="xElement">Родительский элемент, элементы котого нужно сгруппировать.</param>
        /// <returns>Родительский элемент с сгруппированными элементами.</returns>
        public static XElement CreatingNestedElements(this XElement xElement)
        {
            string str = xElement.ToString();
            XElement nested = new XElement("Nested");
            XElement document = new XElement("Package");
            ArrayList nameList = new ArrayList();
            XDocument node = XDocument.Parse(str);
            var names = node.Elements().Elements().Select(e => e);

            // Поиск элементов с одинаковыми именами
            foreach (var name in names)
            {
                foreach (var element in node.Elements().Elements())
                {
                    if (name.IsBefore(element) && element.Name == name.Name)
                    {
                        if (!nameList.Contains(element.Name))
                            nameList.Add(element.Name);
                    }
                }
            }
            foreach (var item in nameList)
            {
                foreach (var name in names)
                {
                    // Группирование одинаковых элементов в отдельные узлы.
                    if (name.Name == item.ToString())
                    {
                        if (!nested.HasElements)
                        {
                            nested.Add(new XElement(name.Name));

                            if (item.ToString() == nested.Element(name.Name)?.Name)
                                nested.Element(name.Name)?.Add(name);
                        }
                        else
                        {
                            if (item.ToString() == nested.Element(name.Name)?.Name)
                                nested.Element(name.Name)?.Add(name);
                            else
                                nested.Add(new XElement(name.Name, name));
                        }
                    }
                    // Элементы с уникальными именами.
                    if (!nameList.Contains(name.Name))
                    {
                        if (!document.Elements().Contains(document.Element(name.Name)))
                            document.Add(name);
                    }
                }
            }
            // Добавление узлов в общий узел.
            document.Add(nested.Elements());

            if (nameList.Count == 0)
                return xElement;
            return document;
        }
    }
}
