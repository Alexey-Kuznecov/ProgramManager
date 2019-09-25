
namespace ProgramManager.Services
{
    using System;

    /// <summary>
    /// The main events that occur in the program.
    /// </summary>
    public class EventAggregate
    {
        /// <summary>
        /// The package changed.
        /// </summary>
        public static event EventHandler<BaseEventArgs> PackageChanged;

        /// <summary>
        /// The new package.
        /// </summary>
        public static event EventHandler<BaseEventArgs> NewPackage;

        /// <summary>
        /// The load tag list.
        /// </summary>
        public static event EventHandler<BaseEventArgs> LoadTagList;

        /// <summary>
        /// The remove package.
        /// </summary>
        public static event EventHandler<BaseEventArgs> RemovePackage;

        /// <summary>
        /// The category changed.
        /// </summary>
        public static event EventHandler<BaseEventArgs> CategoryChanged;

        /// <summary>
        /// The image load.
        /// </summary>
        public static event EventHandler<BaseEventArgs> ImageLoad;

        /// <summary>
        /// The load package.
        /// </summary>
        public static event Action<string> LoadPackage;

        /// <summary>
        /// The event raise when the Package was changed.
        /// </summary>
        /// <param name="package">Package that was changed.</param>
        public void OnPackageChanged(object package)
        {
            PackageChanged?.Invoke(this, new BaseEventArgs(package));
        }

        /// <summary>
        /// The event raise when the Package was added.
        /// </summary>
        /// <param name="package">Package that was added.</param>
        public void OnNewPackage(object package)
        {
            NewPackage?.Invoke(this, new BaseEventArgs(package));
        }

        /// <summary>
        /// The event raise when tags load in edit tags window.
        /// </summary>
        /// <param name="package">Package that contains tags.</param>
        public void OnLoadTagsList(object package)
        {
            LoadTagList?.Invoke(this, new BaseEventArgs(package));
        }

        /// <summary>
        /// The event raise when user delete Package of list Package.
        /// </summary>
        /// <param name="package">Package that to be delete.</param>
        public void OnRemovePackage(object package)
        {
            RemovePackage?.Invoke(this, new BaseEventArgs(package));
        }

        /// <summary>
        /// The event raise when the package was loaded.
        /// </summary>
        /// <param name="message">Can notify the user of any Message on load package.</param>
        public void OnLoadPackage(string message)
        {
            LoadPackage?.Invoke(message);
        }

        /// <summary>
        /// The event raise when the user selected Category in combo box.
        /// </summary>
        /// <param name="category">Category that was selected.</param>
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
            ImageLoad?.Invoke(this, new BaseEventArgs(imageCover));
        }
    }
}
