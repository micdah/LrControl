using System.Collections.Generic;
using System.Linq;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Functions.Catalog;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Mapping
{
    public class FunctionGroupManager
    {
        private readonly IFunctionCatalog _functionCatalog;
        private readonly DeviceManager _deviceManager;
        private readonly List<ModuleGroup> _modules;

        private FunctionGroupManager(IFunctionCatalog functionCatalog, DeviceManager deviceManager, List<ModuleGroup> modules)
        {
            _functionCatalog = functionCatalog;
            _deviceManager = deviceManager;
            _deviceManager.ControllerAdded += DeviceManagerOnControllerAdded;
            _modules = modules;
        }

        public IEnumerable<ModuleGroup> Modules => _modules;

        internal static FunctionGroupManager DefaultGroups(LrApi api, IFunctionCatalog functionCatalog, DeviceManager deviceManager)
        {
            return new FunctionGroupManager(functionCatalog, deviceManager, new List<ModuleGroup>
            {
                CreateModuleWithGlobal(api, Module.Library),
                CreateDevelopModule(api),
                CreateModuleWithGlobal(api, Module.Map),
                CreateModuleWithGlobal(api, Module.Book),
                CreateModuleWithGlobal(api, Module.Slideshow),
                CreateModuleWithGlobal(api, Module.Print),
                CreateModuleWithGlobal(api, Module.Web)
            });
        }

        private static ModuleGroup CreateModuleWithGlobal(LrApi api, Module module)
        {
            return new ModuleGroup(module, new List<FunctionGroup>
            {
                new FunctionGroup(api)
                {
                    Key = $"{module.Value}:Global"
                }
            });
        }

        private static ModuleGroup CreateDevelopModule(LrApi api)
        {
            var group = new ModuleGroup(Module.Develop, new List<FunctionGroup>
            {
                new FunctionGroup(api)
                {
                    Key = $"{Module.Develop.Value}:Global"
                }
            });

            foreach (var panel in Panel.GetAll())
            {
                group.AddFunctionGroup(new FunctionGroup(api, panel)
                {
                    Key = $"{Module.Develop.Value}:{panel.Value}"
                });
            }

            return group;
        }

        internal void Load(IEnumerable<ModuleConfiguration> moduleConfigurations)
        {
            Reset();

            foreach (var moduleConfiguration in moduleConfigurations)
            {
                // Find matching module
                var module = Modules.SingleOrDefault(m => m.Module.Value == moduleConfiguration.ModuleName);
                if (module == null) continue;

                foreach (var functionGroupConfiguration in moduleConfiguration.FunctionGroups)
                {
                    // Find matching function group
                    var functionGroup = module.FunctionGroups.SingleOrDefault(g => g.Key == functionGroupConfiguration.Key);
                    if (functionGroup == null) continue;

                    foreach (var controllerFunctionConfiguration in functionGroupConfiguration.ControllerFunctions)
                    {
                        // Find controller function, for controller key
                        var controllerFunction = functionGroup.ControllerFunctions
                            .SingleOrDefault(c => c.Controller.IsController(controllerFunctionConfiguration.ControllerKey));
                        if (controllerFunction == null) continue;

                        // Find function factory, for function key
                        var functionFactory = _functionCatalog.GetFunctionFactory(controllerFunctionConfiguration.FunctionKey);
                        if (functionFactory == null) continue;

                        controllerFunction.Function = functionFactory.CreateFunction();
                    }
                }

                module.RecalculateControllerFunctionState();
            }
        }

        internal void Reset()
        {
            foreach (var module in Modules)
            {
                foreach (var group in module.FunctionGroups)
                {
                    group.ClearControllerFunctions();

                    foreach (var controller in _deviceManager.Controllers)
                    {
                        group.AddControllerFunction(new ControllerFunction(controller));
                    }
                }
            }
        }

        internal List<ModuleConfiguration> GetConfiguration()
        {
            return Modules.Select(x => new ModuleConfiguration(x)).ToList();
        }

        internal void EnableModule(Module module)
        {
            // First disable all other module groups
            foreach (var moduleGroup in Modules.Where(g => g.Module != module))
            {
                if (moduleGroup.Module != module)
                {
                    moduleGroup.Disable();
                }
            }

            // Now enable module group
            foreach (var moduleGroup in Modules.Where(g => g.Module == module))
            {
                moduleGroup.Enable();
            }
        }

        private void DeviceManagerOnControllerAdded(object sender, Controller controller)
        {
            // Add new controller to each function group, within each module
            foreach (var module in Modules)
            {
                foreach (var group in module.FunctionGroups)
                {
                    group.AddControllerFunction(new ControllerFunction(controller));
                }
            }
        }
    }
}