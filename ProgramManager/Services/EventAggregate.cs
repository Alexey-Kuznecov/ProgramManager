using System;

namespace ProgramManager.Services
{
    public class EventAggregate
    {
        public static event EventHandler<BaseEventArgs> PackageChanged;
        public static event EventHandler<BaseEventArgs> NewPackage;
        public static event EventHandler<BaseEventArgs> TagListUpdate;
        public static event EventHandler<BaseEventArgs> RemovePackage;
        public static event EventHandler<BaseEventArgs> CategoryChanged;
        public static event Action<string> LoadPackage;

        public void OnPackageChanged(object package)
        {
            PackageChanged?.Invoke(this, new BaseEventArgs(package));
        }
        public void OnNewPackage(object package)
        {
            NewPackage?.Invoke(this, new BaseEventArgs(package));
        }
        public void OnLoadTagsList(object package)
        {
            TagListUpdate?.Invoke(this, new BaseEventArgs(package));
        }
        public void OnRemovePackage(object package)
        {
            RemovePackage?.Invoke(this, new BaseEventArgs(package));
        }
        public void OnLoadPackage(string message)
        {
            LoadPackage?.Invoke(message);
        }
        public void OnCategoryChanged(object category)
        {
            CategoryChanged?.Invoke(this, new BaseEventArgs(category));
        }
    }
}
