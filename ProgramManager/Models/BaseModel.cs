using System.Collections.ObjectModel;
using ProgramManager.Filters;
using ProgramManager.Models.NewModel;
using ProgramManager.ViewModels;

namespace ProgramManager.Models
{
    public class BaseModel
    {
        private static ObservableCollection<PackageBase> _packages;

        public void AddNewPackage(object sender, ConnectorEventArgs e)
        {
            //_packageAccess.AddPackage(e.Package);
        }

        public static ObservableCollection<PackageBase> GetPackages(CategoryModel category)
        {
            if (category.Name == "Программы")
            {
                _packages = new ObservableCollection<PackageBase>(ProgramAccess.GetPackages(category));
                return _packages;
            }
            else if (category.Name == "Драйвера")
            {
                _packages = new ObservableCollection<PackageBase>(DriverAccess.GetPackages(category));
                return _packages;
            }
            else if (category.Name == "Моды")
            {
                ModModel mods = new ModModel();
                _packages = new ObservableCollection<PackageBase>(mods.GetPackages(category));
                return _packages;
            }
            else if (category.Name == "Плагины")
            {
                PluginModel plugins = new PluginModel();
                _packages = new ObservableCollection<PackageBase>(plugins.GetPackages(category));
                return _packages;
            }
            else if (category.Name == "Игры")
            {
                GameModel games = new GameModel();
                _packages = new ObservableCollection<PackageBase>(games.GetPackages(category));
                return _packages;
            }
            return null;
        }

        public static ObservableCollection<PackageBase> GetPackages()
        {
            CategoryModel categoryModel = new CategoryModel { Name = "Программы" };
            _packages = new ObservableCollection<PackageBase>(ProgramAccess.GetPackages(categoryModel));
            return _packages;
        }

    }
}
