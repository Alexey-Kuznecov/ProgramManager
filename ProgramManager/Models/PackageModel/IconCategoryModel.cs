using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ProgramManager.Resources;
using ProgramManager.ViewModels.Base;

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

            foreach (var header in IconsOptionReader.GetCategory())
                cat.Add(new IconCategoryModel { Header = header });

            var dd = IconsOptionReader.GetIcons();
            return cat;
        }
    }
}
