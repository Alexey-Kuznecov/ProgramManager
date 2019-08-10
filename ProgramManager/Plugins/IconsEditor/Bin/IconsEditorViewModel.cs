using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ProgramManager.Contracts;
using ProgramManager.Converters;
using ProgramManager.Models;
using ProgramManager.Plugins.IconsEditor.Converter;
using ProgramManager.Plugins.IconsEditor.Data;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using Path = System.Windows.Shapes.Path;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    /// <summary>
    /// View model for window IconsEditor. 
    /// </summary>
    partial class IconsEditorViewModel : PropertiesChanged
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private string _filterText;
        private ObservableCollection<IconsCollectionModel> _iconCollectionName;
        private ObservableCollection<ButtonExtension> _icons;
        private static ButtonExtension _buttonExtension;
        private int _selectIndex;
        private bool _enableColorIcon;
        private static string _currentCollection;

        #region Constructors

        public IconsEditorViewModel()
        {
            _dialogService = Singleton.SingleInstance<DefaultDialogService>();
            _fileService = Singleton.SingleInstance<XamlFileService>();
            // Init collection.
            IconCollectionBase.OnCollectionChanged += UpdateCollection;
            IconCollectionName = IconsCollectionModel.GetCollection();
            AddMenuItem();
            //// Loading icons...
            LoadCollection();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Contains a current the icon collection.
        /// </summary>
        public ObservableCollection<ButtonExtension> Icons
        {
            get { return _icons; }
            set
            {
                _icons = value;
                OnPropertyChanged("Icons");
            }
        }

        /// <summary>
        /// Contains a copy of the Icon object.
        /// </summary>
        public ObservableCollection<ButtonExtension> IconClone { get; set; }

        /// <summary>
        /// Contains a list names of the icon collection.
        /// </summary>
        public ObservableCollection<IconsCollectionModel> IconCollectionName
        {
            get { return _iconCollectionName; }
            set
            {
                _iconCollectionName = value;
                OnPropertyChanged("IconCollectionName");
            }
        }
        
        /// <summary>
        /// Sets the color of the icons using the Combobox value.
        /// </summary>
        public ComboBoxItem ColorBrush { get; set; }
        
        /// <summary>
        /// Contains a corrent index of the icon collection.
        /// </summary>
        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                _selectIndex = value;
                if (Icons == null)
                    return;
                LoadCollection();
                OnPropertyChanged("SelectIndex");
            }
        }

        /// <summary>
        /// Filters colection by icon name.
        /// </summary>
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;

                if (!string.IsNullOrEmpty(_filterText))
                {
                    var filtered = new ObservableCollection<ButtonExtension>();
                    var query = from icon in IconClone
                        where icon.IconName.ToLower().Contains(_filterText.ToLower())
                        select icon;

                    foreach (var button in query)
                    {
                        if (EnableColorIcon)
                        {
                            button.Style = (Style)Application.Current.FindResource("IconStylesEditorColor");
                            button.Template = Application.Current.FindResource("IconTemplateEditorColor");
                        }
                        filtered.Add(button);
                    }
                    Icons = filtered;
                } 
                else
                    Icons = IconClone;
                
                OnPropertyChanged("FilterText");
            }
        }
        
        /// <summary>
        /// Allows display color icons if property is enabled.
        /// </summary>
        public bool EnableColorIcon
        {
            get { return _enableColorIcon; }
            set
            {
                _enableColorIcon = value;
                LoadCollection();
            }
        }

        #endregion

        #region Commands
        /// <summary>
        /// The command sets the brush color, background color, and name for the icon.
        /// </summary>
        public ICommand SelectIconCommand => new RelayCommand(obj =>
        {
            ButtonExtension bt = obj as ButtonExtension;            
            IconModel iconModel = new IconModel
            {
                Name = bt?.IconName,
                Path = bt?.Path,
                FgroundColor = "#FFFFFF".StringFormatToSolidColor(),
                BgroundColor = ColorBrush.Content.ToString().StringFormatToSolidColor(),
                Scale = 254,
                Brush = bt?.Brush
            };
            OnPropertyChanged("CurrnButtonExtension");
            DataSync.IconLoad.Invoke(iconModel);
        });

        /// <summary>
        /// The command sets the default icon.
        /// </summary>
        public ICommand ResetByDefaultCommand => new RelayCommand(obj =>
        {
            DataSync.IconLoad.Invoke(null);
        });
        
        /// <summary>
        /// Close icon editor.
        /// </summary>
        public ICommand ShutdownCommand => new RelayCommand(obj =>
        {
            Application app = Application.Current;
            app.Shutdown();
        });

        /// <summary>
        /// Loads an xaml format icon from a computer and writes it to an xml file.
        /// </summary>
        public ICommand AddNewIconCommand => new RelayCommand(obj =>
        {
            if (_dialogService.OpenFileDialog())
            {
                var path = _fileService.Open(_dialogService.FilePath);
                //  filename and extract geometry path of xaml file and .
                string name = HelperFunctions.ClearExtension(_dialogService.FileShortName);
                List<Path> paths = ConverterForeignPlugins.XamlExport64PathArray(path as Viewbox);

                if (ResourceNameValidation.StoreName != null)
                    CommonProperties.IconNames.Add(name);
                // Icon data packing to saving.
                IconModel iconModel = new IconModel
                {
                    Name = name,
                    BgroundColor = ColorBrush.Content.ToString().StringFormatToSolidColor(),
                    FgroundColor = "#FFFFFF".StringFormatToSolidColor(),
                    CollectionName = _currentCollection,
                    PathList = paths,
                    Scale = 64
                };
                IconsDataWriter.Save(iconModel);
                LoadCollection();
            }
        });
        
        /// <summary>
        /// Command assign new name for icon.
        /// </summary>
        private ICommand RemaneIconCommand => new RelayCommand(name =>
        {
            string newName = (string)name;
            // Save new name to xmal file.
            IconsDataWriter.SetName(_buttonExtension.IconName, newName);
            // Update icon name without reload icon collection.
            if (_buttonExtension != null)
            {
                _buttonExtension.IconName = newName;
                _buttonExtension.ToolTip = newName;
            }
            // Sort by name and updated the collection.
            Icons = Icons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            Components.InputBox.InputBox.Close();
        });
        
        #endregion

        #region Functions

        /// <summary>
        /// Updating the icon collection menu when changing
        /// the collection in the IconCollection base class.
        /// </summary>
        private void UpdateCollection(ushort id, string colName)
        {
            if (colName != null)
            {
                foreach (var bt in Icons)
                    if (bt.Id == id)
                    {
                        Icons.Remove(bt);
                        break;
                    }
                return;
            }
            IconCollectionName = IconsCollectionModel.GetCollection();
            AddMenuItem();
        }

        /// <summary>
        /// Loads collection that is selected in the collection menu.
        /// </summary>
        private void LoadCollection()
        {
            var buttons = new ObservableCollection<ButtonExtension>();
            IconsDataReader dataReader = new IconsDataReader();
            _currentCollection = IconCollectionName.ElementAt(_selectIndex).CollectionName;
            List<IconModel> icons = dataReader.GetIcons(_currentCollection);

            foreach (var icon in icons)
            {
                buttons.Add(new ButtonExtension
                {
                    Id = icon.Id,
                    IconName = icon.Name,
                    Brush = icon.Brush,
                    CollectionName = icon.CollectionName,
                    Path = icon.Path,
                    ToolTip = icon.Name
                });
            }

            #region Initializes button properties.

            foreach (var bt in buttons)
            {
                bt.CommandParameter = bt;
                bt.RemoveIcon = new RelayCommand(obj =>
                {
                    IconsDataModifier.Remove(bt.Id, bt.CollectionName);
                    Icons.Remove(obj as ButtonExtension);
                });
                bt.RenameIcon = new RelayCommand(obj =>
                {
                    _buttonExtension = obj as ButtonExtension;
                    Components.InputBox.InputBox.Show(RemaneIconCommand, Components.InputBox.Actions.Change, (obj as ButtonExtension)?.IconName);
                });
                bt.ReplaceIcon = new RelayCommand(obj =>
                {
                    IconsDataWriter.IconReplace(((ButtonExtension)obj).Id, "Неподшитые", "Игры");
                    Icons.Remove((ButtonExtension)obj);
                });
                bt.Color = "#1A1E24".StringFormatToSolidColor();
                bt.Template = EnableColorIcon
                    ? Application.Current.FindResource("IconTemplateEditorColor")
                    : Application.Current.FindResource("IconTemplateEditor");
                bt.Style = EnableColorIcon
                    ? (Style)Application.Current.FindResource("IconStylesEditorColor")
                    : (Style)Application.Current.FindResource("IconStylesEditor");
            }

            #endregion

            Icons = buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            IconClone = Icons;
        }

        /// <summary>
        /// Adds contextmenu item for the command of adding.
        /// </summary>
        private void AddMenuItem()
        {
            foreach (var category in IconCollectionName)
            {
                category.CollectionContextMenu.Items.Insert(0, new MenuItem
                {
                    Header = "Добавить иконку",
                    Command = AddNewIconCommand,
                    CommandParameter = category.CollectionName
                });
            }
        }

        /// <summary>
        /// Serializes icons data in the file optimization data load.
        /// </summary>
        /// <param name="iconData">Expect packed data icons.</param>
        private void SerializeIconData(ObservableCollection<ButtonExtension> iconData)
        {
            object data = null;
            if (!File.Exists("button_icons"))
                Serialization.BinSerialize(iconData, "button_icons");
            else
                Serialization.BinDeserialize(out data, "button_icons");
            Icons = (ObservableCollection<ButtonExtension>)data;
        }

        /// <summary>
        /// Clear fields after build object.
        /// </summary>
        public void Dispose()
        {
            Singleton.Status = true;
        }
        
        #endregion
    }
}
