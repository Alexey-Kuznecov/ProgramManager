using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Contracts;
using ProgramManager.Resources;
using ProgramManager.Services;
using ProgramManager.ViewModels.Base;
using ProgramManager.Views.DialogPacks;
using Icon = ProgramManager.Resources.Icon;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModel : PropertiesChanged
    {
        private SolidColorBrush _color;
        private IconControl _previewIcon;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;

        #region Constructors

        public IconEditorViewModel()
        {
            this._dialogService = new DefaultDialogService();
            this._fileService = new XamlFileService();
            LoadIcons();
        }

        #endregion

        #region Properties

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                // Устанавливает цвет иконки.
                if (_color != null)
                    foreach (var item in Buttons)
                        item.Foreground = _color;
            }
        }
        public DrawingBrush IconBrush { get; set; }
        public List<WrapPanel> WrapIcons { get; set; }
        public List<Button> Buttons { get; set; }
        public IconControl PreviewIcon
        {
            get { return _previewIcon; }
            set
            {
                _previewIcon = value;
                SetProperty(ref _previewIcon, value, () => PreviewIcon);
            }
        }

        #endregion

        #region Commands
        /// <summary>
        /// Команда для иконки устанавлевает цвет иконки
        /// </summary>
        public ICommand SelectIconCommand => new RelayCommand(obj =>
        {  
            Button bt = obj as Button;
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
            PreviewIcon = new IconControl();
            PreviewIcon.DataContext = new IconViewModel();
            Messenger.Default.Send(new Icon(bt.Name, "#FFFFFF", "#3AE2CE"));
        });
        /// <summary>
        /// Команда устанавливает иконку
        /// </summary>
        public ICommand LoadIconCommand => new RelayCommand(obj =>
        {

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
                    resourceDictionary.Add(HelperFunctions.ClearExtension(_dialogService.FilePath),
                        ConverterXamlResources.ConvertMarkupDrawingBrush(listGeometry));

                    // Сохраняет словарь ресурсов.
                    xamlFile.Save("../../Resources/Icons.xaml", resourceDictionary);
                    _dialogService.ShowMessage("Файл открыт");
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
        /// Загуржает иконки в редактор иконок.
        /// </summary>
        public void LoadIcons()
        {
            Buttons = new List<Button>();
            Collection<ResourceDictionary> collMergedDictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary resourceDictionary = collMergedDictionaries.Single(p => p.Source.ToString().Contains("Icons.xaml"));

            foreach (var key in resourceDictionary.Keys)
            {
                var drawBrush = Application.Current.FindResource(key);
                var brush = drawBrush as DrawingBrush;

                Button bt = new Button { Content = brush, Name = key.ToString() };
                if (brush != null)
                {
                    ToolTipService.SetToolTip(bt, key);
                    Buttons.Add(bt);
                }
            } WrapperIcons();
        }
        /// <summary>
        /// Создает контейнер для иконок, метод нужнен для 
        /// отображения иконок по горизонтали 
        /// </summary>
        public void WrapperIcons()
        {
            WrapPanel wrap = new WrapPanel();
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Command = SelectIconCommand;
                Buttons[i].CommandParameter = Buttons[i];
                Buttons[i].Style = (Style)Application.Current.FindResource("IconStyle");
                wrap.Children.Add(Buttons[i]);
            }
            WrapIcons = new List<WrapPanel> { wrap };
        }
        
        #endregion
    }
}
