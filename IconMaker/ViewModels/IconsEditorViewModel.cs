using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using IconMaker.Converter;
using IconMaker.Models;

namespace IconMaker.ViewModels
{
    using AlexLibWpf.Help;
    using AlexLibWpf.Patterns;
    using AlexLibWpf.Mvvm.Base;
    using AlexLibWpf.Models;
    using AlexLibWpf.Contracts;
    using AlexLibWpf.Services;
    /// <summary>
    /// View model for window <see cref="IconsEditor"/>. 
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

        /// <summary>
        /// </summary>
        private static string _currentCollection;

        #region Constructors

        /// <summary>
        /// </summary>
        public IconsEditorViewModel()
        {
            this._dialogService = Singleton.SingleInstance<DefaultDialogService>();
            this._fileService = Singleton.SingleInstance<XamlFileService>();
            // Init collection.
            IconCollectionBase.OnCollectionChanged += this.UpdateCollection;
            this.IconCollectionName = IconsCollectionModel.GetCollection();
            this.AddMenuItem();
            //// Loading icons...
            this.LoadCollection();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains a current the icon collection.
        /// </summary>
        public ObservableCollection<ButtonExtension> Icons
        {
            get { return this._icons; }
            set
            {
                this._icons = value;
                this.OnPropertyChanged("Icons");
            }
        }

        /// <summary>
        /// Contains a copy of the Icon <c>object</c>.
        /// </summary>
        public ObservableCollection<ButtonExtension> IconClone { get; set; }

        /// <summary>
        /// Contains a list names of the icon collection.
        /// </summary>
        public ObservableCollection<IconsCollectionModel> IconCollectionName
        {
            get { return this._iconCollectionName; }
            set
            {
                this._iconCollectionName = value;
                this.OnPropertyChanged("IconCollectionName");
            }
        }

        /// <summary>
        /// Sets the color of the icons using the Combobox value.
        /// </summary>
        public ComboBoxItem ColorBrush { get; set; }

        /// <summary>
        /// Contains a correct index of the icon collection.
        /// </summary>
        public int SelectIndex
        {
            get { return this._selectIndex; }
            set
            {
                this._selectIndex = value;
                if (this.Icons == null)
                    return;
                this.LoadCollection();
                this.OnPropertyChanged("SelectIndex");
            }
        }

        /// <summary>
        /// Filters collection by icon name.
        /// </summary>
        public string FilterText
        {
            get { return this._filterText; }
            set
            {
                this._filterText = value;

                if (!string.IsNullOrEmpty(this._filterText))
                {
                    var filtered = new ObservableCollection<ButtonExtension>();
                    var query = from icon in this.IconClone
                                where icon.IconName.ToLower().Contains(this._filterText.ToLower())
                                select icon;

                    foreach (var button in query)
                    {
                        if (this.EnableColorIcon)
                        {
                            button.Style = (Style)Application.Current.FindResource("IconStylesEditorColor");
                            button.Template = Application.Current.FindResource("IconTemplateEditorColor");
                        }
                        filtered.Add(button);
                    }
                    this.Icons = filtered;
                }
                else
                    this.Icons = this.IconClone;

                this.OnPropertyChanged("FilterText");
            }
        }

        /// <summary>
        /// Allows display color icons if property is enabled.
        /// </summary>
        public bool EnableColorIcon
        {
            get { return this._enableColorIcon; }
            set
            {
                this._enableColorIcon = value;
                this.LoadCollection();
            }
        }

        #endregion

        #region Commands
        /// <summary>
        /// The command sets the brush color, background color, and name for the icon.
        /// </summary>
        public ICommand SelectIconCommand => new RelayCommand(obj =>
        {
            //ButtonExtension bt = obj as ButtonExtension;            
            //IconModel iconModel = new IconModel
            //{
            //    Name = bt?.IconName,
            //    Path = bt?.Path,
            //    FgroundColor = "#FFFFFF".StringFormatToSolidColor(),
            //    BgroundColor = ColorBrush.Content.ToString().StringFormatToSolidColor(),
            //    Scale = 254,
            //    Brush = bt?.Brush
            //};
            //OnPropertyChanged("CurrnButtonExtension");
            //DataSync.IconLoad.Invoke(iconModel);
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
            if (this._dialogService.OpenFileDialog())
            {
                var path = this._fileService.Open(this._dialogService.FilePath);
                //  filename and extract geometry path of xaml file and .
                string name = HelperFunctions.ClearExtension(this._dialogService.FileShortName);
                List<Path> paths = ConverterForeignPlugins.XamlExport64PathArray(path as Viewbox);

                // Icon data packing to saving.
                IconModel iconModel = new IconModel
                {
                    Name = name,
                    BgroundColor = this.ColorBrush.Content.ToString().StringFormatToSolidColor(),
                    FgroundColor = "#FFFFFF".StringFormatToSolidColor(),
                    CollectionName = _currentCollection,
                    PathList = paths,
                    Scale = 64
                };
                IconsDataWriter.Save(iconModel);
                this.LoadCollection();
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
            this.Icons = this.Icons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            AlexLibWpf.Components.InputBox.InputBox.Close();
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
                foreach (var bt in this.Icons)
                    if (bt.Id == id)
                    {
                        this.Icons.Remove(bt);
                        break;
                    }
                return;
            }
            this.IconCollectionName = IconsCollectionModel.GetCollection();
            this.AddMenuItem();
        }

        /// <summary>
        /// Loads collection that is selected in the collection menu.
        /// </summary>
        private void LoadCollection()
        {
            var buttons = new ObservableCollection<ButtonExtension>();
            IconsDataReader dataReader = new IconsDataReader();
            _currentCollection = this._selectIndex != -1
                ? this.IconCollectionName.ElementAt(this._selectIndex).CollectionName
                : this.IconCollectionName.ElementAt(this.IconCollectionName.Count - 1).CollectionName;
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
                    this.Icons.Remove(obj as ButtonExtension);
                });
                bt.RenameIcon = new RelayCommand(obj =>
                {
                    _buttonExtension = obj as ButtonExtension;
                    AlexLibWpf.Components.InputBox.InputBox.Show(this.RemaneIconCommand, AlexLibWpf.Components.InputBox.Actions.Change, (obj as ButtonExtension)?.IconName);
                });
                bt.ReplaceIcon = new RelayCommand(obj =>
                {
                    IconsDataWriter.IconReplace(((ButtonExtension)obj).Id, "Неподшитые", "Игры");
                    this.Icons.Remove((ButtonExtension)obj);
                });
                bt.Color = "#1A1E24".StringFormatToSolidColor();
                bt.Template = this.EnableColorIcon
                    ? Application.Current.FindResource("IconTemplateEditorColor")
                    : Application.Current.FindResource("IconTemplateEditor");
                bt.Style = this.EnableColorIcon
                    ? (Style)Application.Current.FindResource("IconStylesEditorColor")
                    : (Style)Application.Current.FindResource("IconStylesEditor");
            }

            #endregion

            try
            {
                this.Icons = buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            }
            catch (ArgumentOutOfRangeException)
            {
                this.Icons = buttons.OrderBy(p => p.IconName.Substring(0, 1)).ToObservableCollection();
            }
            this.IconClone = this.Icons;
        }

        /// <summary>
        /// Adds contextmenu item for the command of adding.
        /// </summary>
        private void AddMenuItem()
        {
            foreach (var category in this.IconCollectionName)
            {
                category.CollectionContextMenu.Items.Insert(0, new MenuItem
                {
                    Header = "Добавить иконку",
                    Command = this.AddNewIconCommand,
                    CommandParameter = category.CollectionName
                });
            }
        }

        /// <summary>
        /// Clear fields after build object.
        /// </summary>
        public void Dispose()
        {
            Singleton.Status = true;
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.ColorBrush != null);
        }
    }
}
