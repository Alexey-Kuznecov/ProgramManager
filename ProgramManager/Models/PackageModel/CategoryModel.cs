<<<<<<< HEAD
﻿using System.Collections.Generic;

namespace ProgramManager.Models.PackageModel
{
    public class CategoryModel
    {
        public string Name { get; private set; }

        public PackageBase PackageType { get; private set; } = new ProgramModel();

        public static List<CategoryModel> Categories => new List<CategoryModel>
        {
            new CategoryModel { Name = "Программы", PackageType = new ProgramModel() },
            new CategoryModel { Name = "Драйвера", PackageType = new DriverModel() },
            new CategoryModel { Name = "Моды", PackageType = new ModModel() },
            new CategoryModel { Name = "Плагины", PackageType = new PluginModel() },
            new CategoryModel { Name = "Игры", PackageType = new GameModel() },
        };
        public Dictionary<string, string> MenuItem { get; set; } = new Dictionary<string, string>();

        public void SetMenuItem()
        {
            MenuItem = PackageType.LoadItem.Invoke();
        }
    }
}
=======
﻿using System.Collections.Generic;
using System.Windows.Controls;
using static ProgramManager.Models.PackageModel.PackageBase;

namespace ProgramManager.Models.PackageModel
{
    public class CategoryModel
    {
        public string Name { get; private set; }

        public PackageBase PackageType { get; private set; } = new ProgramModel();

        public static List<CategoryModel> Categories => new List<CategoryModel>()
        {
            new CategoryModel() { Name = "Программы", PackageType = new ProgramModel() },
            new CategoryModel() { Name = "Драйвера", PackageType = new DriverModel() },
            new CategoryModel() { Name = "Моды", PackageType = new ModModel() },
            new CategoryModel() { Name = "Плагины", PackageType = new PluginModel() },
            new CategoryModel() { Name = "Игры", PackageType = new GameModel() },
        };
        public Dictionary<string, string> MenuItem { get; set; } = new Dictionary<string, string>() { };

        public void SetMenuItem()
        {
            MenuItem = PackageType.LoadItem.Invoke();
        }
    }
}
>>>>>>> ca87b0a1458075bdb18f5e61aba52b5e947baa11
