using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgramManager.Resources
{
    [Serializable]
    class ButtonExtension
    {
        public string Category { get; set; }
        public string IconName { get; set; }
        public DrawingBrush Brush { get; set; }
        public ButtonExtension CommandParameter { get; internal set; }
        public object ToolTip { get; set; }
    }
}
