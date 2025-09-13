using ProgramManager.Enums;
using System.Collections.Generic;

namespace ProgramManager.Associations
{
    public class CategoryDictionary
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
