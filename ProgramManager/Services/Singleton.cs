using System.Collections.Generic;

namespace ProgramManager.Services
{
    struct Singleton  
    {
        private static readonly List<object> Instance = new List<object>();

        public static T SingleInstance<T>() where T : new ()
        {
            T store = new T();
            byte up = 0, low = 0;

            if (Instance.Count == 0)
                Instance.Add(new T());

            for (var i = 0; i < Instance.Count; i++)
                if (Instance[i] is T) low++;
                else up++;

            if (up <= up - low)
                Instance.Add(new T());
            return store;
        }
        public static object GetSingleInsance<T>()
        {
            foreach (var j in Instance)
                if (j is T)
                    return (T)j;
            return null;
        }
        public static object Back = null;
        public static int Count = 0;
        public static int Counter = 0;
        public static bool Status = false;
    }
}
