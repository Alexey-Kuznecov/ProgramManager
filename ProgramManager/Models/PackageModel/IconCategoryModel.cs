using System.Collections.ObjectModel;
using System.Windows.Controls;
using ProgramManager.Resources;

namespace ProgramManager.Models.PackageModel
{
    class IconCategoryBase
    {
        public string Header { get; set; }
        public ContextMenu ContextCatMenu { get; set; }
        public ObservableCollection<WrapPanel> Categories { get; set; }
    }
    class IconCategoryModel : IconCategoryBase
    {
        public static ObservableCollection<IconCategoryModel> GetCategory()
        {
            var cat = new ObservableCollection<IconCategoryModel>();

            foreach (var header in IconsDataReader.GetCategory())
                cat.Add(new IconCategoryModel { Header = header });

            return cat;
        }
    }
}
