using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Contracts
{
    interface ISearchable
    {
        void Search();
        void SearchResult();
        void RestoreState();
    }
}
