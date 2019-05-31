using ProgramManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramManager.Dict
{
    public class CategoryDict
    {
        private Dictionary<Categories, string> TableCode = new Dictionary<Categories, string>(5)
        {
            { Categories.Programs,  "Программы" },
            { Categories.Drivers,  "Драйвера" },
            { Categories.Mods,  "Моды" },
            { Categories.Plugins,  "Плагины" },
            { Categories.Games,  "Игры" }
        };
        public Categories GetKey(string Key)
        {
            foreach (var item in TableCode)
                if (Key == item.Value)
                    return item.Key;
            return Categories.Null;
        }
        public string GetValue(Categories Value)
        {
            foreach (var item in TableCode)
                if (Value == item.Key)
                    return item.Value;
            return null;
        }
    }
}
