using System;
using System.Collections.Generic;

namespace ProgramManager.Models
{
    public class CategoryAccess
    {
        internal static List<CategoryModel> GetSubcategories()
        {
            List<CategoryModel> subcategory = new List<CategoryModel> {

                new CategoryModel() { CategoryName = "Программы", TagName = "3D Редакторы"  },
                new CategoryModel() { CategoryName = "Программы", TagName = "IDE" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Антивирусы" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Браузеры" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Меседжеры" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Редакторы кода" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Инструменты разработчика" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Офисные" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Оптимизатры" },
                new CategoryModel() { CategoryName = "Программы", TagName = "Органазеры" },
                new CategoryModel() { CategoryName = "Игры", TagName = "Стратегии" },
                new CategoryModel() { CategoryName = "Игры", TagName = "RPG" },
                new CategoryModel() { CategoryName = "Игры", TagName = "Shooter" },
                new CategoryModel() { CategoryName = "Моды", TagName = "Прически" },
                new CategoryModel() { CategoryName = "Моды", TagName = "Персонажи" },
                new CategoryModel() { CategoryName = "Моды", TagName = "Реворк" },
                new CategoryModel() { CategoryName = "Драйвера", TagName = "Видеокарты" },
                new CategoryModel() { CategoryName = "Драйвера", TagName = "Сетевые адаптары" },
                new CategoryModel() { CategoryName = "Драйвера", TagName = "Материнская плата" },
                new CategoryModel() { CategoryName = "Драйвера", TagName = "Звуковая карта" },
                new CategoryModel() { CategoryName = "Драйвера", TagName = "Аксессуары" },
                new CategoryModel() { CategoryName = "Плагины", TagName = "Для фотошопа" },
            };

            return subcategory;
        }
        internal static List<CategoryModel> GetCategories()
        {
            List<CategoryModel> catergory = new List<CategoryModel> {

                new CategoryModel() { CategoryName = "Программы" },
                new CategoryModel() { CategoryName = "Драйвера" },
                new CategoryModel() { CategoryName = "Моды" },
                new CategoryModel() { CategoryName = "Плагины" },
                new CategoryModel() { CategoryName = "Игры" }
            };

            return catergory;
        }
    }
}
