using System;
using ProgramManager.Models;
using ProgramManager.ViewModels;

namespace ProgramManager.Services
{
    /// <summary>
    /// EventAggregate is  
    /// </summary>
    public class EventAggregate
    {
        #region Icon Editor Event

        public static event EventHandler<BaseEventArgs> UpdateCollectionNames;
        /// <summary>
        /// The event raise when creating objects Icon.
        /// </summary>
        /// <param name="collectionNames"></param>
        public void OnUpdateCollectionNames(object collectionNames)
        {
            UpdateCollectionNames?.Invoke(this, new BaseEventArgs(collectionNames));
        }
        #endregion

        #region Package Event

        public static event EventHandler<BaseEventArgs> PackageChanged;
        public static event EventHandler<BaseEventArgs> NewPackage;
        public static event EventHandler<BaseEventArgs> LoadTagList;
        public static event EventHandler<BaseEventArgs> RemovePackage;
        public static event EventHandler<BaseEventArgs> CategoryChanged;
        public static event EventHandler<BaseEventArgs> ImageLaod;
        public static event Action<string> LoadPackage;
        /// <summary>
        /// The event raise when the package was changed.
        /// </summary>
        /// <param name="package">Package that was changed.</param>
        public void OnPackageChanged(object package)
        {
            PackageChanged?.Invoke(this, new BaseEventArgs(package));
        }
        /// <summary>
        /// The event raise when the package was added.
        /// </summary>
        /// <param name="package">Package that was added.</param>
        public void OnNewPackage(object package)
        {
            NewPackage?.Invoke(this, new BaseEventArgs(package));
        }
        /// <summary>
        /// The event raise when tags load in edit tags window.
        /// </summary>
        /// <param name="package">Package that conains tags.</param>
        public void OnLoadTagsList(object package)
        {
            LoadTagList?.Invoke(this, new BaseEventArgs(package));
        }
        /// <summary>
        /// The event raise when user delete package of list package.
        /// </summary>
        /// <param name="package">Package that to be delete.</param>
        public void OnRemovePackage(object package)
        {
            RemovePackage?.Invoke(this, new BaseEventArgs(package));
        }
        /// <summary>
        /// The event raise when the package was loaded.
        /// </summary>
        /// <param name="message">Can notify the user of any message on load package.</param>
        public void OnLoadPackage(string message)
        {
            LoadPackage?.Invoke(message);
        }
        /// <summary>
        /// The event raise when the user selected category in combobox.
        /// </summary>
        /// <param name="category">Сategory that was selected.</param>
        public void OnCategoryChanged(object category)
        {
            CategoryChanged?.Invoke(this, new BaseEventArgs(category));
        }
        /// <summary>
        /// The event raise when the user selected images in package dialog.
        /// </summary>
        /// <param name="imageCover">Image object type ImageCover.</param>
        public void OnImageLoad(object imageCover)
        {
            ImageLaod?.Invoke(this, new BaseEventArgs(imageCover));
        }
        #endregion
    }
}
