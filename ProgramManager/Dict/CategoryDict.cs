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
        private readonly Dictionary<Categories, string> _tableCode = new Dictionary<Categories, string>(5)
        {
            { Categories.Programs,  "Программы" },
            { Categories.Drivers,  "Драйвера" },
            { Categories.Mods,  "Моды" },
            { Categories.Plugins,  "Плагины" },
            { Categories.Games,  "Игры" }
        };
        public Categories GetKey(string key)
        {
            foreach (var item in _tableCode)
                if (key == item.Value)
                    return item.Key;
            return Categories.Null;
        }
        public string GetValue(Categories value)
        {
            foreach (var item in _tableCode)
                if (value == item.Key)
                    return item.Value;
            return null;
        }
    }
}
