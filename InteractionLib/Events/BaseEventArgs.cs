using System;

namespace InteractionLib.Events
{
    public class BaseEventArgs : EventArgs
    {
        public BaseEventArgs(object param)
        {
            Package = param;
        }
        public object Package { get; }
    }
}
