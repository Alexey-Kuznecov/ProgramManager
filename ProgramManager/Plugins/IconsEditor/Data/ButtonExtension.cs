using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgramManager.Plugins.IconsEditor.Data
{
    [Serializable]
    class ButtonExtension
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string IconName { get; set; }
        public DrawingBrush Brush { get; set; }
        public ButtonExtension CommandParameter { get; internal set; }
        public object ToolTip { get; set; }
        public SolidColorBrush Color { get; set; }
        public Style Style { get; set; }
        public object Template { get; set; }
        /// <summary>
        /// Комманда для удаления икнок из редактора.
        /// </summary>       
        public ICommand RemoveIcon { get; set; }
        public ICommand RanameIcon { get; set; }
        public Path Path { get; internal set; }
    }
}
