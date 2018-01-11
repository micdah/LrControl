using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Functions.Catalog;

namespace LrControl.Core.Mapping
{
    public class FunctionGroupManager : INotifyPropertyChanged
    {
        private readonly IFunctionCatalog _functionCatalog;
        private readonly Device _device;
        private readonly List<ModuleGroup> _modules;

        private FunctionGroupManager(IFunctionCatalog functionCatalog, Device device, List<ModuleGroup> modules)
        {
            _functionCatalog = functionCatalog;
            _device = device;
            _device.ControllerAdded += DeviceOnControllerAdded;
            _modules = modules;
            OnPropertyChanged(nameof(Modules));
        }

        public IEnumerable<ModuleGroup> Modules => _modules;

        public event PropertyChangedEventHandler PropertyChanged;

        internal static FunctionGroupManager DefaultGroups(LrApi api, IFunctionCatalog functionCatalog, Device device)
        {
            return new FunctionGroupManager(functionCatalog, device, new List<ModuleGroup>
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

            foreach (var panel in Panel.AllEnums)
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

                    foreach (var controller in _device.Controllers)
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

        private void DeviceOnControllerAdded(Controller controller)
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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}