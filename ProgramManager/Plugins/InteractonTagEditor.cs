using System.Collections.Generic;
using ProgramManager.ViewModels;

namespace ProgramManager.Plugins
{
    class InteractonTagEditor
    {
        private static List<TagDialogModel> _tagList;
        /// <summary>
        /// Use this property for specify source of a tag list.
        /// </summary>
        public static List<TagDialogModel> TagList
        {
            get { return _tagList; }
            set { _tagList = value; }
        }
        public static string TagSingle { get; set; }
    }
}
