using ProgramManager.MarkupExtensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using ProgramManager.Resources;

namespace ProgramManager.Converters
{
    public class ScaleConverter : ConverterBase<ScaleConverter>, IValueConverter {
        public double Scale { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double num = (double)value;
            return (num * (Scale / 100));
        }
    }
    public class BrushConverter : ConverterBase<BrushConverter>, IValueConverter {
        [SuppressMessage("ReSharper", "PossibleInvalidCastExceptionInForeachLoop")]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Icon icon = value as Icon;
            if (icon != null)
            {
                Brush brush = parameter?.ToString() == "rect" ? icon.BgroundColor : icon.FgroundColor;
                return brush;
            }
            return value;
        }
    }
    public class ContentConverter : ConverterBase<ContentConverter>, IValueConverter {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PathGeometry path = new PathGeometry();
            DrawingGroup draw = value is Icon
                ? (value as Icon).Brush.Drawing as DrawingGroup
                : (value as DrawingBrush)?.Drawing as DrawingGroup;

            if (draw != null)
                foreach (var drawing in draw.Children)
                {
                    var item = (GeometryDrawing) drawing;
                    path.AddGeometry(item.Geometry);
                }
            return path;
        }
    }
}
