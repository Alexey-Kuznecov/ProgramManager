using System;
using ProgramManager.ViewModels.Base;

namespace ProgramManager.ViewModels
{
    public class BaseConnector
    {
        public static event EventHandler<ConnectorEventArgs> PackageChanged;
        public static event EventHandler<ConnectorEventArgs> TagListUpdate;

        public void OnPackageChanged(object package)
        {
            PackageChanged?.Invoke(this, new ConnectorEventArgs(package));
        }
        public void OnLoadTagsList(object package)
        {
            TagListUpdate?.Invoke(this, new ConnectorEventArgs(package));
        }
    }
}
