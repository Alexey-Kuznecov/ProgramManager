using System;
using ProgramManager.Models;

namespace ProgramManager.Contracts
{
    public class BaseConnector
    {
        public BaseConnector()
        {
            BaseModel model = new BaseModel();
            PackageChanged += model.AddNewPackage;
        }
        public void OnPackageChanged(PackageModel package)
        {
            PackageChanged?.Invoke(this, new PackageEventArgs(package));
        }
        public EventHandler<PackageEventArgs> PackageChanged { get; set; }
    }
}
