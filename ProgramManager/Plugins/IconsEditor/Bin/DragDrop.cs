using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    [DebuggerStepThrough]
    public class DragDrop
    {
        public static MouseEventHandler MouseMoveHandler { get; set; } = OnMouseMove;
        public static EventHandler Initializing { get; set; } = OnInitializing;
        public static DragEventHandler DropHandler { get; set; } = OnDrop;
        public static DragEventHandler DragOverHandler { get; set; } = OnDragOver;
        public static DragEventHandler DragLeaveHandler { get; set; } = OnDragLeave;
        public static DragEventHandler DragEnterHandler { get; set; } = OnDragEnter;
        public static GiveFeedbackEventHandler GiveFeedbackHandler { get; set; } = OnGiveFeedback;

        public static void OnInitializing(object sender, EventArgs e)
        {

        }
        public static void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var control = sender as ContentControl;

                if (control != null)
                {
                    // Package the data.
                    DataObject data = new DataObject();
                    data.SetData(data: control);
                    data.SetData("Object", new DragDrop());
                    // Inititate the drag-and-drop operation.
                    System.Windows.DragDrop.DoDragDrop(control, data: data,
                        allowedEffects: DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }
        public static void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ContentControl)) is ContentControl)
            {
                var textBlock = sender as TextBlock;
                if (textBlock != null)
                    textBlock.Style = null;

                IconCollectionBase.DragDrop.Invoke(sender, e);
                // Set Effects to notify the drag source what effect
                // the drag-and-drop operation had.
                // (Copy if CTRL is pressed; otherwise, move.)
                if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    e.Effects = DragDropEffects.Copy;
                else
                    e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }
        public static void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            var textBlock = sender as TextBlock;
            if (textBlock != null)
                textBlock.Style = (Style)Application.Current.TryFindResource("CollectionMenuDragDropStyle");

            // Set Effects to notify the drag source what effect
            // the drag-and-drop operation will have. These values are 
            // used by the drag source's GiveFeedback event handler.
            // (Copy if CTRL is pressed; otherwise, move.)
            if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
        public static void OnDragEnter(object sender, DragEventArgs e)
        {
            var button = e.Data.GetData(typeof(Button));

            if (button != null)
            {
                var dds = sender as TextBox;
                var collection = dds?.Text;
            }
        }
        public static void OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Copy))
                Mouse.SetCursor(Cursors.Cross);
            else if (e.Effects.HasFlag(DragDropEffects.Move))
                Mouse.SetCursor(Cursors.UpArrow);
            else
                Mouse.SetCursor(Cursors.No);
            e.Handled = true;
        }
        public static void OnDragLeave(object sender, DragEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                textBlock.Style = null;
            }
        }
    }
}
