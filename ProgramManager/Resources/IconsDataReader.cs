using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ProgramManager.Resources
{
    class IconsDataReader
    {
        private const string DocumentName = @"..\..\Resources\IconsData.xml";
        private static IEnumerable<XElement> _elementIconAt;
        private static IEnumerable<XElement> _elementCat;
        /// <summary>
        /// Loads xml document and get icon categories from xml file.
        /// </summary>
        /// <returns>Returns names of categories in collection.</returns>
        public static List<string> GetCategory()
        {
            List<string> categories = new List<string>();
            XElement root = XElement.Load(DocumentName);
            _elementCat = from element in root.Elements("Category") select element;

            foreach (var cat in _elementCat)
                categories.Add(cat.FirstAttribute.Value);
            return categories;
        }
        /// <summary>
        /// Extract icon attribute values from an xml file.
        /// Icon attribute repacking from xml markup to icon type.
        /// </summary>
        /// <returns>The collection containing xml elements.</returns>
        public static ObservableCollection<ButtonExtension> GetIcons()
        {
            List<IconModel> iconList = new List<IconModel>();
            _elementIconAt = from icon in _elementCat.Elements("Icon") select icon;

            foreach (var element in _elementIconAt.ToList())
                iconList.Add(new IconModel
                {
                    Name = element.Attribute("Name")?.Value,
                    FgroundColor = element.Attribute("Foreground")?.Value.FormatStringToSolidColor(),
                    BgroundColor = element.Attribute("Background")?.Value.FormatStringToSolidColor(),
                    Scale = element.Attribute("Scale")?.Value,
                    Category = element.Parent?.FirstAttribute.Value,
                    Path = CreatePathGeometry(element.Elements("Paths").ToList()),
                    // If want use brush in the project, to uncomment this is line,
                    // Brush = IconGeomertryToBrushPack(element.Elements().ToList())
                });
            // Packing an object before passing in the ViewModel.
            return InitialButtonProperties(iconList);
        }
        /// <summary>
        /// Initializes button properties of a using icon properties
        /// then packs to ObservableCollection.
        /// </summary>
        /// <param name="icons">Waiting for an object of type IconModel.</param>
        /// <returns>Returns the final object that can be transfer to the ViewModel.</returns>
        private static ObservableCollection<ButtonExtension> InitialButtonProperties(List<IconModel> icons)
        {
            ObservableCollection<ButtonExtension> buttons = new ObservableCollection<ButtonExtension>();

            foreach (var icon in icons)
            {
                buttons.Add(new ButtonExtension
                {
                    IconName = icon.Name,
                    Brush = icon.Brush,
                    Category = icon.Category,
                    Path = icon.Path,
                    ToolTip = icon.Name
                });
            }
            return buttons;
        }
        /// <summary>
        /// Extracts all elements named Path, if the paths are larger than one, 
        /// path are merged then the value is converted into Data.
        /// </summary>
        /// <param name="pathElements">Node named Paths.</param>
        /// <returns>Returns path as icon.</returns>
        private static Path CreatePathGeometry(List<XElement> pathElements)
        {
            Path path = new Path();
            string pathCancat = " ";

            foreach (var xpath in pathElements.Elements())
                pathCancat = pathCancat + xpath.Value;
            path.Data = Geometry.Parse(pathCancat);

            return path;
        }

        #region Archive

        /// <summary>
        /// Finds a icon geometric path and packs path into a brush.
        /// </summary>
        /// Elements named GeometryGroup in the xml file, that contain the geometry path.
        /// <remarks>
        /// If want use brush in the project uncomment this is line 63, then 
        /// need defined source binding on <example>Button Content="{Binding Brush}"</example> in <view cref="Views.IconsEditor"/>.
        /// </remarks>
        /// <param name="nodePaths">Node named Paths.</param>
        /// <returns>Returns a icon brush.</returns>
        private static DrawingBrush IconGeomertryToBrushPack(List<XElement> nodePaths)
        {
            List<GeometryDrawing> geometryDrawing = new List<GeometryDrawing>();
            var geometryDraw = from geometry in nodePaths select geometry;

            foreach (var path in geometryDraw.Elements())
                geometryDrawing.Add(new GeometryDrawing { Geometry = Geometry.Parse(path.Value) });

            return ConverterXamlResources.ConvertMarkupDrawingBrush(geometryDrawing);
        }

        #endregion

    }
}
