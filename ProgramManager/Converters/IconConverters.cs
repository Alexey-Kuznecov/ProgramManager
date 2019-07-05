using ProgramManager.MarkupExtensions;
using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

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
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color brush = (value as SolidColorBrush).Color;
            return brush;
        }
    }

    public class ContentConverter : ConverterBase<ContentConverter>, IValueConverter {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (true)
            {
                DrawingGroup draw = (value as DrawingBrush).Drawing as DrawingGroup;
                PathGeometry path = new PathGeometry();
                foreach (GeometryDrawing item in draw.Children)
                {
                    path.AddGeometry(item.Geometry);
                }
                return path;
            }
            return "";
        }
    }
}
