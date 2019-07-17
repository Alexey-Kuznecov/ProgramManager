using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Resources;

namespace ProgramManager.Services
{
    struct Singleton  
    {
        public static List<object> Instance = new List<object>();

        public static T InitialInstance<T>() where T : new ()
        {
            if (Instance.Count == 0)
                Instance.Add(new T());

            foreach (var item in Instance)
            {
                bool d = item is T;
                if (d)
                    return (T)item;
                else
                    Instance.Add(new T());
            }
            return new T();
        }
        public static object Back = null;
        public static int Count = 0;
        public static int Counter = 0;
        public static bool Status = false;
    }
}
