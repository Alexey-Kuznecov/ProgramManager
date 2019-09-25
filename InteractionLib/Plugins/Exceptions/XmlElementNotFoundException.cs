using System;

namespace InteractionLib.Plugins.Exceptions
{
    class XmlElementNotFoundException : Exception
    {
        public XmlElementNotFoundException(string message)
           : base(message){ }
        public XmlElementNotFoundException()
            : base ("Элемент с имени ") { }
    }
}
