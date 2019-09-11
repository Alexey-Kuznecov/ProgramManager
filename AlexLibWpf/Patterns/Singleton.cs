using System.Collections.Generic;
using System.Diagnostics;

namespace AlexLibWpf.Patterns
{
    [DebuggerStepThrough]
    public struct Singleton  
    {
        public static object Back = null;
        public static int Count = 0;
        public static bool Status = false;
        /// <summary>
        /// Это поле хранит все экзепляры, достовать от туда ссылки не разумно, так как получить любой экземпляр 
        /// можно через метод SingleInstance, достаточно указать тип желаемого класса в качестве параметра обобщеного метода,
        /// Поэтому я остовляю это поля закрытым и только для чтения, в целях безопасности разумеется.
        /// </summary>
        private static readonly List<object> Instance = new List<object>();
        /// <summary>
        /// Метод создает один единственный экземпляр простого класса, который нельзя преопределить в других классах. 
        /// Единственным экземпляром будет являться превый экземпляр, который был создан в классе или структуре.
        /// При этом нет никакой разницы в каком классе был объявлен экземпляр. Это особенно удобно проводить связь менжду 
        /// разными ViewModel окнами, раделять ресурсы и объекты. Чтобы создать единный экземпляр класса 
        /// нужно написать всего одну строку кода <example> YourType instance = Singleton.SingleInstance{YourType}(); </example>
        /// </summary>
        /// <typeparam name="T">Любой ссылочный объект.</typeparam>
        /// <returns>Единственный экземпляр.</returns>
        public static T SingleInstance<T>() where T : new ()
        {
            T singleInstance = new T();
            byte multi = 0, single = 0;
            // If method was call one time then needed instantiated
            if (Instance.Count == 0)  
                Instance.Add(new T());
            // Collection traversals and if type already exist returns it.
            for (var i = 0; i < Instance.Count; i++)
                if (Instance[i] is T)
                {
                    single++;
                    singleInstance = (T)Instance[i];
                }
                else multi++;
            // If iterator didn't found same type then adds new type in collection. 
            if (multi <= (multi - single))
                Instance.Add(new T());
            return singleInstance;
        }
        /// <summary>
        /// Object returns that was queried.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>Reference into object.</returns>
        public static T GetSingleInstance<T>() where T : new ()
        {
            foreach (var obj in Instance)
                if (obj is T)
                    return (T)obj;
            return new T();
        }
    }
}