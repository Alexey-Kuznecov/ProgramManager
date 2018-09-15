using System.Collections.Generic;
using ProgramManager.Models;
using System.Collections.ObjectModel;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel
    {
        /// <summary>
        /// This method to save a list current state before will begin search. 
        /// </summary>
        private void FixStatePackage()
        {
            if (_filterPackages == null || _filterPackages == "")
                _storage = Packages;
            else
                Packages = _storage;
        }
        /// <summary>
        /// Repacks object for normal data output of the current package.
        /// </summary>
        /// <param name="obj">Data of the current pack.</param>
        private void SetCurrentPackage(PackageModel obj)
        {
            List<DataPanelModel> current = new List<DataPanelModel>();

            current.Add(new DataPanelModel() { Label = "Имя:", Field = obj.Name });
            current.Add(new DataPanelModel() { Label = "Автор:", Field = obj.Author });
            current.Add(new DataPanelModel() { Label = "Версия:", Field = obj.Version });
            current.Add(new DataPanelModel() { Label = "Категория:", Field = obj.Category });
            current.Add(new DataPanelModel() { Label = "Тип:", Field = obj.TagName });
            current.Add(new DataPanelModel() { Label = "Описания:", Field = obj.Description });

            _currentPackage = current;
        }
        /// <summary>
        /// Calculates packages number which correspond to the Tag and shows number in each tag.
        /// </summary>
        private void CalculateByTag()
        {
            List<CategoryModel> tags = CategoryAccess.GetSubcategories();
            int total = 0, count = 0;

            foreach (var category in tags)
            {
                for (var i = 0; i < _instancePackages.Count; i++)
                {
                    if (_instancePackages[i].TagName.Contains(category.TagName))
                    {
                        total++;
                    }
                }
                tags[count].Count = total;
                total = 0;
                count++;
            }

            Tags = new ObservableCollection<CategoryModel>(tags);
        }
        /// <summary>
        /// Calculates packages number which correspond to the Category and shows number in item "All".
        /// </summary>
        private void CalculateByCategory()
        {
            List<CategoryModel> categories = CategoryAccess.GetCategories();
            int total = 0, count = 0;

            foreach (var category in categories)
            {
                for (var i = 0; i < _instancePackages.Count; i++)
                {
                    if (_instancePackages[i].Category == category.CategoryName)
                    {
                        total++;
                    }
                }
                Category[count].Count = total;
                total = 0;
                count++;
            }
        }
    }
}
