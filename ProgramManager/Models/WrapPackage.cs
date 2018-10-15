using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Models.PackageDerives;

namespace ProgramManager.Models
{
    /// <summary>
    /// Класс оболочка, содержит методы для упаковки пакетов. Основная идея создания оболочки для представления,
    /// это привязка объектов(пакетов) к определенным именам (тегам), обращение к объектам из представления
    /// по их именам позволяет отсеивать лишние объекты. Методы совершают упаковку объектов потомков PackageBase,
    /// создают оболочку для представления.
    /// </summary>
    public class WrapPackage : IEnumerable<PackageBase>
    {
        public string Name { get; set; }
        public List<PackageBase> Packages { get; set; }
        public static List<PackageBase> AllPackages { get; set; }
        /// <summary>
        /// Главный метод в основном делегирует работу других методов и возвращает результат.
        /// Метод также выполняет роль упаковщка, для спецальных тегов (Избранные, Все и т.д)
        /// </summary>
        /// <param name="collection">Применяет коллекцию объектов типа PackageBase</param>
        /// <returns>Возвращает объект оболочку для представления</returns>
        public static List<WrapPackage> WrapPackageTag<T>(List<T> @collection) where T : PackageBase, new ()
        {
            List<WrapPackage> wrapperPackage = new List<WrapPackage>();
            
            // TODO: Обратотать исключение, например ввыести сообщение об отсутвии данных...
            // WARNING: Выдает исключение если в базе отсутствует данные с какой-либо категорией.
            // Вызов метода для поиска тегов а xml документе и инициализация свойств класса оболочки
            wrapperPackage = wrapperPackage.Count == 0 ? TagFinder(@collection[0].Category) : wrapperPackage;
            InitialPackages(@collection, wrapperPackage);
            
            // Эта часть кода добавлеяет все данные по нулевому индексу.
            wrapperPackage.Insert(0, new WrapPackage() { Name = "Все теги" });
            wrapperPackage[0].Packages = new List<PackageBase>();
            for (var index = 0; index < @collection.Count; index++)
            {
                var package = @collection[index];
                wrapperPackage[0].Packages.Add(package);
            }                                  
            AllPackages = wrapperPackage[0].Packages;
            return wrapperPackage;
        }
        /// <summary>
        /// Метод распределяет пакеты в соответствии с значением свойства Name. Например:
        /// Пакет программы Visual Studio будет добавлен в свойство Package если Name равное IDE.
        /// Если пакет имеет больше тегов, метод InsertTags его обработает.
        /// Методы InitialPackages и InsertTags работают в паре, синхронно.
        /// </summary>
        /// <param name="collection">Коллекция пакетов для инициализции свойства Packages</param>
        /// <param name="wrapperPackages">Коллекция класса оболочки</param>
        public static void InitialPackages(dynamic @collection, List<WrapPackage> wrapperPackages)
        {
            int index = 0;

            foreach(var wrapper in wrapperPackages)
            {
                wrapperPackages[index].Packages = new List<PackageBase>();

                foreach (var package in @collection)
                {
                    if (package.TagOne != null)
                    {
                        if (wrapper.Name.Contains(package.TagOne))
                        {
                            wrapperPackages[index].Packages.Add(package);
                        }                       
                    }
                }
                // Синхронный вызов метода:
                InsertTags(@collection, wrapperPackages, wrapper, index);
                index++;
            }
        }
        /// <summary>
        /// Метод распределяет пакеты по тегам, если пакеты принадлежат сразу нескольким тегам.
        /// </summary>
        /// <param name="collection">Коллекция пакетов для инициализции свойства Packages</param>
        /// <param name="wrapperPackages">Коллекция класса оболочки</param>
        /// <param name="wrapper">Контекст текущей оболочки</param>
        /// <param name="index">Индекс текущей оболочки</param>
        public static void InsertTags(dynamic @collection, List<WrapPackage> wrapperPackages, WrapPackage wrapper,  int index)
        {         
            // Вставляет пакеты которые могут иметь больше одного тега 
            foreach (var package in @collection)
            {
                if (package.TagList.Count != 0)
                {
                    var count = wrapperPackages[index].Packages.Count;

                    foreach (var packageTag in package.TagList)
                    {
                        if (wrapper.Name.Contains(packageTag))
                        {
                            wrapperPackages[index].Packages.Insert(count, package);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Метод является вспомогательным для метода WrapPackageTag который осуществляет поиск
        /// по xml документу и выбрирает только те значения которые являются уникальными.
        /// </summary>
        /// <param name="category">Категория пакетов, необходимая для учтонения выборки</param>
        /// <returns>Возвращает коллекцию уникальных значений.</returns>
        private static List<WrapPackage> TagFinder(string category)
        {
            string xmlDoc = "packages.xml";
            XElement root = XElement.Load(xmlDoc);

            var queryTag = (from e in root.Descendants("Package").Elements()
                where e.Name == "Tag" && e.Parent?.LastAttribute.Value == category
                select e.Value).ToList();

            var queryTags = root.Descendants("Package").Elements().Elements()
                .Where(e => e.Name == "Tag" && e.Parent?.Parent?.LastAttribute.Value == category)
                .Select(e => e.Value).ToList();

            queryTag.AddRange(queryTags);
            // Сортирует, фильтрует, выберает и преобразует в список:
            return queryTag.Distinct().OrderBy(x => x.Substring(0, 3))
                .Select(element => new WrapPackage() { Name = element }).ToList();
        }
        /// <summary>
        /// Реализалация интерфеса IEnumerator
        /// </summary>
        /// <returns>Возвращает пакет</returns>
        public IEnumerator<PackageBase> GetEnumerator()
        {
            foreach (var package in Packages)
            {
                yield return package;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
