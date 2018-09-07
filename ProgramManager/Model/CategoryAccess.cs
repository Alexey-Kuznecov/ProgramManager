using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Model
{
    public class CategoryAccess
    {
        internal static List<CategoryModel> GetCategories()
        {
            List<CategoryModel> catergory = new List<CategoryModel> {

                new CategoryModel() {
                  CategoryId = 0,
                  CategoryName = "Все",
                  SubcategoryName = "Все"
                },
                new CategoryModel() {
                  CategoryId = 1,
                  CategoryName = "Программы",
                  SubcategoryName = "3D Редакторы" 
                },
                new CategoryModel() {
                  CategoryId = 2,
                  CategoryName = "Программы",
                  SubcategoryName = "IDE"
                },
                new CategoryModel() {
                  CategoryId = 3,
                  CategoryName = "Программы",
                  SubcategoryName = "Антивирусы"
                },
                new CategoryModel() {
                  CategoryId = 4,
                  CategoryName = "Программы",
                  SubcategoryName = "Браузеры"
                },
                new CategoryModel() {
                  CategoryId = 5,
                  CategoryName = "Программы",
                  SubcategoryName = "Меседжеры"
                },
                new CategoryModel() {
                  CategoryId = 6,
                  CategoryName = "Программы",
                  SubcategoryName = "Редакторы кода"
                },
                new CategoryModel() {
                  CategoryId = 7,
                  CategoryName = "Программы",
                  SubcategoryName = "Инструменты вебразработчика"
                },
                new CategoryModel() {
                  CategoryId = 8,
                  CategoryName = "Программы",
                  SubcategoryName = "Офисные"
                },
                new CategoryModel() {
                  CategoryId = 9,
                  CategoryName = "Программы",
                  SubcategoryName = "Оптимизатры"
                },
                new CategoryModel() {
                  CategoryId = 10,
                  CategoryName = "Программы",
                  SubcategoryName = "Органазеры"
                },
            };

            return catergory;
        }
    }
}
