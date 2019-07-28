using ProgramManager.Models;
using ProgramManager.Plugins.IconsEditor.Bin;
using ProgramManager.ViewModels;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Plugins
{
    class DataSync : PropertiesChanged
    {
        /// <summary>
        /// Contains reference to the Method
        /// <ref cref="PackagesDialogViewModel.LoadSelectIcon"/>
        /// </summary>
        public static CancelChangeIcon IconLoad;
        /// <summary>
        /// Transfers icon data:
        /// from <source cref="IconsEditorViewModel.SelectIconCommand"/> 
        /// in <target cref="PackagesDialogViewModel.LoadSelectIcon"/>
        /// </summary>
        /// <param name="obj">Icon data as <model cref="IconModel"/></param>
        public delegate void CancelChangeIcon(IconModel obj);
    }
}
