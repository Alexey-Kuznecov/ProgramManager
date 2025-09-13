
namespace ProgramManager.ViewModels.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;
    using Base;
    using InteractionLib;
    using Models.Settings;

    /// <summary>
    /// The plugin view model.
    /// </summary>
    public class PluginViewModel : PropertiesChanged
    {
        /// <summary>
        /// The current assembly.
        /// </summary>
        private Assembly _currentAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginViewModel"/> class.
        /// </summary>
        public PluginViewModel()
        {
            this.PluginList = new List<PluginModel>();
            this.GetPluginInfo();
        }
        
        /// <summary>
        /// The command open settings panel.
        /// </summary>
        public ICommand CommandOpenSettings => new RelayCommand(obj =>
        {
            var types = _currentAssembly.ExportedTypes;

            foreach (var item in types)
            {
                if (typeof(IPlugin).IsAssignableFrom(item))
                {
                    var dd = Activator.CreateInstance(item);
                }
            }
        });

        /// <summary>
        /// Gets the plugin list.
        /// </summary>
        public List<PluginModel> PluginList { get; }

        /// <summary>
        /// The get plugin info.
        /// </summary>
        public void GetPluginInfo()
        {
            foreach (var name in PluginManager.FindNameDirectories())
            {
                Assembly asm =
                    Assembly.LoadFrom(
                        AppDomain.CurrentDomain.BaseDirectory + @"\Plugins\" + name + "\\" + name + ".dll");

                this.PluginList.Add(new PluginModel
                {
                    Name = asm.CustomAttributes.Single(p => p.AttributeType.Name == "AssemblyTitleAttribute")
                        .ConstructorArguments[0].Value.ToString(),
                    Author = asm.CustomAttributes.Single(p => p.AttributeType.Name == "AssemblyCompanyAttribute")
                        .ConstructorArguments[0].Value.ToString(),
                    Version = asm.CustomAttributes.Single(p => p.AttributeType.Name == "AssemblyFileVersionAttribute")
                        .ConstructorArguments[0].Value.ToString(),
                    Description = asm.CustomAttributes
                        .Single(p => p.AttributeType.Name == "AssemblyDescriptionAttribute")
                        .ConstructorArguments[0].Value.ToString(),
                    Execute = new RelayCommand(obj =>
                    {
                        var types = asm.ExportedTypes;

                        foreach (var item in types)
                        {
                            if (typeof(IPlugin).IsAssignableFrom(item))
                            {
                                Activator.CreateInstance(item);
                            }
                        }
                    })
                });
            }
        }
    }
}
