using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ProgramManager.Contracts;
using ProgramManager.Models.PackageModel;
using ProgramManager.Resources;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using Application = System.Windows.Application;
using Icon = ProgramManager.Resources.Icon;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModel : PropertiesChanged
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private ListBoxItem _selectCategory;
        private string _filterText;
        private ObservableCollection<IconCategoryModel> _iconCategory;
        private ObservableCollection<ButtonExtension> _buttons;

        #region Constructors

        public IconEditorViewModel()
        {
            _dialogService = new DefaultDialogService();
            _fileService = new XamlFileService();
            IconCategory = IconCategoryModel.GetCategory();
            AddMenuItem();
            LoadIcons();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Устанавлевает цвет иконок из выбранного цвета в Combobox.
        /// </summary>
        public ComboBoxItem Color { get; set; }
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
        public ObservableCollection<ButtonExtension> ButtonsStore { get; set; }
        public ObservableCollection<IconCategoryModel> IconCategory
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
                SetProperty(ref _filterText, value, () => FilterText);

                if (string.IsNullOrEmpty(_filterText))
                {
                    Buttons = ButtonsStore;
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
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Команда для иконки устанавлевает цвет иконки
        /// </summary>
        public ICommand SelectIconCommand => new RelayCommand(obj =>
        {
            ButtonExtension bt = obj as ButtonExtension;
            DrawingBrush brush = (DrawingBrush)bt?.Brush;
            DrawingGroup group = brush?.Drawing.Clone() as DrawingGroup;
            var colorBrush = Color.Content.ToString().FormatStringToSolidColor();

            if (@group != null)
            {
                foreach (var item in @group.Children)
                {
                    var geometry = item as GeometryDrawing;
                    if (geometry != null) geometry.Brush = colorBrush;
                }
                IconBrush = new DrawingBrush { Drawing = @group };
            }

            Singleton.Status = false;
            Synchronizer.IconLoad.Invoke(new Icon(bt?.IconName, IconBrush, "#FFFFFF".FormatStringToSolidColor(), colorBrush));
        });
        /// <summary>
        /// Команда устанавливает иконку
        /// </summary>
        public ICommand Cansel => new RelayCommand(obj =>
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
            try
            {
                if (_dialogService.OpenFileDialog())
                {
                    var path = _fileService.Open(_dialogService.FilePath);
                    // Загружает файл ресурса икоки, преобразует в форму GeometryDrawing. 
                    XamlFileService xamlFile = new XamlFileService();
                    List<GeometryDrawing> listGeometry =
                        ConverterXamlResources.ConvertDataToGeometry(ConverterForeignPlugins.XamlExport64(path as Viewbox));

                    // Упаковывает геометрию иконки в кисть. Добавляет новую кисть в словарь ресурсов.
                    Collection<ResourceDictionary> collMergedDictionaries = Application.Current.Resources.MergedDictionaries;
                    ResourceDictionary resourceDictionary = collMergedDictionaries.Single(p => p.Source.ToString().Contains("Icons.xaml"));
                    var brush = ConverterXamlResources.ConvertMarkupDrawingBrush(listGeometry);
                    string name = HelperFunctions.ClearExtension(_dialogService.FileShortName);
                    resourceDictionary.Add(name, brush);

                    // Сохраняет словарь ресурсов.
                    xamlFile.Save("../../Resources/Icons.xaml", resourceDictionary);

                    if (Buttons.Count != resourceDictionary.Count - 1)
                        LoadIcons((string)obj);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        });

        #endregion

        #region Functions
        /// <summary>
        /// Добавляет элемент в контекстное меню категорий.
        /// </summary>
        public void AddMenuItem()
        {
            foreach (var category in IconCategory)
            {
                category.ContextCatMenu.Items.Add(new MenuItem
                {
                    Header = "Добавить иконку",
                    Command = OpenFileIconCommand,
                    CommandParameter = category.Header
                });
            }
        }
        /// <summary>
        /// Загуржает иконки в редактор иконок.
        /// </summary>
        public void LoadIcons(string category = null, SolidColorBrush color = null)
        {
            Buttons = new ObservableCollection<ButtonExtension>();
            ButtonsStore = new ObservableCollection<ButtonExtension>();
            Collection<ResourceDictionary> collMergedDictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary resourceDictionary = collMergedDictionaries.Single(p => p.Source.ToString().Contains("Icons.xaml"));

            foreach (var key in resourceDictionary.Keys)
            {
                var drawBrush = Application.Current.FindResource(key);
                var brush = drawBrush as DrawingBrush;
                var bt = new ButtonExtension
                {
                    Brush = brush,
                    IconName = key.ToString(),
                    Category = category,
                    ToolTip = key
                };
                if (brush != null)
                {
                    bt.CommandParameter = bt;
                    Buttons.Add(bt);
                    ButtonsStore.Add(bt);
                }
            }
        }

        #endregion
    }
}
