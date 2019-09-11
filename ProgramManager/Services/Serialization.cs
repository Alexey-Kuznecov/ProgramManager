using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Services
{
    [DebuggerStepThrough]
    public class Serialization
    {
        /// <summary>
        /// Бинарная сериализация данных объектов.
        /// Внимание: Если объект наследует другие объекты они должны быть также помечены атрибутом [Serializable].
        /// </summary>
        /// <param name="obj">Любой объект который отмечен как сериaлизуемый.</param>
        /// <param name="filename">Filename to serialize object.</param>
        public static void BinSerialize(object obj, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, obj);
            }
        }
        /// <summary>
        /// Бинарная десериализация данных объектов из файла.
        /// </summary>
        /// <param name="obj">Возвращает объект, который необходимо будет привести к объекту,
        /// который подвергался сериализации.</param>
        /// <param name="filename">Filename to deserialize object.</param>
        public static void BinDeserialize(out object obj, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                obj = bf.Deserialize(fs);
            }
        }
    }
}
