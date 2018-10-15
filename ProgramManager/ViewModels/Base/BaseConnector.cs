using System;
using ProgramManager.Models;

namespace ProgramManager.ViewModels
{
    public class BaseConnector
    {
        public BaseConnector()
        {
            BaseModel model = new BaseModel();
            PackageChanged += model.AddNewPackage;
        }
        public void OnPackageChanged(object package)
        {
            PackageChanged?.Invoke(this, new ConnectorEventArgs(package));
        }
        public EventHandler<ConnectorEventArgs> PackageChanged { get; set; }
    }
}
