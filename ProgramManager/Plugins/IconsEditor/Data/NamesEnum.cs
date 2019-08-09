namespace ProgramManager.Plugins.IconsEditor.Data
{
    /// <summary>
    /// Names of collection.
    /// </summary>
    enum NamesEnum : byte
    {
        Unsigned = 0,
        Game = 1
    }
    static class Names
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetName(this NamesEnum code)
        {
            switch (code)
            {
                case NamesEnum.Unsigned:
                    return "Неподшитые";
                case NamesEnum.Game:
                    return "Игры";
            }
            return null;
        }
    }
}
