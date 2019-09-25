using System.Windows;
using InteractionLib.Events;

namespace TagEditor
{
    class TagEditor
    {
        public static void Start(object src)
        {
            TagDialog dialog = new TagDialog();
            dialog.ShowDialog();
        }
        public static void LoadTags(object s)
        {
          /// EventAggregate.LoadTagList += new TagDialogViewModel().DisplayLoadTagList;
        }
    }
}
