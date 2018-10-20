﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ProgramManager.Enums;
using ProgramManager.Filters;

namespace ProgramManager.Models.PackageModel
{
    /// Этот класс является обобщенным поэтому каждый потомок класса PackageBase будет совместим с данным классом, свойства производных классов
    /// будут гарантированно проинициализированы значениями из элементов xml документа, если имена свойств совпадают с именами xml элементов.
    /// По результатам работы класса создается коллекция объектов типа List Т, свойства объектов которых будут инициализированы значениями элементов xml документа.
    public class PackageAccess<T> where T : PackageBase, new()
    {
        private const string DocumentName = "packages.xml";
        /// <summary>
        /// Базовый метод фильтрует и извлекает данные из xml документа, на основе данных формирует новый объект.
        /// Вызывает некоторые вспомогательные методы для боллее тонкой обработки данных и объекта. 
        /// </summary>
        /// <param name="category">Принимает объект, свойство(Name) которого равняется текущей категории, необходимой для усвловия фильтрации.</param>
        /// <returns>Возвращает коллекцию объектов производных от базового класса PackageBase.</returns>
        public static List<T> GetPackages(CategoryModel category)
        {
            List<T> packages = new List<T>();
            FilterProperties<T> propNotIsEmpty = new FilterProperties<T>();
            XElement root = XElement.Load(DocumentName);
            int index = 0;

            // Запрос с фильтрацией данных. 
            IEnumerable<XElement> document = from element in root.Elements("Package")
                                             where (string)element.Attribute("Category") == category.Name
                                             select element;

            // Формирования нового объекта на основе данных xml документа.
           foreach (XElement element in document)
           {
                // Инициализация свойств из базового класса
               packages.Add(new T
               {
                   Name = element.Element(FieldTypes.Name.ToString())?.Value,
                   Author = element.Element(FieldTypes.Author.ToString())?.Value,
                   Version = element.Element(FieldTypes.Version.ToString())?.Value,
                   Image = element.Element(FieldTypes.Image.ToString())?.FirstAttribute.Value,
                   Description = element.Element(FieldTypes.Description.ToString())?.Value,
                   TagOne = element.Element(FieldTypes.Tag.ToString())?.Value,
                   HashSumm = element.Element(FieldTypes.HashSumm.ToString())?.Value,
                   // Вызов метода для создания коллекции тегов, если пакет имеет более одного тега
                   TagList = GetTagsList(element),
                   Category = element.LastAttribute.Value,
               });
               // Вызов метода для инициализации свойств производного класса.
               SetValueDeclaredProperties(packages, element, index);
               // Вызов метода фильтрации полей с пустыми значениями данного объекта.
               packages[index].Datails = propNotIsEmpty.Filter(packages[index++]);              
            }
            return packages;
        }
        /// <summary>
        /// Вспомогательный метод для получения массива тегов(текст), так как пакеты могут иметь больше одного тега.
        /// Метод находит элемент <TagList></TagList> и формирует массив на основе содержимого данного элемента.
        /// </summary>
        /// <param name="node">Контекст текущего родительского элемента.</param>
        /// <returns>Коллекцию строковых элеменов(тегов), принадлежащих текущему пакету</returns>
        private static List<string> GetTagsList(XElement node)
        {
            IEnumerable<XElement> elements = node.Elements("TagList");
            List<string> tags = new List<string>();

            foreach (var element in elements.Elements(FieldTypes.Tag.ToString())) tags.Add(element.Value);

            return tags;
        }
        /// <summary>
        /// Устанавливает значения свойствам объявленным в производном классе.
        /// </summary>
        /// <param name="packages">Коллекция объектов(пактов)</param>
        /// <param name="element">Контекст текущего родительского элемента xml документа.</param>
        /// <param name="index">Текущий индекс элемента коллекции.</param>
        private static void SetValueDeclaredProperties(List<T> packages, XElement element, int index)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {
                property.SetValue(packages[index], element.Element(property.Name)?.Value);
            }
        }
    }
}