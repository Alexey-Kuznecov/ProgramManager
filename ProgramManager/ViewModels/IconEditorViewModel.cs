using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight.Messaging;
using ProgramManager.Resources;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModel : PropertiesChanged
    {
        private object _color;
        private ComboBoxItem _comboBox;
        private string _nameResource;

        #region Constructors

        public IconEditorViewModel()
        {
            Buttons = new List<Button>();
            ResourceDictionary dictionary = Application.Current.Resources;

            foreach (var dict in dictionary.MergedDictionaries)
            {
                if (dict.Source.OriginalString.Contains("Icons.xaml"))
                {
                    foreach (var key in dict.Keys)
                    {
                        var drawBrush = Application.Current.FindResource(key);
                        var brush = drawBrush as DrawingBrush;

                        if (brush != null)
                        {
                            Button bt = new Button { Content = brush, Name = key.ToString() };
                            ToolTipService.SetToolTip(bt, key);
                            Buttons.Add(bt);
                        }
                    }
                    break;
                }
            }
            WrapperIcons();
        }

        #endregion

        #region Properties

        public object Color
        {
            get { return _color; }
            set
            { 
                _color = value;
                SetProperty(ref _color, value, () => IconBrush);

                _comboBox = value as ComboBoxItem;
                if (_color != null)
                    SetColor(_comboBox?.Content.ToString());
            }
        }
        public DrawingBrush IconBrush { get; set; }
        public List<WrapPanel> WrapIcons { get; set; }
        public List<Button> Buttons { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// Команда для иконки устанавлевает цвет иконки
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public ICommand SelectIcon => new RelayCommand(obj =>
        {
            Button bt = obj as Button;
            DrawingBrush brush = bt.Content as DrawingBrush;
            DrawingGroup group = brush.Drawing.Clone() as DrawingGroup;
            IconBrush = new DrawingBrush();
            Color hex = (Color) ColorConverter.ConvertFromString(_comboBox.Content.ToString());
            Color backColor = (Color)ColorConverter.ConvertFromString("#FFFFFF");

            foreach (var item in group.Children)
            {
                GeometryDrawing geometry = item as GeometryDrawing;
                geometry.Brush = new SolidColorBrush(hex);
            }
            // Данные выбранной иконки готовые для отправки
            _nameResource = bt.Name;
            IconBrush.Drawing = group;
            Messenger.Default.Send(new Icon { Brush = IconBrush, BgroundColor = new SolidColorBrush(hex), FgroundColor = new SolidColorBrush(backColor) });
        });
        /// <summary>
        /// Команда для кнопки устанавлевает иконку
        /// </summary>
        public ICommand SetIcon => new RelayCommand(obj => { });

        #endregion

        #region Functions

        /// <summary>
        /// Устанавливает цвет иконки.
        /// </summary>
        /// <param name="color">Цвет выбранный пользователем.</param>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void SetColor(string color)
        {
            if (color != null)
            {
                Color hex = (Color)ColorConverter.ConvertFromString(color);
                foreach (var item in Buttons)
                    item.Foreground = new SolidColorBrush(hex);
            }
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
                Buttons[i].Command = SelectIcon;
                Buttons[i].CommandParameter = Buttons[i];
                Buttons[i].Style = (Style)Application.Current.FindResource("IconStyle");
                wrap.Children.Add(Buttons[i]);
            }
            WrapIcons = new List<WrapPanel> { wrap };
        }

        #endregion
    }
}
