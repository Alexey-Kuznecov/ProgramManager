using ProgramManager.Services;

namespace ProgramManager.Plugins.IconsEditor.Bin
{
    class StartPlugin
    {
        public static void RunPlugin(object src)
        {
            IconsEditor singleInstense = Singleton.SingleInstance<IconsEditor>();
            singleInstense.Show();
        }
    }
}
