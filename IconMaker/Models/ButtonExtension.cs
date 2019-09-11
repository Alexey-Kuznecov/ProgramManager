using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IconMaker.Models
{
    [Serializable]
    class ButtonExtension
    {
        public ushort Id { get; set; }
        public DrawingBrush Brush { get; set; }
        public ButtonExtension CommandParameter { get; internal set; }
        public SolidColorBrush Color { get; set; }
        public Style Style { get; set; }
        public Path Path { get; internal set; }
        public string CollectionName { get; set; }
        public string IconName { get; set; }
        public object ToolTip { get; set; }
        public object Template { get; set; }
        /// <summary>
        /// Комманда для удаления икнок из редактора.
        /// </summary>       
        public ICommand RemoveIcon { get; set; }
        public ICommand RenameIcon { get; set; }
        public ICommand ReplaceIcon { get; set; }
    }
}
