using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Resources.Exceptions
{
    class XmlElementNotFoundException : Exception
    {
        public XmlElementNotFoundException(string message)
           : base(message)
        {
            
        }
        public XmlElementNotFoundException()
            : base ("Элемент с имени ")
        {
                
        }
    }
}
