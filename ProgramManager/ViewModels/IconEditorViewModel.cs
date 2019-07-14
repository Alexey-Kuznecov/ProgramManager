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
using ProgramManager.Views.DialogPacks;
using Application = System.Windows.Application;
using Icon = ProgramManager.Resources.Icon;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModel : PropertiesChanged
    {
        private SolidColorBrush _color;
        private IconControl _previewIcon;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private ObservableCollection<WrapPanel> _wrapIcons;
        private ListBoxItem _selectCategory;
        private string _filterText;
        private ObservableCollection<IconCategoryModel> _iconCategory;

        #region Constructors

        public IconEditorViewModel()
        {
            _dialogService = new DefaultDialogService();
            _fileService = new XamlFileService();
            OpenFileIconCommand = new RelayCommand(obj => OpenFileIcon());
            LoadIcons();

            IconCategory = new ObservableCollection<IconCategoryModel>
            {
                new IconCategoryModel { Header = "Программы" },
                new IconCategoryModel { Header = "Логотипы" },
                new IconCategoryModel { Header = "Игры" },
                new IconCategoryModel { Header = "Бренды" },
                new IconCategoryModel { Header = "Разное" }
            };

        }
        #endregion

        #region Properties

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                //Устанавливает цвет иконки.
                if (_color != null)
                    foreach (var item in Buttons)
                        item.Foreground = _color;
            }
        }
        public DrawingBrush IconBrush { get; set; }
        public ObservableCollection<WrapPanel> WrapIcons
        {
            get { return _wrapIcons; }
            set
            {
                _wrapIcons = value;
                OnPropertyChanged("WrapIcons");
            }
        }
        public ObservableCollection<ButtonExtension> Buttons { get; set; }
        public IconControl PreviewIcon
        {
            get { return _previewIcon; }
            set
            {
                _previewIcon = value;
                OnPropertyChanged("WrapIcons");
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
        public ObservableCollection<IconCategoryModel> IconCategory
        {
            get { return _iconCategory; }
            set
            {
                _iconCategory = value;               
                OnPropertyChanged("IconCategory");
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
                    WrapIcons = new ObservableCollection<WrapPanel>();
                    WrapPanel wrap = new WrapPanel();

                    foreach (var bt in Buttons)
                    { bt.RemoveFromParent(); wrap.Children.Add(bt); }
                    WrapIcons.Add(wrap);
                }
                else
                {
                    ObservableCollection<ButtonExtension> filtered = 
                        new ObservableCollection<ButtonExtension>();
                    WrapIcons = new ObservableCollection<WrapPanel>();
                    WrapPanel wrap = new WrapPanel();

                    var query = from button in Buttons
                        where button.IconName.ToLower().Contains(_filterText.ToLower())
                        select button;

                    foreach (var varButton in query)
                    { varButton.RemoveFromParent(); filtered.Add(varButton); }
                    foreach (var bt in filtered)
                        wrap.Children.Add(bt);

                    WrapIcons.Add(wrap);
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
            DrawingBrush brush = bt?.Content as DrawingBrush;
            DrawingGroup group = brush?.Drawing.Clone() as DrawingGroup;
            
            if (@group != null)
            {
                foreach (var item in @group.Children)
                {
                    var geometry = item as GeometryDrawing;
                    if (geometry != null) geometry.Brush = Color;
                }
                IconBrush = new DrawingBrush { Drawing = @group };
            }
            Singleton._status = false;
            Synchronizer.IconLoad.Invoke(new Icon(bt?.Name, IconBrush, "#FFFFFF".FormatStringToSolidColor(), Color));
        });
        /// <summary>
        /// Команда устанавливает иконку
        /// </summary>
        public ICommand Cansel => new RelayCommand(obj =>
        {
            Singleton._status = true;
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
        public ICommand OpenFileIconCommand;

        #endregion

        #region Functions

        /// <summary>
        /// Команда физический добавляет новую иконку ресурса,
        /// представленной в виде геометрической последовательности.
        /// </summary>
        public void OpenFileIcon()
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
                        LoadIcons();
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }
        /// <summary>
        /// Загуржает иконки в редактор иконок.
        /// </summary>
        public void LoadIcons()
        {
            Buttons = new ObservableCollection<ButtonExtension>();
            Collection<ResourceDictionary> collMergedDictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary resourceDictionary = collMergedDictionaries.Single(p => p.Source.ToString().Contains("Icons.xaml"));

            foreach (var key in resourceDictionary.Keys)
            {
                var drawBrush = Application.Current.FindResource(key);
                var brush = drawBrush as DrawingBrush;
                var bt = new ButtonExtension
                {
                    Content = brush,
                    IconName = key.ToString(),
                    Command = SelectIconCommand,
                    Style = (Style)Application.Current.FindResource("IconStyle")
                };
                if (brush != null)
                {
                    bt.CommandParameter = bt;
                    ToolTipService.SetToolTip(bt, key);
                    Buttons.Add(bt);
                    bt.Commander += Show;
                }
            } WrapperIcons();
        }
        /// <summary>
        /// Создает контейнер для иконок, метод нужнен для 
        /// отображения иконок по горизонтали 
        /// </summary>
        public void WrapperIcons()
        {
            WrapIcons = new ObservableCollection<WrapPanel>();
            WrapPanel wrap = new WrapPanel();

            foreach (var bt in Buttons)
                    wrap.Children.Add(bt);
            WrapIcons.Add(wrap);
        }

        #endregion
    }
}
