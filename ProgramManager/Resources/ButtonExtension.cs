using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Resources
{
    class ButtonExtension
    {
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
    }
}
