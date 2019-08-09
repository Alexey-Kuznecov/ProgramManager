using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    public class DragDrop
    {
        private static Brush _previousFill = null;
        public static MouseEventHandler MouseMoveHandler { get; set; } = OnMouseMove;
        public static EventHandler Initializing { get; set; } = OnInitializing;
        public static DragEventHandler DropHandler { get; set; } = OnDrop;
        public static DragEventHandler DragOverHandler { get; set; } = OnDragOver;
        public static DragEventHandler DragEnterHandler { get; set; } = OnDragEnter;
        public static GiveFeedbackEventHandler GiveFeedbackHandler { get; set; } = OnGiveFeedback;

        public static void OnInitializing(object sender, EventArgs e)
        {

        }
        public static void OnMouseMove(object sender, MouseEventArgs e)
        {
           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var grid = sender as Grid;
                if (grid != null)
                {
                    // Package the data.
                    DataObject data = new DataObject();
                    data.SetData(DataFormats.StringFormat, grid.Background.ToString());
                    data.SetData("Double", grid.Height);
                    data.SetData("Object", new DragDrop());
                    // Inititate the drag-and-drop operation.
                    System.Windows.DragDrop.DoDragDrop(grid, data: data,
                        allowedEffects: DragDropEffects.Copy | DragDropEffects.Move);
                }
                else
                {
                    var button = sender as Button;
                    if (button != null)
                    {
                        // Package the data.
                        DataObject data = new DataObject();
                        data.SetData(button);
                        data.SetData("Double", button.Height);
                        data.SetData("Object", new DragDrop());
                        // Inititate the drag-and-drop operation.
                        System.Windows.DragDrop.DoDragDrop(button, data: data,
                            allowedEffects: DragDropEffects.Copy | DragDropEffects.Move);
                    }
                }
            }
        }
        public static void OnDrop(object sender, DragEventArgs e)
        {
            // If the DataObject contains string data, extract it.
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

                // If the string can be converted into a Brush, 
                // convert it and apply it to the ellipse.
                BrushConverter converter = new BrushConverter();
                if (converter.IsValid(dataString))
                {
                    Brush newFill = (Brush)converter.ConvertFromString(dataString);
                    //circleUI.Fill = newFill;

                    // Set Effects to notify the drag source what effect
                    // the drag-and-drop operation had.
                    // (Copy if CTRL is pressed; otherwise, move.)
                    if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
            }
            e.Handled = true;
        }
        public static void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            // If the DataObject contains string data, extract it.
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

                // If the string can be converted into a Brush, allow copying or moving.
                BrushConverter converter = new BrushConverter();
                if (converter.IsValid(dataString))
                {
                    // Set Effects to notify the drag source what effect
                    // the drag-and-drop operation will have. These values are 
                    // used by the drag source's GiveFeedback event handler.
                    // (Copy if CTRL is pressed; otherwise, move.)
                    if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
            }
            e.Handled = true;
        }
        public static void OnDragEnter(object sender, DragEventArgs e)
        {
            // Save the current Fill brush so that you can revert back to this value in DragLeave.
            if (sender is Grid)
                _previousFill = (sender as Grid).Background;
            else if (sender is Button)
                _previousFill = (sender as Button).Foreground;

            // If the DataObject contains string data, extract it.
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

                // If the string can be converted into a Brush, convert it.
                BrushConverter converter = new BrushConverter();
                if (converter.IsValid(dataString))
                {
                    Brush newFill = (Brush)converter.ConvertFromString(dataString);

                    if (sender is Grid)
                        ((Grid) sender).Background = newFill;
                    else if (sender is Button)
                        ((Button)sender).Background = newFill;
                }
            }
        }
        public static void OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }
    }
}
