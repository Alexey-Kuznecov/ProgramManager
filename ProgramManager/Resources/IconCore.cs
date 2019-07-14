using System.Windows;
using System.Windows.Media;

namespace ProgramManager.Resources
{
    public class IconCore
    {
        #region Constructor

        protected IconCore(string name, string color, string bg)
        {
            Name = name;
            FgroundColor = ConvertFromStringToColor(color);
            BgroundColor = ConvertFromStringToColor(bg);
            DrawIcon();
        }
        protected IconCore(string name, SolidColorBrush color, SolidColorBrush bg)
        {
            Name = name;
            FgroundColor = color;
            BgroundColor = bg;
        }
        protected IconCore(string name, DrawingBrush brush, SolidColorBrush color, SolidColorBrush bg)
        {
            Name = name;
            Brush = brush;
            FgroundColor = color;
            BgroundColor = bg;
        }

        #endregion

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
            } Brush = dBrush;
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
    public class Icon : IconCore
    {
        #region Constructor

        public Icon(string icon, string color, string bg)
            : base(icon, color, bg) { }
        public Icon(string icon, SolidColorBrush color, SolidColorBrush bg)
            : base(icon, color, bg) { }
        public Icon(string name, DrawingBrush brush, SolidColorBrush color, SolidColorBrush bg)
            : base(name, brush, color, bg) { }

        #endregion
    }
}
