using System.Collections.ObjectModel;
using ProgramManager.Contracts;

namespace ProgramManager.Models
{
    public class BaseModel
    {
        private PackageAccess _packageAccess;
        private CategoryAccess _categoryAccess;

        public BaseModel()
        {
            _categoryAccess = new CategoryAccess();
            _packageAccess = new PackageAccess();
        }
        public void AddNewPackage(object sender, PackageEventArgs e)
        {
            _packageAccess.AddPackage(e.Package);
        }
        public ObservableCollection<PackageModel> GetPackages()
        {
            ObservableCollection<PackageModel> packages = new ObservableCollection<PackageModel>(PackageAccess.GetPackages());
            return packages;
        }
        public ObservableCollection<CategoryModel> GetCategories()
        {
            ObservableCollection<CategoryModel> categories = new ObservableCollection<CategoryModel>(CategoryAccess.GetCategories());
            return categories;
        }
    }
}
