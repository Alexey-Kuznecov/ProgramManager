using ProgramManager.Models;
using ProgramManager.Plugins.IconsEditor.Bin;
using ProgramManager.Resources;
using ProgramManager.ViewModels;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.Services
{
    class Synchronizer : PropertiesChanged
    {
        private SearchIcon _iconSearch;
        public SearchIcon IconSearch
        {
            get { return _iconSearch; }
            set
            {
                _iconSearch = value;
                OnPropertyChanged("IconSearch");
            }
        }
        /// <summary>
        /// Contains reference to the Method
        /// <ref cref="PackagesDialogViewModel.LoadSelectIcon"/>
        /// </summary>
        public static CancelChangeIcon IconLoad;
        /// <summary>
        /// Transfers icon data:
        /// from <source cref="IconEditorViewModel.SelectIconCommand"/> 
        /// in <target cref="PackagesDialogViewModel.LoadSelectIcon"/>
        /// </summary>
        /// <param name="obj">Icon data as <model cref="IconModel"/></param>
        public delegate void CancelChangeIcon(IconModel obj);
        public delegate void SearchIcon(string name);
    }
}
