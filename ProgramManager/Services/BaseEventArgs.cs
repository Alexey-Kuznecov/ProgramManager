using ProgramManager.Models.PackageModel;
using System;

namespace ProgramManager.Services
{
    public class BaseEventArgs : EventArgs
    {
        public BaseEventArgs(object param)
        {
            Package = param;
        }
        public object Package { get; private set; }
    }
}
