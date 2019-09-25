
namespace SimplePlugin
{
    using System.Windows;
    using InteractionLib;

    /// <summary>
    /// The class 1.
    /// </summary>
    public class MainPlugin : IPlugin
    {
        /// <summary>
        /// The run.
        /// </summary>
        public void Run()
        {
            MessageBox.Show("It works! This is SimplePlugin.");
        }
    }
}
