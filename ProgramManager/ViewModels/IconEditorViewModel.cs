using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;

namespace ProgramManager.ViewModels
{
    class IconEditorViewModel : PropertiesChanged
    {
        public DrawingBrush IconBrush { get; set; }
        public Brush Color { get; set; }
        public IconEditorViewModel IconReady { get; set; }

        public ICommand SelectIcon => new RelayCommand(obj =>
        {
            var button = obj as Button;
            IconEditorViewModel IconReady = new IconEditorViewModel
            {
                IconBrush = button?.Content as DrawingBrush,
                Color = button?.Foreground
            };
        });
        public ICommand SetIcon => new RelayCommand(obj => { Messenger.Default.Send(IconReady); });
     }
}
