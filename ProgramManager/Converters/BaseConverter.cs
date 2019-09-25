
namespace ProgramManager.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// The <see langword="base"/> converter.
    /// </summary>
    /// <typeparam name="T"> The class converter. </typeparam>
    public abstract class BaseConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        /// <summary>
        /// The converter.
        /// </summary>
        private static T _converter = null;

        /// <summary>
        /// Must be implemented in inheritor.
        /// </summary>
        /// <param name="value"> The <c>value</c>. </param>
        /// <param name="targetType"> The target Type. </param>
        /// <param name="parameter"> The <c>parameter</c>. </param>
        /// <param name="culture"> The <paramref name="culture"/>. </param>
        /// <returns> The object. </returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Override if needed.
        /// </summary>
        /// <param name="value"> The <c>value</c>. </param>
        /// <param name="targetType"> The target Type. </param>
        /// <param name="parameter"> The <c>parameter</c>. </param>
        /// <param name="culture"> The <paramref name="culture"/>. </param>
        /// <returns> The object. </returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The provide value.
        /// </summary>
        /// <param name="serviceProvider"> The service provider. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }
    }
}
