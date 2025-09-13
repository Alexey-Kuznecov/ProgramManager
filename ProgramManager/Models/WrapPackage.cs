<<<<<<< HEAD
﻿

namespace ProgramManager.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;
    using PackageModel;
    using Services;

    /// <summary>
    /// The wrapper class contains methods for packaging packages.
    /// The main idea of creating a the wrapper for the presentation is to bind objects (packages) to specific names (tags),
    /// accessing objects from the view by their names allows you to filter out unnecessary objects.
    /// Methods pack the objects of <see cref="PackageBase"/> descendants, create a wrapper for View.
    /// </summary>
    public class WrapPackage : IEnumerable<PackageBase>
    {
        /// <summary>
        /// The _tag list.
        /// </summary>
        private static List<WrapPackage> _tagList;

        /// <summary>
        /// The _connector.
        /// </summary>
        private static EventAggregate _connector;

        /// <summary>
        /// Gets or sets the all packages.
        /// </summary>
        public static List<PackageBase> AllPackages { get; set; }

        /// <summary>
        /// Gets or sets the tag list.
        /// </summary>
        public static List<WrapPackage> TagList
        {
            get => _tagList;
            set
            {
                _tagList = value;
                _connector = new EventAggregate();

                // Raising the add event occurs when the tag list is fully loaded.          
                _connector.OnLoadTagsList(_tagList);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the packages.
        /// </summary>
        public List<PackageBase> Packages { get; set; }

        /// <summary>
        /// The main method basically delegates the work of other methods and returns the result.
        /// The method also acts as a wrapper for special tags (Favorites, All, etc.)
        /// </summary>
        /// <typeparam name="T"> Objects derived from <see cref="PackageBase"/>. </typeparam>
        /// <param name="collection"> Accepts a <paramref name="collection"/> of objects of type <see cref="PackageBase"/>. </param>
        /// <returns> Wrapper object for view. </returns>
        public static List<WrapPackage> WrapPackageTag<T>(List<T> @collection) where T : PackageBase, new()
        {
            List<WrapPackage> wrapperPackage = new List<WrapPackage>();

            // TODO: Handle an exception, for example, display a message about missing data ...
            // WARNING: Throws an exception if the database is missing data of any category.
            // Calling a method to search for tags in an xml document and initializing shell class properties
            wrapperPackage = wrapperPackage.Count == 0 ? TagFinder(@collection[0].Category) : wrapperPackage;
            InitialPackages(@collection, wrapperPackage);

            // This piece of code adds all the data at the zero index.
            wrapperPackage.Insert(0, new WrapPackage() {Name = "Все теги"});
            wrapperPackage[0].Packages = new List<PackageBase>();
            foreach (var package in @collection)
            {
                wrapperPackage[0].Packages.Add(package);
            }

            AllPackages = wrapperPackage[0].Packages;
            return wrapperPackage;
        }

        /// <summary>
        /// IEnumerator Interface Implementation.
        /// </summary>
        /// <returns> Returns package. </returns>
        public IEnumerator<PackageBase> GetEnumerator()
        {
            foreach (var package in this.Packages)
            {
                yield return package;
            }
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns> The <see cref="IEnumerator"/>. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// The method distributes packages according to the value of the <see cref="Name"/> property. For example:
        /// The package of the Visual Studio program will be added to the Package property if Name is equal to the IDE.
        /// If the package has more tags, the <see cref="InsertTags"/> method will process it.
        /// </summary>
        /// <param name="collection"> Packages collection for initializing Packages properties. </param>
        /// <param name="wrapperPackages"> The wrapper class collection. </param>
        private static void InitialPackages(dynamic @collection, List<WrapPackage> wrapperPackages)
        {
            int index = 0;

            foreach (var wrapper in wrapperPackages)
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

                // Synchronous method call:
                InsertTags(@collection, wrapperPackages, wrapper, index);
                index++;
            }
        }

        #region Methods for handling package tags.

        /// <summary>
        /// The method distributes packages by tags if packages belong to several tags at once.
        /// </summary>
        /// <param name="collection">A <paramref name="collection"/> of packages to initialize the <see cref="Packages"/> property.</param>
        /// <param name="wrapperPackages"> The wrapper class collection. </param>
        /// <param name="wrapper"> The context of the current wrapper class. </param>
        /// <param name="index"> The current wrapper class index. </param>
        [DebuggerStepThrough]
        private static void InsertTags(dynamic @collection, List<WrapPackage> wrapperPackages, WrapPackage wrapper, int index)
        {
            // Inserts packages that can have more than one tag. 
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
        /// The method is auxiliary to the <see cref="WrapPackageTag"/> method, which searches
        /// the xml document and selects only those values that are unique.
        /// </summary>
        /// <param name="category"> The category of packages needed to refine the selection. </param>
        /// <returns> Gets a collection of unique values. </returns>
        [DebuggerStepThrough]
        private static List<WrapPackage> TagFinder(string category)
        {
            string xmlDoc = "../../Resources/User/packages.xml";
            XElement root = XElement.Load(xmlDoc);

            var queryTag = (from e in root.Descendants("Package").Elements()
                where e.Name == "Tag" && e.Parent?.LastAttribute.Value == category
                select e.Value).ToList();

            var queryTags = root.Descendants("Package").Elements().Elements()
                .Where(e => e.Name == "Tag" && e.Parent?.Parent?.LastAttribute.Value == category)
                .Select(e => e.Value).ToList();

            queryTag.AddRange(queryTags);

            // Sorts, filters, selects and converts to a list:
            TagList = queryTag.Distinct().OrderBy(x => x.Substring(0, 3))
                .Select(element => new WrapPackage {Name = element}).ToList();

            return TagList;
        }

        #endregion
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ProgramManager.Models.PackageModel;
using ProgramManager.Services;
using ProgramManager.ViewModels;

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
        private static List<WrapPackage> _tagList;
        private static EventAggregate _connector;
        public string Name { get; set; }
        public List<PackageBase> Packages { get; set; }
        public static List<PackageBase> AllPackages { get; set; }
        public static List<WrapPackage> TagList
        {
            get { return _tagList; }
            set
            {
                _tagList = value;
                _connector = new EventAggregate();
                // Вызов события добавления, возникает при полной загруки списка тегов.           
                _connector.OnLoadTagsList(_tagList);
            }
        }
        /// <summary>
        /// Главный метод в основном делегирует работу других методов и возвращает результат.
        /// Метод также выполняет роль упаковщка, для спецальных тегов (Избранные, Все и т.д)
        /// </summary>
        /// <param name="collection">Принимает коллекцию объектов типа PackageBase</param>
        /// <returns>Возвращает объект оболочку для представления</returns>
        public static List<WrapPackage> WrapPackageTag<T>(List<T> @collection) where T : PackageBase, new ()
        {
            List<WrapPackage> wrapperPackage = new List<WrapPackage>();
            
            // TODO: Обратотать исключение, например ввыести сообщение об отсутвии данных...
            // WARNING: Выдает исключение если в базе отсутствует данные какой-либо категорией.
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
        private static void InitialPackages(dynamic @collection, List<WrapPackage> wrapperPackages)
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
        private static void InsertTags(dynamic @collection, List<WrapPackage> wrapperPackages, WrapPackage wrapper,  int index)
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
        /// <param name="category">Категория пакетов, необходимая для учтонения выборки.</param>
        /// <returns>Возвращает коллекцию уникальных значений.</returns>
        private static List<WrapPackage> TagFinder(string category)
        {
            string xmlDoc = "../../Resources/User/packages.xml";
            XElement root = XElement.Load(xmlDoc);

            var queryTag = (from e in root.Descendants("Package").Elements()
                where e.Name == "Tag" && e.Parent?.LastAttribute.Value == category
                select e.Value).ToList();

            var queryTags = root.Descendants("Package").Elements().Elements()
                .Where(e => e.Name == "TagList" && e.Parent?.Parent?.LastAttribute.Value == category)
                .Select(e => e.Value).ToList();

            queryTag.AddRange(queryTags);
            // Сортирует, фильтрует, выберает и преобразует в список:
            TagList = queryTag.Distinct().OrderBy(x => x.Substring(0, 3))
                .Select(element => new WrapPackage() { Name = element }).ToList();

            return TagList;
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
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
