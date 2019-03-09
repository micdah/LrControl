using System;
using LrControl.Configurations.Models;
using LrControl.Devices;
using LrControl.Functions.Catalog;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Profiles;
using Serilog;

namespace LrControl.Configurations
{
    public interface IConfigurationManager
    {
        void Load(ConfigurationModel configuration);
    }
    
    public class ConfigurationManager : IConfigurationManager
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ConfigurationManager>();
        private readonly IFunctionCatalog _functionCatalog;
        private readonly IProfileManager _profileManager;

        public ConfigurationManager(IFunctionCatalog functionCatalog, IProfileManager profileManager)
        {
            _functionCatalog = functionCatalog ?? throw new ArgumentNullException(nameof(functionCatalog));
            _profileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
        }
        
        public void Load(ConfigurationModel configuration)
        {
            var profile = configuration.Profile ?? throw new ArgumentException($"Configuration must contain profile");

            // Assign module functions
            if (profile.Modules != null)
            {
                foreach (var entry in profile.Modules)
                {
                    var module = Module.GetEnumForValue(entry.Key);
                    if (module == null)
                    {
                        Log.Warning("Unable to find Module {Name}, skipping section", entry.Key);
                        continue;
                    }
                    
                    foreach (var mapping in entry.Value)
                    {
                        if (!_functionCatalog.TryGetFunctionFactory(mapping.Function, out var functionFactory))
                        {
                            Log.Warning("Unable to find Function {Function}, skipping mapping", mapping.Function);
                            continue;
                        }

                        var function = functionFactory.CreateFunction();
                        
                        var controllerId = new ControllerId(
                            mapping.Controller.MessageType,
                            mapping.Controller.Channel,
                            mapping.Controller.Parameter);

                        Log.Debug("Assigning function {Function} to controller {Controller} in module {Module}",
                            function.DisplayName, controllerId, module.Name);

                        _profileManager.AssignFunction(module, controllerId, function);
                    }
                }
            }
            
            // Assign panel functions
            if (profile.Panels != null)
            {
                foreach (var entry in profile.Panels)
                {
                    var panel = Panel.GetEnumForValue(entry.Key);
                    if (panel == null)
                    {
                        Log.Warning("Unable to find Panel {Name}, skipping section", entry.Key);
                        continue;
                    }

                    foreach (var mapping in entry.Value)
                    {
                        if (!_functionCatalog.TryGetFunctionFactory(mapping.Function, out var functionFactory))
                        {
                            Log.Warning("Unable to find Function {Function}, skipping mapping", mapping.Function);
                            continue;
                        }

                        var function = functionFactory.CreateFunction();

                        var controllerId = new ControllerId(
                            mapping.Controller.MessageType,
                            mapping.Controller.Channel,
                            mapping.Controller.Parameter);

                        Log.Debug("Assigning function {Function} to controller {Controller} in panel {Panel}",
                            function.DisplayName, controllerId, panel.Name);
                        
                        _profileManager.AssignPanelFunction(panel, controllerId, function);
                    }
                }
            }
        }
    }
}