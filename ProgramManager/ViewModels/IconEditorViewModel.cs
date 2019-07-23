using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Linq;
using ProgramManager.Contracts;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Resources;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    /// <summary>
    /// View model for window IconsEditor. 
    /// </summary>
    class IconEditorViewModel : PropertiesChanged , IDisposable
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private ListBoxItem _selectCategory;
        private string _filterText;
        private ObservableCollection<IconCollectionModel> _iconCategory;
        private SolidColorBrush _color;
        private static ObservableCollection<ButtonExtension> _buttons;
        
        #region Constructors
        public IconEditorViewModel()
        {
            _dialogService = Singleton.SingleInstance<DefaultDialogService>();
            _fileService = Singleton.SingleInstance<XamlFileService>();
            IconCategory = IconCollectionModel.GetCategory();
            
            // Решает проблему с многократным вызывом
            if (!Singleton.Status)
            {
                _inputBox = Singleton.GetSingleInstance<InputBox>();
                AddMenuItem();
                LoadIcons();
            }
        }
        #endregion

        #region Properties
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
        public ObservableCollection<IconCollectionModel> IconCategory
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

                    var query = from button in Buttons
                        where button.IconName.ToLower().Contains(_filterText.ToLower())
                        select button;

                    foreach (var button in query)
                        filtered.Add(button);
                    Buttons = filtered;
                }
                OnPropertyChanged("FilterText");
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
            // Sets flag to fasle to restore button source state.If user remove been set icon.
            Singleton.Status = false;
            Synchronizer.IconLoad.Invoke(new IconModel()
            {
                Name = bt?.IconName,
                Path = bt?.Path,
                FgroundColor = "#FFFFFF".FormatStringToSolidColor(),
                BgroundColor = ColorBrush.Content.ToString().FormatStringToSolidColor()
            });
        });
        /// <summary>
        /// Команда устанавливает иконку по умолчанию.
        /// </summary>
        public ICommand ResetByDefault => new RelayCommand(obj =>
        {
            Singleton.Status = true;
            Synchronizer.IconLoad.Invoke(null);
        });
        /// <summary>
        /// Команда устанавливает иконку
        /// </summary>
        public ICommand Shutdown => new RelayCommand(obj =>
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();
        });
        /// <summary>
        /// Команда физический добавляет новую иконку ресурса,
        /// представленной в виде геометрической последовательности.
        /// </summary>
        public ICommand OpenFileIconCommand => new RelayCommand(obj =>
        {
            if (_dialogService.OpenFileDialog())
            {
                var path = _fileService.Open(_dialogService.FilePath);
                //  filename and extract geometry path of xaml file and .
                string name = HelperFunctions.ClearExtension(_dialogService.FileShortName);
                string paths = ConverterForeignPlugins.XamlExport64Path(path as Viewbox);

                if (ResourceNameValidation.StoreName != null)
                    CommonProperties.IconNames.Add(name);
                // Icon data packing to saving.
                IconModel iconModel = new IconModel()
                {
                    Id = Buttons.Count + 1,
                    Name = CommonProperties.IconNames.SingleOrDefault(n => n == name) != null ? "new_" + name : "new_" + name + Buttons.Count + 1,
                    BgroundColor = ColorBrush.Content.ToString().FormatStringToSolidColor(),
                    FgroundColor = "#FFFFFF".FormatStringToSolidColor(),
                    Category = "Разное",
                    StringPath = paths,
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
                }
            }
            // Сортирует иконки по алфавиту и упаковывает в коллекцию.
            Buttons = Buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
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
            _inputBoxViewModel = new InputBoxViewModel(_oldName, RemaneIcon, Actions.Change, bt);
            _inputBox.DataContext = _inputBoxViewModel;
            _inputBox.Visibility = Visibility.Visible;
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
                    Command = OpenFileIconCommand,
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
