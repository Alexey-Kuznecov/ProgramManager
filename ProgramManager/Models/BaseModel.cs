using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProgramManager.Filters;
using ProgramManager.Models.NewModel;
using ProgramManager.ViewModels;

namespace ProgramManager.Models
{
    public class BaseModel
    {
        private static ObservableCollection<WrapPackage> _wrapperPackages;

        public void AddNewPackage(object sender, ConnectorEventArgs e)
        {
            //_packageAccess.AddPackage(e.Package);
        }
        public static ObservableCollection<WrapPackage> GetPackages(CategoryModel category)
        {
            _wrapperPackages = new ObservableCollection<WrapPackage>(
                WrapPackage.WrapPackageTag(ProgramAccess<ProgramModel>.GetPackages(category)));
            return _wrapperPackages;
        }
        public static ObservableCollection<WrapPackage> GetPackages()
        {
            CategoryModel.Categories = new List<CategoryModel>()
            {
                new CategoryModel() { Name = "Программы" },
                new CategoryModel() { Name = "Драйвера" },
                new CategoryModel() { Name = "Моды" },
                new CategoryModel() { Name = "Плагины" },
                new CategoryModel() { Name = "Игры" },
            };

            _wrapperPackages = new ObservableCollection<WrapPackage>(
                list: WrapPackage.WrapPackageTag(ProgramAccess<ProgramModel>.GetPackages(CategoryModel.Categories[0])));
            return _wrapperPackages;
        }

    }
}
