using System;
using System.Collections.Generic;

namespace ProgramManager.Models
{
    public class CategoryAccess
    {
        internal static List<CategoryModelOb> GetSubcategories()
        {
            List<CategoryModelOb> subcategory = new List<CategoryModelOb> {

                new CategoryModelOb() { CategoryName = "Программы", TagName = "3D Редакторы"  },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "IDE" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Антивирусы" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Браузеры" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Меседжеры" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Редакторы кода" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Инструменты разработчика" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Офисные" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Оптимизатры" },
                new CategoryModelOb() { CategoryName = "Программы", TagName = "Органазеры" },
                new CategoryModelOb() { CategoryName = "Игры", TagName = "Стратегии" },
                new CategoryModelOb() { CategoryName = "Игры", TagName = "RPG" },
                new CategoryModelOb() { CategoryName = "Игры", TagName = "Shooter" },
                new CategoryModelOb() { CategoryName = "Моды", TagName = "Прически" },
                new CategoryModelOb() { CategoryName = "Моды", TagName = "Персонажи" },
                new CategoryModelOb() { CategoryName = "Моды", TagName = "Реворк" },
                new CategoryModelOb() { CategoryName = "Драйвера", TagName = "Видеокарты" },
                new CategoryModelOb() { CategoryName = "Драйвера", TagName = "Сетевые адаптары" },
                new CategoryModelOb() { CategoryName = "Драйвера", TagName = "Материнская плата" },
                new CategoryModelOb() { CategoryName = "Драйвера", TagName = "Звуковая карта" },
                new CategoryModelOb() { CategoryName = "Драйвера", TagName = "Аксессуары" },
                new CategoryModelOb() { CategoryName = "Плагины", TagName = "Для фотошопа" },
            };

            return subcategory;
        }
        internal static List<CategoryModelOb> GetCategories()
        {
            List<CategoryModelOb> catergory = new List<CategoryModelOb> {

                new CategoryModelOb() { CategoryName = "Программы" },
                new CategoryModelOb() { CategoryName = "Драйвера" },
                new CategoryModelOb() { CategoryName = "Моды" },
                new CategoryModelOb() { CategoryName = "Плагины" },
                new CategoryModelOb() { CategoryName = "Игры" }
            };

            return catergory;
        }
    }
}
