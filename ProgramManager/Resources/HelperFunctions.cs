using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
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
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
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
                // ReSharper disable once PossibleNullReferenceException
                new SolidColorBrush((Color)ConvertFromString(value));
            return solid;
        }
        /// <summary>
        /// Решает проблему: Указанный элемент уже является логическим дочерним для другого элемента. Сначала отсоедините его.
        /// </summary>
        /// <param name="item">Любой потомок класса Controls например (Кнопка).</param>
        [Conditional("DEBUG")]
        public static void RemoveFromParent(this FrameworkElement item)
        {
            var parentItemsControl = (WrapPanel) item?.Parent;
            parentItemsControl?.Children.Remove(item as UIElement);
        }
        /// <summary>
        /// Бинарная сериализация данных объектов.
        /// Внимание: Если объект наследует другие объекты они должны быть также помечены.
        /// </summary>
        /// <param name="obj">Любой объект который отмечен как сериaлизуемый.</param>
        [Conditional("DEBUG")]
        public static void BinSerialize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(@"../../Buttons.bin", FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, obj);
            }
        }
        /// <summary>
        /// Бинарная десериализация данных объектов из файла.
        /// </summary>
        /// <param name="obj">Возвращает объект, который необходимо будет привести к объекту,
        /// который подвергался сериализации.</param>
        public static void BinDeserialize(out object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(@"../../Buttons.bin", FileMode.OpenOrCreate))
            {
                obj = bf.Deserialize(fs);
            }
        }
        /// <summary>
        /// Ищет словарь по ссылкам объяденненых словарей ресурсов,
        /// данным метод не ищет ресурсы в главном словаре App.xaml.
        /// </summary>
        /// <param name="resourceName">Имя словаря ресурсов.</param>
        /// <returns>Возвращает словарь ресурсов.</returns>
        [DebuggerStepThrough]
        public static ResourceDictionary GetResourceDictionary(string resourceName)
        {
            Collection<ResourceDictionary> collMergedDictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary resourceDictionary = collMergedDictionaries.Single(p => p.Source.ToString().Contains(resourceName));
            return resourceDictionary;
        }
        /// <summary>
        /// Выводит хеш-код и тип.
        /// </summary>
        [Conditional("DEBUG")]
        public static void MessageBoxExtension(object obj)
        {
            MessageBox.Show(obj.GetHashCode().ToString(), obj.GetType().FullName);
        }
        /// <summary>
        /// Упаковывает элементы перечислителя в отслеживаемую коллекцию. 
        /// Используется как и стандартный метод расширения ToList.
        /// </summary>
        /// <typeparam name="T">Любой тип.</typeparam>
        /// <param name="collect">Перечисляемая коллекция.</param>
        /// <returns>Возвращает отслеживаемую коллекцию коллекция</returns>
        [DebuggerStepThrough]
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collect )
        {
            var ob = new ObservableCollection<T>();
            foreach (var item in collect)
                ob.Add(item);
            return ob;
        }
    }
}
