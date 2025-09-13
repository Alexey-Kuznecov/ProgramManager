
namespace ProgramManager.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Models;
    using static System.Windows.Media.ColorConverter;

    /// <summary>
    /// The color converter solid color.
    /// </summary>
    public class ColorConverterSolidColor : BaseConverter<ColorConverterSolidColor>
    {   
        /// <summary>
        /// The convert.
        /// </summary>   
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        /// The convert.
        /// </summary>   
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem combobox)
            {
                // ReSharper disable once PossibleNullReferenceException
                return new SolidColorBrush((Color)ConvertFromString(combobox.Content.ToString()));
            } 

            return null;
        }
    }

    /// <summary>
    /// The scale converter.
    /// </summary>
    public class ScaleConverter : BaseConverter<ScaleConverter>
    {
        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public double Scale { get; set; } = 12;
        
        /// <summary>
        /// The convert.
        /// </summary>   
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double num = double.Parse(value.ToString());
                return (num * (Scale / 100));
            }

            return null;
        }

        /// <summary>
        /// The convert back.
        /// </summary>   
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// The brush converter.
    /// </summary>
    public class BrushConverter : BaseConverter<BrushConverter>
    {     
        /// <summary>
        /// The convert.
        /// </summary>   
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IconModel icon)
            {
                Brush brush = parameter?.ToString() == "rect" 
                    ? icon.BackgroundColor 
                    : icon.ForegroundColor;
                return brush;
            }

            return value;
        }
    }

    /// <summary>
    /// The content converter.
    /// </summary>
    public class ContentConverter : BaseConverter<ContentConverter>
    {
        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Path pathdata = value as Path ?? (value as IconModel)?.Path;

            if (pathdata == null)
            {
                PathGeometry path = new PathGeometry();
                DrawingGroup draw = value is IconModel
                    ? ((IconModel)value).Brush.Drawing as DrawingGroup
                    : (value as DrawingBrush)?.Drawing as DrawingGroup;

                if (draw != null)
                {
                    foreach (var drawing in draw.Children)
                    {
                        var item = (GeometryDrawing)drawing;
                        path.AddGeometry(item.Geometry);
                    }
                }

                return path;
            }
            return pathdata.Data;
        }
    }
}
