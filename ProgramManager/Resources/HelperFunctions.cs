using System.IO;
using System.Net.Http.Headers;
using System.Windows.Media;
using static System.Windows.Media.ColorConverter;

namespace ProgramManager.Resources
{
    static class HelperFunctions
    {
        /// <summary>
        /// Удаляет путь и расширения файла, оставляет только имя.
        /// </summary>
        /// <param name="path">Путь или имя файла.</param>
        /// <returns>Возвращает имя файла.</returns>
        public static string ClearExtension(string path)
        {
            var result = path;
            do
            {
                path = result;
                result = Path.Combine(
                    Path.GetDirectoryName(path),
                    Path.GetFileNameWithoutExtension(path));
            }
            while (result != path);
            return result;
        }
        /// <summary>
        /// Преобразует шестнадцатеричное значение в цвет кисти.
        /// </summary>
        /// <param name="value">Шестнадцатеричное значение.</param>
        /// <returns>Возращает цвет кисти.</returns>
        public static SolidColorBrush FormatStringToSolidColor(this string value)
        {
            SolidColorBrush solid =
                new SolidColorBrush((Color)ConvertFromString(value));
            return solid;
        }
    }
}
