using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ProgramManager.Resources
{
    class ConverterForeignPlugins
    {
        /// <summary>
        /// Метод конвертирует xaml разметку сгенерированную плагином 
        /// для илюстратора XamlExport64, в читаемый вид для редактора иконок.
        /// </summary>
        /// <param name="rootXaml">Корневой элементы плагина XamlExport64.</param>
        /// <returns></returns>
        public static List<Path> XamlExport64(Viewbox rootXaml)
        {            
            Canvas canvas = rootXaml.Child as Canvas;
            List<Path> paths = new List<Path>();
            object check = null;

            while (check?.GetType() != typeof(Path))
            {
                foreach (var child in canvas.Children)
                {
                    check = child;
                    if (child is Path)
                        paths.Add(child as Path);
                    else
                        canvas = (Canvas)child;
                }
            } return paths;
        }
    }
}
