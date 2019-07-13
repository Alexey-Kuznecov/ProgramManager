using System.Windows;
using System.Windows.Media;

namespace ProgramManager.Resources
{
    public class Icon : IconCore
    {
        public Icon(string icon, string color, string bg)
            : base(icon, color, bg) { }
        public Icon() { }
    }
    public class IconCore
    {
        protected IconCore(string name, string color, string bg)
        {
            Name = name;
            FgroundColor = ConvertFromStringToColor(color);
            BgroundColor = ConvertFromStringToColor(bg);
            DrawIcon();
        }
        public IconCore() { }

        #region Properties

        public DrawingBrush Brush { get; set; }
        public SolidColorBrush BgroundColor { get; set; }
        public SolidColorBrush FgroundColor { get; set; }
        public string Name { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Устанавливает иконку, использует ресурсы иконок.
        /// Устанавливает цвет кисти.
        /// </summary>
        private void DrawIcon()
        {
            Brush = (DrawingBrush)Application.Current.FindResource(Name);
            DrawingBrush dBrush = Brush?.Clone();
            DrawingGroup group = dBrush?.Drawing as DrawingGroup;
            if (group != null)
            {
                foreach (var child in group.Children)
                {
                    GeometryDrawing geometry = child as GeometryDrawing;
                    if (geometry != null) geometry.Brush = FgroundColor;
                }
            }
            Brush = dBrush;
        }
        /// <summary>
        /// Преобразует строку формата #FFFFFF в цвет для кисти.
        /// </summary>
        private SolidColorBrush ConvertFromStringToColor(string color)
        {
            return color.FormatStringToSolidColor();
        }

        #endregion
    } 
}
