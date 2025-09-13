

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
