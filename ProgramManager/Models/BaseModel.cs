using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProgramManager.Filters;
using ProgramManager.Models.NewModel;
using ProgramManager.ViewModels;

namespace ProgramManager.Models
{
    public class BaseModel
    {
        private static ObservableCollection<PackageBase> _packages;
        private static ObservableCollection<WrapPackages> _tags;

        public void AddNewPackage(object sender, ConnectorEventArgs e)
        {
            //_packageAccess.AddPackage(e.Package);
        }

        public static ObservableCollection<WrapPackages> GetPackages(CategoryModel category)
        {
            if (category.Name == "Программы")
            {
                _tags = new ObservableCollection<WrapPackages>(WrapPackages.WrapTagModel(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[0])));
                return _tags;
            }
            else if (category.Name == "Драйвера")
            {
                _tags = new ObservableCollection<WrapPackages>(WrapPackages.WrapTagModel(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[1])));
                return _tags;
            }
            else if (category.Name == "Моды")
            {
                _tags = new ObservableCollection<WrapPackages>(WrapPackages.WrapTagModel(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[2])));
                return _tags;
            }
            else if (category.Name == "Плагины")
            {
                _tags = new ObservableCollection<WrapPackages>(WrapPackages.WrapTagModel(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[3])));
                return _tags;
            }
            else if (category.Name == "Игры")
            {
                _tags = new ObservableCollection<WrapPackages>(WrapPackages.WrapTagModel(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[4])));
                return _tags;
            }
            return null;
        }
        public static ObservableCollection<WrapPackages> GetPackages()
        {
            CategoryModel.Categories = new List<CategoryModel>()
            {
                new CategoryModel() { Name = "Программы" },
                new CategoryModel() { Name = "Драйвера" },
                new CategoryModel() { Name = "Моды" },
                new CategoryModel() { Name = "Плагины" },
                new CategoryModel() { Name = "Игры" },
            };

            _tags = new ObservableCollection<WrapPackages>(WrapPackages.WrapTagModel(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[0])));
            return _tags;
        }

    }
}
