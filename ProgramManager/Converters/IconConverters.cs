using ProgramManager.MarkupExtensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Controls;
using ProgramManager.Resources;
using static System.Windows.Media.ColorConverter;

namespace ProgramManager.Converters
{
    public class ColorConverterSolidColor : ConverterBase<ColorConverterSolidColor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var combobox = value as ComboBoxItem;
            if (combobox != null)
                // ReSharper disable once PossibleNullReferenceException
                return new SolidColorBrush((Color)ConvertFromString(combobox.Content.ToString()));
            return null;
        }
    }
    public class ScaleConverter : ConverterBase<ScaleConverter>, IValueConverter {
        public double Scale { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double num = (double)value;
                return (num * (Scale / 100));
            }
            return null;
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
