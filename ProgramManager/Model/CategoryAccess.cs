using System;
using System.Collections.Generic;

namespace ProgramManager.Model
{
    public class CategoryAccess
    {
        internal static List<CategoryModel> GetSubcategories()
        {
            List<CategoryModel> subcatergory = new List<CategoryModel> {

                new CategoryModel() { CategoryName = "Все", SubcategoryName = "Все" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "3D Редакторы"  },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "IDE" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Антивирусы" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Браузеры" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Меседжеры" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Редакторы кода" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Инструменты вебразработчика" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Офисные" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Оптимизатры" },
                new CategoryModel() { CategoryName = "Программы", SubcategoryName = "Органазеры" },
                new CategoryModel() { CategoryName = "Игры", SubcategoryName = "Стратегии" },
                new CategoryModel() { CategoryName = "Игры", SubcategoryName = "RPG" },
                new CategoryModel() { CategoryName = "Игры", SubcategoryName = "Shooter" },
                new CategoryModel() { CategoryName = "Моды", SubcategoryName = "Прически" },
                new CategoryModel() { CategoryName = "Моды", SubcategoryName = "Персонажи" },
                new CategoryModel() { CategoryName = "Моды", SubcategoryName = "Реворк" },
                new CategoryModel() { CategoryName = "Драйвера", SubcategoryName = "Видеокарты" },
                new CategoryModel() { CategoryName = "Драйвера", SubcategoryName = "Сетевые адаптары" },
                new CategoryModel() { CategoryName = "Драйвера", SubcategoryName = "Материнская плата" },
                new CategoryModel() { CategoryName = "Драйвера", SubcategoryName = "Звуковая карта" },
                new CategoryModel() { CategoryName = "Драйвера", SubcategoryName = "Аксессуары" },
                new CategoryModel() { CategoryName = "Плагины", SubcategoryName = "Для фотошопа" },
            };

            return subcatergory;
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
