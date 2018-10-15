using System.Collections.Generic;
using ProgramManager.Models.PackageDerives;

namespace ProgramManager.Models
{
    public class CategoryModel
    {
        public string Name { get; private set; }

        public PackageBase PackageType { get; private set; }

        public static List<CategoryModel> Categories => new List<CategoryModel>()
        {
            new CategoryModel() { Name = "Программы", PackageType = new ProgramModel()},
            new CategoryModel() { Name = "Драйвера", PackageType = new DriverModel()},
            new CategoryModel() { Name = "Моды", PackageType = new ModModel()},
            new CategoryModel() { Name = "Плагины", PackageType = new PluginModel()},
            new CategoryModel() { Name = "Игры", PackageType = new GameModel()},
        };
    }
}
