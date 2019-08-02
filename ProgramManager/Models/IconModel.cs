using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ProgramManager.Services;

namespace ProgramManager.Models
{
    [DebuggerStepThrough]
    public class IconModel
    {
        #region Constructor

        public IconModel() {}
        public IconModel(string name, string fg, string bg, string path)
        {
            Name = name;
            FgroundColor = ConvertFromStringToColor(fg);
            BgroundColor = ConvertFromStringToColor(bg);
            Path = new Path { Data = Geometry.Parse(path) };
            StringPath = path;
            DrawIcon();
        }
        public IconModel(string name, SolidColorBrush fg, SolidColorBrush bg)
        {
            Name = name;
            FgroundColor = fg;
            BgroundColor = bg;
            DrawIcon();
        }
        public IconModel(string name, Path path, SolidColorBrush fg, SolidColorBrush bg)
        {
            Name = name;
            Path = path;
            FgroundColor = fg;
            BgroundColor = bg;
        }

        #endregion

        #region Properties

        public int Id { get; set; }
        public string StringPath { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Scale { get; set; }
        public DrawingBrush Brush { get; set; }
        public SolidColorBrush BgroundColor { get; set; }
        public SolidColorBrush FgroundColor { get; set; }
        public Path Path { get; set; }
        public List<Path> PathList { get; set; }

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
