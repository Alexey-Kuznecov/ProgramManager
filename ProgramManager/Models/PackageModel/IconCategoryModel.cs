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
        protected IconCategoryBase()
        {
            ContextCatMenu = new ContextMenu();

            ContextCatMenu.Items.Add(new MenuItem
            {
                Header = "Добавить категорию",
                Command = new RelayCommand(obj => { MessageBox.Show("it works!"); })
            });
            ContextCatMenu.Items.Add(new MenuItem
            {
                Header = "Добавить разделитель",
                Command = new RelayCommand(obj => { MessageBox.Show("it works!"); })
            });
            ContextCatMenu.Items.Add(new MenuItem
            {
                Header = "Переименовать",
                Command = new RelayCommand(obj => { MessageBox.Show("it works!"); })
            });
        }
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
