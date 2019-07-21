using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgramManager.Resources
{
    [Serializable]
    class ButtonExtension
    {
        public static int Id { get; set; }
        public string Category { get; set; }
        public string IconName { get; set; }
        public DrawingBrush Brush { get; set; }
        public ButtonExtension CommandParameter { get; internal set; }
        public object ToolTip { get; set; }
        /// <summary>
        /// Комманда для удаления икнок из редактора.
        /// </summary>       
        public ICommand RemoveIcon { get; set; }
        public ICommand RenameIcon { get; set; }
        public Path Path { get; internal set; }
    }
}
