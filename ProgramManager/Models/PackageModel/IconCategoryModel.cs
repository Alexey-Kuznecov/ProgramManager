using System.Windows;
using System.Windows.Controls;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using ProgramManager.ViewModels;

namespace ProgramManager.Models.PackageModel
{
    class IconCategoryModel
    {
        public IconCategoryModel()
        {
            ContextCatMenu = new ContextMenu();

            ContextCatMenu.Items.Add(new MenuItem
            {
                Header = "Добавить иконку",
                Command = new RelayCommand(obj => { MessageBox.Show("it works!"); })
            });
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

        public string Header { get; set; }

        public ContextMenu ContextCatMenu { get; set; }
    }
}
