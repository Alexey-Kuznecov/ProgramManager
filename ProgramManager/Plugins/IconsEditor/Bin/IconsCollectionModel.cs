using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using ProgramManager.Components.InputBox;
using ProgramManager.Plugins.IconsEditor.Data;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    /// <summary>
    /// Class of model that responsible to way display icons collection.
    /// </summary>
    class IconCollectionBase : PropertiesChanged
    {
        public static event Action OnCollectionChanged;

        public ObservableCollection<ButtonExtension> Icons { get; set; }

        public ContextMenu NameContextMenu { get; set; }

        public string CollectionName { get; set; }

        public static ICommand AddNewCollection => new RelayCommand(name =>
        {
            using (IconsDataWriter dataWriter = new IconsDataWriter())
            {
                dataWriter.AddNewCollection((string)name);
                OnCollectionChanged?.Invoke();
                InputBox.Close();
            }
        });

        private void AddSeparator()
        {
            throw new NotImplementedException();
        }

        private void RenameCollection()
        {
            throw new NotImplementedException();
        }

        private void RemoveCollection(object obj)
        {
            using (IconsDataWriter dataWriter = new IconsDataWriter())
            {
                dataWriter.RemoveCollection(CollectionName);
                OnCollectionChanged?.Invoke();
                InputBox.Close();
            }
        }

        /// <summary>
        /// Create context menu for collection names.
        /// </summary>
        protected IconCollectionBase()
        {
            NameContextMenu = new ContextMenu();
            NameContextMenu.Items.Add(newItem: new MenuItem
            {
                Header = "Добавить категорию",
                Command = new RelayCommand(obj => InputBox.Show(AddNewCollection, Actions.Add))
            });
            NameContextMenu.Items.Add(new MenuItem { Header = "Добавить разделитель", Command = new RelayCommand(AddSeparator) });
            NameContextMenu.Items.Add(new MenuItem { Header = "Переименовать", Command = new RelayCommand(RenameCollection) });
            NameContextMenu.Items.Add(new MenuItem { Header = "Удалить", Command = new RelayCommand(RemoveCollection) });
        }
    }
    class IconsCollectionModel : IconCollectionBase
    {
        /// <summary>
        /// Adds headers of icons collection.
        /// </summary>
        /// <returns>Retruns collection objects which contain 
        /// icon collection names and it context menu.</returns>
        public static ObservableCollection<IconsCollectionModel> GetCollection()
        {
            var cat = new ObservableCollection<IconsCollectionModel>();
            using (IconsDataReader dataReader = new IconsDataReader())
            {
                foreach (var name in dataReader.GetCollection())
                    cat.Add(new IconsCollectionModel { CollectionName = name });
            }
            return cat;
        }
    }
}
