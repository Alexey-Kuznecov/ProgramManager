using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<IconsCollectionModel> _iconCollection;
        private ObservableCollection<ButtonExtension> _buttons;
        private int _selectIndex;
        private bool _enableColorIcon;
        private ButtonExtension _currentButtonExtension;
        private static ButtonExtension _buttonExtension;
        private static string _currentCollection;

        #region Constructors

        public IconsEditorViewModel()
        {
            _dialogService = Singleton.SingleInstance<DefaultDialogService>();
            _fileService = Singleton.SingleInstance<XamlFileService>();
            // Init collection.
            IconCollectionBase.OnCollectionChanged += UpdateCollection;
            IconCollection = IconsCollectionModel.GetCollection();

            AddMenuItem();
            //// Loading icons...
            LoadCollection();
        }
        
        #endregion

        #region Properties

        public ButtonExtension CurrentButtonExtension
        {
            get { return _currentButtonExtension; }
            set
            {
                _currentButtonExtension = value;
                OnPropertyChanged("CurrnButtonExtension");
            }
        }
        
        /// <summary>
        /// Contains a current name of the icon collection.
        /// </summary>
        public ObservableCollection<ButtonExtension> Buttons
        {
            get { return _buttons; }
            set
            {
                _buttons = value;
                OnPropertyChanged("Buttons");
            }
        }

        public ObservableCollection<ButtonExtension> ButtonsClone { get; set; }

        public ObservableCollection<IconsCollectionModel> IconCollection
        {
            get { return _iconCollection; }
            set
            {
                _iconCollection = value;
                OnPropertyChanged("IconCollection");
            }
        }
        /// <summary>
        /// Sets the color of the icons using the Combobox value.
        /// </summary>
        public ComboBoxItem ColorBrush { get; set; }

        public DrawingBrush IconBrush { get; set; }
        
        /// <summary>
        /// Contains a corrent index of the icon collection.
        /// </summary>
        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                _selectIndex = value;
                if (Buttons == null)
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
                    var query = from button in ButtonsClone
                        where button.IconName.ToLower().Contains(_filterText.ToLower())
                        select button;

                    foreach (var button in query)
                    {
                        if (EnableColorIcon)
                        {
                            button.Style = (Style)Application.Current.FindResource("IconStylesEditorColor");
                            button.Template = Application.Current.FindResource("IconTemplateEditorColor");
                        }
                        filtered.Add(button);
                    }
                    Buttons = filtered;
                } 
                else
                    Buttons = ButtonsClone;

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
        /// Команда устанавлевает цвет кисти, цвет фона и имя для иконки.
        /// </summary>
        public ICommand SelectIconCommand => new RelayCommand(obj =>
        {
            ButtonExtension bt = obj as ButtonExtension;
            CurrentButtonExtension = bt;
            OnPropertyChanged("CurrnButtonExtension");
            //IconModel iconModel = new IconModel
            //{
            //    Name = bt?.IconName,
            //    Path = bt?.Path,
            //    FgroundColor = "#FFFFFF".FormatStringToSolidColor(),
            //    BgroundColor = ColorBrush.Content.ToString().FormatStringToSolidColor(),
            //    Scale = 254,
            //    Brush = bt?.Brush
            //};
            //DataSync.IconLoad.Invoke(iconModel);
        });

        /// <summary>
        /// The command sets the default icon.
        /// </summary>
        public ICommand ResetByDefault => new RelayCommand(obj =>
        {
            DataSync.IconLoad.Invoke(null);
        });
        
        /// <summary>
        /// Close icon editor.
        /// </summary>
        public ICommand Shutdown => new RelayCommand(obj =>
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
        private ICommand RemaneIcon => new RelayCommand(name =>
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
            Buttons = Buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            Components.InputBox.InputBox.Close();
        });
        
        #endregion

        #region Functions

        /// <summary>
        /// Updating the icon collection menu when changing
        /// the collection in the IconCollection base class.
        /// </summary>
        private void UpdateCollection()
        {
            IconCollection = IconsCollectionModel.GetCollection();
            AddMenuItem();
        }

        /// <summary>
        /// Loads collection that is selected in the collection menu.
        /// </summary>
        private void LoadCollection()
        {
            var buttons = new ObservableCollection<ButtonExtension>();
            IconsDataReader dataReader = new IconsDataReader();
            _currentCollection = IconCollection.ElementAt(_selectIndex).CollectionName;
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
                    IconDataModifier.Remove(bt.Id, bt.CollectionName);
                    Buttons.Remove(obj as ButtonExtension);
                });
                bt.RenameIcon = new RelayCommand(obj =>
                {
                    _buttonExtension = obj as ButtonExtension;
                    Components.InputBox.InputBox.Show(RemaneIcon, Components.InputBox.Actions.Change, (obj as ButtonExtension)?.IconName);
                });
                bt.ReplaceIcon = new RelayCommand(obj =>
                {
                    IconsDataWriter.IconReplace(((ButtonExtension)obj).Id, "Неподшитые", "Игры");
                    Buttons.Remove((ButtonExtension)obj);
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

            Buttons = buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            ButtonsClone = Buttons;
        }

        /// <summary>
        /// Adds contextmenu item for the command of adding.
        /// </summary>
        private void AddMenuItem()
        {
            foreach (var category in IconCollection)
            {
                category.NameContextMenu.Items.Insert(0, new MenuItem
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
            Buttons = (ObservableCollection<ButtonExtension>)data;
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
