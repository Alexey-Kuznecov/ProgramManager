using System;
using System.Collections.Generic;

namespace InteractionLib.TagEditor
{
    public class Interaction
    {
        /// <summary>
        /// Use this property for specify source of a tag list.
        /// </summary>
        public static List<TagDialogModel> TagList { get; set; }
        public static string TagSingle { get; set; }
        public static List<string> TagListString { get; set; }
    }
}
