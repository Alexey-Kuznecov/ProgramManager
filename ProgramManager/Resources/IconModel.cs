using System.Windows;
using System.Windows.Media;

namespace ProgramManager.Resources
{
    public class IconCore
    {
        #region Constructor

        protected IconCore(string name, string fg, string bg)
        {
            Name = name;
            FgroundColor = ConvertFromStringToColor(fg);
            BgroundColor = ConvertFromStringToColor(bg);
            DrawIcon();
        }
        protected IconCore(string name, SolidColorBrush fg, SolidColorBrush bg)
        {
            Name = name;
            FgroundColor = fg;
            BgroundColor = bg;
        }
        protected IconCore(string name, DrawingBrush brush, SolidColorBrush fg, SolidColorBrush bg)
        {
            Name = name;
            Brush = brush;
            FgroundColor = fg;
            BgroundColor = bg;
        }

        #endregion

        #region Properties

        public DrawingBrush Brush { get; set; }
        public SolidColorBrush BgroundColor { get; set; }
        public SolidColorBrush FgroundColor { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Устанавливает иконку, использует ресурсы иконок.
        /// Устанавливает цвет кисти.
        /// </summary>
        private void DrawIcon()
        {
            Brush = (DrawingBrush)Application.Current.TryFindResource(Name);
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
    public class IconModel : IconCore
    {
        #region Constructor

        public IconModel(string icon, string fg, string bg)
            : base(icon, fg, bg) { }
        public IconModel(string icon, SolidColorBrush fg, SolidColorBrush bg)
            : base(icon, fg, bg) { }
        public IconModel(string name, DrawingBrush brush, SolidColorBrush fg, SolidColorBrush bg)
            : base(name, brush, fg, bg) { }

        #endregion
    }
}
