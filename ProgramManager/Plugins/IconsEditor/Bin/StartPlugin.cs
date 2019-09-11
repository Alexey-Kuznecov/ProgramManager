using System;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Threading;
using ProgramManager.Services;
using ProgramManager.ViewModels;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    class StartPlugin
    {
        public static void RunPlugin(object src)
        {
            try
            {
                IconsEditor singleInstense = Singleton.GetSingleInstance<IconsEditor>() ?? Singleton.SingleInstance<IconsEditor>();
                singleInstense.ShowDialog();
            }
            catch (InvalidCastException e)
            {
                MessageBox.Show(e.Message, "Файл не был загружен.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
