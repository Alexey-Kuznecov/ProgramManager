using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
            var cat = new ObservableCollection<IconCategoryModel>
            {
                new IconCategoryModel { Header = "Программы" },
                new IconCategoryModel { Header = "Логотипы" },
                new IconCategoryModel { Header = "Игры" },
                new IconCategoryModel { Header = "Бренды" },
                new IconCategoryModel { Header = "Разное" }
            };
            return cat;
        }
    }
}
