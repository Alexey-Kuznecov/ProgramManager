using System;

namespace ProgramManager.Services
{
    public class BaseEventArgs : EventArgs
    {
        public BaseEventArgs(object package)
        {
            Package = package;
        }
        public object Package { get; private set; }
    }
}
