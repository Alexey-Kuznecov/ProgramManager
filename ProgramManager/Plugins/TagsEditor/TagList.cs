using System.Windows;
using ProgramManager.Services;
using ProgramManager.Views.DialogPacks;

namespace ProgramManager.Plugins.TagsEditor
{
    class TagList
    {
        public static void SetTags(object src)
        {
            TagDialog dialog = new TagDialog();
            dialog.ShowDialog();
        }
        public static void LoadTags(object s)
        {
            EventAggregate.LoadTagList += new TagDialogViewModel().DisplayLoadTagList;
        }
    }
}
