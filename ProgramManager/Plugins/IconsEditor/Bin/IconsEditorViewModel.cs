using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ProgramManager.Contracts;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Models;
using ProgramManager.Plugins.IconsEditor.Converter;
using ProgramManager.Plugins.IconsEditor.Data;
using ProgramManager.Services;
using ProgramManager.ViewModels;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    /// <summary>
    /// View model for window IconsEditor. 
    /// </summary>
    class IconsEditorViewModel : PropertiesChanged , IDisposable
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private ListBoxItem _selectCategory;
        private string _filterText;
        private ObservableCollection<IconsCollectionModel> _iconCategory;
        private string _selectItem;
        private int _selectIndex;
        private bool _enableColourIcon;
        private static ObservableCollection<ButtonExtension> _buttons;
        
        #region Constructors
        public IconsEditorViewModel()
        {
            _dialogService = Singleton.SingleInstance<DefaultDialogService>();
            _fileService = Singleton.SingleInstance<XamlFileService>();
            IconCategory = IconsCollectionModel.GetCategory();
            
            // Init collection.
            IconCollectionBase.FilterCollection = new RelayCommand(name => FilterCollection((string)name));
            IconCategory = IconsCollectionModel.GetCategory();
            AddMenuItem();
            // Loading icons...
            LoadIcons();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Contains a corrent index of the icon collection.
        /// </summary>
        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                _selectIndex = value;
                OnPropertyChanged("SelectIndex");
            }
        }
        /// <summary>
        /// Contains a current name of the icon collection.
        /// </summary>
        public string SelectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                OnPropertyChanged("SelectItem");
            }
        }
        /// <summary>
        /// Устанавлевает цвет иконок из выбранного значение в Combobox.
        /// </summary>
        public ComboBoxItem ColorBrush { get; set; }
        public DrawingBrush IconBrush { get; set; }
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
        public ObservableCollection<IconsCollectionModel> IconCategory
        {
            get { return _iconCategory; }
            set
            {
                _iconCategory = value;               
                OnPropertyChanged("IconCategory");
            }
        }
        public ListBoxItem SelectedCategory
        {
            get { return _selectCategory; }
            set
            {
                _selectCategory = value;
                FilterText = _selectCategory.Content.ToString();
                OnPropertyChanged("FilterText");
            }
        }
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;

                if (string.IsNullOrEmpty(_filterText))
                {
                    Buttons = ButtonsClone;
                }
                else
                {
                    ObservableCollection<ButtonExtension> filtered 
                        = new ObservableCollection<ButtonExtension>();

                    var query = from button in ButtonsClone
                                where button.IconName.ToLower().Contains(_filterText.ToLower())
                                select button;

                    foreach (var button in query)
                        filtered.Add(button);
                    Buttons = filtered;
                }
                OnPropertyChanged("FilterText");
            }
        }
        public bool EnableColourIcon
        {
            get { return _enableColourIcon; }
            set
            {
                _enableColourIcon = value;
                LoadIcons();
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
            IconModel iconModel = new IconModel
            {
                Name = bt?.IconName,
                Path = bt?.Path,
                FgroundColor = "#FFFFFF".FormatStringToSolidColor(),
                BgroundColor = ColorBrush.Content.ToString().FormatStringToSolidColor(),
                Scale = 254,
                Brush = bt?.Brush
            };
            DataSync.IconLoad.Invoke(iconModel);
        });
        /// <summary>
        /// Команда устанавливает иконку по умолчанию.
        /// </summary>
        public ICommand ResetByDefault => new RelayCommand(obj =>
        {
            DataSync.IconLoad.Invoke(null);
        });
        /// <summary>
        /// Команда устанавливает иконку
        /// </summary>
        public ICommand Shutdown => new RelayCommand(obj =>
        {
            Application app = Application.Current;
            app.Shutdown();
        });
        /// <summary>
        /// Команда физический добавляет новую иконку ресурса,
        /// представленной в виде геометрической последовательности.
        /// </summary>
        public ICommand AddNewFileIconCommand => new RelayCommand(obj =>
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
                    Name = CommonProperties.IconNames.SingleOrDefault(n => n == name) != null ? name : name + Buttons.Count + 1,
                    BgroundColor = ColorBrush.Content.ToString().FormatStringToSolidColor(),
                    FgroundColor = "#FFFFFF".FormatStringToSolidColor(),
                    Category = obj == null ? "Вся коллекция" : (string)obj,
                    PathList = paths,
                    Scale = 64
                };
                IconsDataWriter.Save(iconModel);
                LoadIcons();
            }
        });
        /// <summary>
        /// Command assign new name for icon.
        /// </summary>
        private ICommand RemaneIcon => new RelayCommand(obj =>
        {
            string newName = _inputBoxViewModel.Text;
            // Get button from view.
            var bt = _inputBoxViewModel.CommandParam as ButtonExtension;
            // Hide inputbox if name was success renamed.
            if (_oldName != null && _inputBoxViewModel.UserAction == Actions.Change)
                _inputBox.Visibility = Visibility.Hidden;
            // Save new name to xmal file.
            IconsDataWriter.SetName(_oldName, newName); 
            // Update icon name without reload icon collection.
            if (bt != null)
            {
                bt.IconName = newName;
                bt.ToolTip = newName;
            }
            // Sort by name and updated the collection.
            Buttons = Buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
        });
        #endregion

        #region Functions
        /// <summary>
        /// Filters collection by collection name.
        /// </summary>
        /// <param name="category">Collection name.</param>
        public void FilterCollection(string category)
        {
            var filtered
                = new ObservableCollection<ButtonExtension>();

            var query = from button in ButtonsClone
                        where button.Category.ToLower().Contains(category.ToLower())
                        select button;

            foreach (var button in query)
                filtered.Add(button);
            Buttons = filtered;

            IconsCollectionModel collectionModel = (IconCategory.Single(o => o.CollectionName == category));
            SelectIndex = IconCategory.IndexOf(collectionModel);
            if (category == "Вся коллекция")
                Buttons = ButtonsClone;
        }
        /// <summary>
        /// Loading icons in the icon editor.
        /// </summary>
        public void LoadIcons()
        {
            using (IconsDataReader icon = new IconsDataReader())
            {
                icon.Dispose();
                Buttons = icon.GetIcons();
                // Initializes button properties and adds in the collection it. 
                foreach (var bt in Buttons)
                {
                    bt.CommandParameter = bt;
                    bt.RemoveIcon = new RelayCommand(obj =>
                    {
                        IconsDataModifier.Remove(bt.Id, bt.Category);
                        Buttons.Remove(obj as ButtonExtension);
                    });
                    bt.RanameIcon = new RelayCommand(obj =>
                    {
                        var bts = obj as ButtonExtension;
                        InitWindowRanameIcon(bts);
                    });
                    bt.Color = "#1A1E24".FormatStringToSolidColor();
                    bt.Template = EnableColourIcon ? Application.Current.TryFindResource("IconTemplateEditorColour") : Application.Current.TryFindResource("IconTemplateEditor");
                    bt.Style = EnableColourIcon ? (Style)Application.Current.TryFindResource("IconStylesEditorColour") : (Style)Application.Current.TryFindResource("IconStylesEditor");
                }
            }
            // Сортирует иконки по алфавиту и упаковывает в коллекцию.
            Buttons = Buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            // Select current collection.
            SelectIndex = 0;
            SelectItem = "";
            // Клонирует коллекцию — для того чтобы восстановить в  
            // исходное состояние коллекцию по необходимости.
            ButtonsClone = Buttons;
            Singleton.Status = true;
        }
        /// <summary>
        /// Field declaration for inputbox.
        /// </summary>
        private static InputBox _inputBox;
        private static InputBoxViewModel _inputBoxViewModel;
        private static string _oldName;
        /// <summary>
        /// Method creates new window of inputbox to rename icon.
        /// </summary>
        /// <param name="bt">Button was selected.</param>
        public void InitWindowRanameIcon(ButtonExtension bt)
        {
            _oldName = bt.IconName;
            // Intializaion inputbox by constructor argument
            _inputBox = new InputBox();
            _inputBoxViewModel = new InputBoxViewModel(_oldName, RemaneIcon, Actions.Change, bt);
            _inputBox.DataContext = _inputBoxViewModel;
            _inputBox.ShowDialog();
        }
        /// <summary>
        /// Adds contextmenu item for the command of adding.
        /// </summary>
        private void AddMenuItem()
        {
            foreach (var category in IconCategory)
            {
                category.NameContextMenu.Items.Insert(0, new MenuItem
                {
                    Header = "Add new icon",
                    Command = AddNewFileIconCommand,
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
    }
}
