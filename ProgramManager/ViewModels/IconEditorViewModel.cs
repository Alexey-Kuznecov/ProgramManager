using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ProgramManager.Contracts;
using ProgramManager.Converters;
using ProgramManager.Enums;
using ProgramManager.Models.PackageModel;
using ProgramManager.Resources;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModel : PropertiesChanged
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private ListBoxItem _selectCategory;
        private string _filterText;
        private ObservableCollection<IconCategoryModel> _iconCategory;
        private static ObservableCollection<ButtonExtension> _buttons;

        #region Constructors
        public IconEditorViewModel()
        {
            _dialogService = Singleton.SingleInstance<DefaultDialogService>();
            _fileService = Singleton.SingleInstance<XamlFileService>();
            IconCategory = IconCategoryModel.GetCategory();
            LoadIcons();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Устанавлевает цвет иконок из выбранного цвета в Combobox.
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
            DrawingBrush brush = (DrawingBrush)bt?.Brush;
            DrawingGroup group = brush?.Drawing.Clone() as DrawingGroup;
            var colorBrush = ColorBrush.Content.ToString().FormatStringToSolidColor();

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
            Synchronizer.IconLoad.Invoke(new IconModel(bt?.IconName, IconBrush, "#FFFFFF".FormatStringToSolidColor(), colorBrush));
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
                    List<GeometryDrawing> listGeometry = ConverterXamlResources.ConvertDataToGeometry(ConverterForeignPlugins.XamlExport64(path as Viewbox));

                    // Поиск ресурсов иконок
                    ResourceDictionary resourceDictionary = HelperFunctions.GetResourceDictionary("Icons.xaml");
                    
                    // Упаковывает геометрию иконки в кисть. Добавляет новую кисть в словарь ресурсов.
                    DrawingBrush brush = ConverterXamlResources.ConvertMarkupDrawingBrush(listGeometry);
                    string name = HelperFunctions.ClearExtension(_dialogService.FileShortName);

                    // Добавление иконки в словарь.
                    try { resourceDictionary.Add(name, brush); }
                    catch (Exception)
                    {
                        InputBoxViewModel.UserAction = Actions.Add;
                        RenameIcon(name);
                    }
                    // Сохраняет словарь ресурсов.
                    xamlFile.Save("../../Resources/Icons.xaml", resourceDictionary);
                    
                    // Обнавление списка иконок
                    if (Buttons.Count != resourceDictionary.Count)
                        LoadIcons((string) obj);
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
        /// Загружает иконки в редактор иконок.
        /// </summary>
        public void LoadIcons(string category = null, SolidColorBrush color = null)
        {
            Buttons = new ObservableCollection<ButtonExtension>();
            ButtonsClone = new ObservableCollection<ButtonExtension>();
            Collection<ResourceDictionary> collMergedDictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary resourceDictionary = collMergedDictionaries.Single(p => p.Source.ToString().Contains("Icons.xaml"));

            foreach (var key in resourceDictionary.Keys)
            {
                var drawBrush = Application.Current.FindResource(key);                
                var brush = drawBrush as DrawingBrush;

                #region Инициализация иконок

                // Инициализирует свойства иконоки и добавляет их в коллекцию  
                var bt = new ButtonExtension
                {
                    Brush = brush,
                    IconName = key.ToString(),
                    Category = category,
                    ToolTip = key
                };
                // Если убрать проверку в выборку попадают не только кисти
                // но другие ресурсы, которые есть в словаре.
                if (brush != null)
                {
                    bt.CommandParameter = bt;
                    bt.RemoveIcon = new RelayCommand(RemoveIcon);
                    bt.RenameIcon = new RelayCommand(oName =>
                    {
                        InputBoxViewModel.UserAction = Actions.Change;
                        RenameIcon((string) oName);
                    });
                    Buttons.Add(bt);
                }

                #endregion
            }
            // Сортирует иконки по алфавиту и упаковывает в коллекцию.
            Buttons = Buttons.OrderBy(p => p.IconName.Substring(0, 2)).ToObservableCollection();
            // Клонирует коллекцию — для того чтобы восстановить в  
            // исходное состояние коллекцию по необходимости.
            ButtonsClone = Buttons;
        }
        /// <summary>
        /// Удаляет иконку из редактора иконок.
        /// </summary>
        /// <param name="name">Имя иконки.</param>
        public void RemoveIcon(object name)
        {
            try
            {
                ResourceDictionary resDictionary = HelperFunctions.GetResourceDictionary("Icons.xaml");
                XamlFileService xamlFile = new XamlFileService();
                resDictionary.Remove((string) name);
                xamlFile.Save("../../Resources/Icons.xaml", resDictionary);
                LoadIcons();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage(e.Message);
            }
        }
        /// <summary>
        /// Метод присваивает новое имя для иконки.
        /// </summary>
        /// <param name="oldName">Старое имя иконки.</param>
        public void RenameIcon(string oldName)
        {
            ResourceDictionary resDictionary = HelperFunctions.GetResourceDictionary("Icons.xaml");
            XamlFileService xamlFile = new XamlFileService();
            var dictionary = resDictionary;
            _oldName = oldName;
            // Добавляет имя иконок в исключение, чтобы конвертер знал какие имена уже существуют в словаре ресурсов 
            // и блокировал кнопу действия, дабы избежать проблем с коллизией имен в словаре ресурсов.
            if (ResourceNameValidation.Store != null)
                ResourceNameValidation.Store.Add(oldName);

            #region Тело комманды изменения и добавления иконок

            ICommand action = new RelayCommand(obj =>
            {
                string newName = InputBoxViewModel.Text;

                #region Добавление или изменение иконок

                foreach (var item in dictionary)
                {
                    var entry = (DictionaryEntry)item;
                    var brush = entry.Value as DrawingBrush;
                    var name = (string)entry.Key;
                    if (name == _oldName)
                    {
                        try
                        {
                            dictionary.Add(newName, brush);
                            if (_oldName != null && InputBoxViewModel.UserAction == Actions.Change) dictionary.Remove(_oldName);                          
                            InputBox.Visibility = Visibility.Hidden; // Скрыть окно посли добавления:
                            xamlFile.Save("../../Resources/Icons.xaml", resDictionary); // Сохранить данные в словарь:                          
                            LoadIcons(); // Обновить редактор:
                        }
                        catch (Exception e)
                        {
                            MessageBoxResult choose;
                            choose = MessageBox.Show(e.Message, "Введите другое имя", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                            if (choose == MessageBoxResult.No)
                                InputBox.Visibility = Visibility.Hidden;
                        }
                        break;
                    }
                }
                #endregion

            });
            #endregion

            _cmd = action;
            _userAction = InputBoxViewModel.UserAction;
            InitInputBox();
        }

        #region СОЗДАНИЕ ОКНА ВВОДА ИМЕНИ.

        private static readonly Views.InputBox InputBox = Singleton.SingleInstance<Views.InputBox>();
        private static readonly InputBoxViewModel InputBoxViewModel = Singleton.SingleInstance<InputBoxViewModel>();
        private static ICommand _cmd; // Команда для обработки данных.
        private static string _oldName; // Данные которое нужно корректировать, передаются в окно.
        private static Actions _userAction; // Имя кнопки действие.
        /// <summary>
        /// Инициализирует окно для ввода имени, 
        /// команду можно передать вторым параметром.
        /// </summary>
        private void InitInputBox()
        {
            IconsEditor parent = Singleton.Back as IconsEditor;
            InputBox.DataContext = InputBoxViewModel;
            InputBox.Owner = parent;
            InputBoxViewModel.Text = _oldName;
            InputBoxViewModel.Action = _cmd;
            InputBoxViewModel.UserAction = _userAction;
            InputBox.Visibility = Visibility.Visible;
        }
        #endregion

        #endregion
    }
}
