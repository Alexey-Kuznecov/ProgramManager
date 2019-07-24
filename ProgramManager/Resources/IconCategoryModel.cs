using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Resources
{
    /// <summary>
    /// Class of model that responsible to way display icons collection.
    /// </summary>
    class IconCollectionBase : PropertiesChanged
    {
        /// <summary>
        /// Create context menu for collection names.
        /// </summary>
        protected IconCollectionBase()
        {
            NameContextMenu = new ContextMenu();
            NameContextMenu.Items.Add(new MenuItem { Header = "Добавить категорию", Command = new RelayCommand(GetCollection) });
            NameContextMenu.Items.Add(new MenuItem { Header = "Добавить разделитель", Command = new RelayCommand(GetCollection) });
            NameContextMenu.Items.Add(new MenuItem { Header = "Переименовать", Command = new RelayCommand(GetCollection) });
        }
        public string CollectionName { get; set; }
        public ContextMenu NameContextMenu { get; set; }
        public ObservableCollection<WrapPanel> IconCollection { get; set; }

        public static ICommand FilterCollection { get; set; }

        private void GetCollection()
        {
            MessageBox.Show("Collection");
        }
    }
    class IconCollectionModel : IconCollectionBase
    {
        /// <summary>
        /// Adds headers of icons collection.
        /// </summary>
        /// <returns>Retruns collection objects which contain 
        /// icon collection names and it context menu.</returns>
        public static ObservableCollection<IconCollectionModel> GetCategory()
        {

            var cat = new ObservableCollection<IconCollectionModel>();
            
            foreach (var name in IconsDataReader.GetCategory())
                cat.Add(new IconCollectionModel { CollectionName = name });
            return cat;
        }
    }
}
