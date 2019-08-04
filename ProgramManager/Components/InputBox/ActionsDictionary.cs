using System.Collections.Generic;

namespace ProgramManager.Components.InputBox
{
    class ActionsDictionary
    {
        private readonly Dictionary<Enums.Actions, string> _tableCode = new Dictionary<Enums.Actions, string>(5)
        {
            { Enums.Actions.Add,  "Добавить" },
            { Enums.Actions.Change,  "Изменить" },
            { Enums.Actions.Cancal,  "Отменить" },
            { Enums.Actions.Delete,  "Удалить" }
        };
        public Enums.Actions GetKey(string key)
        {
            foreach (var item in _tableCode)
                if (key == item.Value)
                    return item.Key;
            return Enums.Actions.Null;
        }
        public string GetValue(Enums.Actions value)
        {
            foreach (var item in _tableCode)
                if (value == item.Key)
                    return item.Value;
            return null;
        }
    }
}
