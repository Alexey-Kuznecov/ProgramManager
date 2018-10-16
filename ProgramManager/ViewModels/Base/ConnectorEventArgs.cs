using System;

namespace ProgramManager.ViewModels.Base
{
    public class ConnectorEventArgs : EventArgs
    {
        public ConnectorEventArgs(object package)
        {
            Package = package;
        }
        public object Package { get; private set; }
    }
}
