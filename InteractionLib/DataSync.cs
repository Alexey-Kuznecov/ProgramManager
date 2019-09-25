using System.Collections.Generic;
using AlexLibWpf.Models;
using AlexLibWpf.Mvvm.Base;

namespace InteractionLib
{
    public class DataSync : PropertiesChanged
    {
        /// <summary>
        /// Transfers icon data:
        /// from <source cref="IconsEditorViewModel.SelectIconCommand"/> 
        /// in <target cref="PackagesDialogViewModel.LoadSelectIcon"/>
        /// </summary>
        /// <param name="obj">Icon data as <model cref="IconModel"/></param>
        public delegate void CancelChangeIcon(IconModel obj);
        public static CancelChangeIcon IconLoad;
        /// <summary>
        /// Send tags selected in the package dialog.
        /// TagLoad: Contains reference on method from package dialog, 
        /// which assign tags list selected to TagList property of PackageBase object.
        /// </summary>
        /// <param name="obj"></param>
        public delegate void TagLoader(List<string> obj);
        public static TagLoader TagLoad;
        /// <summary>
        /// Calls the method that is communicated with the delegate,
        /// when load package in the package dialog to edit.
        /// </summary>
        /// <param name="package">Contains package to edit.</param>
        public delegate void PackageLoader(object package);
        public static PackageLoader PackageLoad;
        /// <summary>
        /// Calls the method that is communicated with the delegate,
        /// when load image cover in the package dialog.
        /// </summary>
        /// <param name="package">Contains package to edit.</param>
        //public delegate void ImageLoader(ImageCover package);
        //public static ImageLoader ImageLoad;
    }
}
