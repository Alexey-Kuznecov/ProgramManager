using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using ProgramManager.Contracts;

namespace ProgramManager.Services
{
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }
        public string FileShortName { get; set; }
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                FileShortName = openFileDialog.SafeFileName;
                return true;
            }
            return false;
        }
        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
