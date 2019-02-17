using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Functions.Catalog;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Mapping
{
    public class FunctionMappingManager : IDisposable
    {
        private bool _disposed;
        private readonly ILrApi _lrApi;
        private readonly IFunctionCatalog _functionCatalog;
        private readonly DeviceManager _deviceManager;
        private readonly List<ModuleGroup> _modules;

        private FunctionMappingManager(ILrApi lrApi, IFunctionCatalog functionCatalog, DeviceManager deviceManager, List<ModuleGroup> modules)
        {
            _lrApi = lrApi;
            _functionCatalog = functionCatalog;
            _deviceManager = deviceManager;
            _deviceManager.ControllerAdded += DeviceManagerOnControllerAdded;
            _modules = modules;
            
            _lrApi.LrDevelopController.ParameterChanged += LrDevelopControllerOnParameterChanged;
        }
        
        public IEnumerable<ModuleGroup> Modules => _modules;

        internal static FunctionMappingManager Create(ILrApi api, IFunctionCatalog functionCatalog, DeviceManager deviceManager)
        {
            ModuleGroup CreateModuleWithGlobal(Module module)
            {
                return new ModuleGroup(module, new List<ControllerFunctionGroup>
                {
                    new ControllerFunctionGroup(api)
                    {
                        Key = $"{module.Value}:Global"
                    }
                });
            }
            
            ModuleGroup CreateDevelopModule()
            {
                var group = new ModuleGroup(Module.Develop, new List<ControllerFunctionGroup>
                {
                    new ControllerFunctionGroup(api)
                    {
                        Key = $"{Module.Develop.Value}:Global"
                    }
                });

                foreach (var panel in Panel.GetAll())
                {
                    group.AddControllerFunctionGroup(new ControllerFunctionGroup(api, panel)
                    {
                        Key = $"{Module.Develop.Value}:{panel.Value}"
                    });
                }

                return group;
            }
            
            return new FunctionMappingManager(api, functionCatalog, deviceManager, new List<ModuleGroup>
            {
                CreateModuleWithGlobal(Module.Library),
                CreateDevelopModule(),
                CreateModuleWithGlobal(Module.Map),
                CreateModuleWithGlobal(Module.Book),
                CreateModuleWithGlobal(Module.Slideshow),
                CreateModuleWithGlobal(Module.Print),
                CreateModuleWithGlobal(Module.Web)
            });
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
                    var functionGroup = module.ControllerFunctionGroups.SingleOrDefault(g => g.Key == functionGroupConfiguration.Key);
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
                foreach (var group in module.ControllerFunctionGroups)
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
            foreach (var moduleGroup in Modules.Where(g => !Equals(g.Module, module)))
            {
                if (!Equals(moduleGroup.Module, module))
                {
                    moduleGroup.Disable();
                }
            }

            // Now enable module group
            foreach (var moduleGroup in Modules.Where(g => Equals(g.Module, module)))
            {
                moduleGroup.Enable();
            }
        }

        private void DeviceManagerOnControllerAdded(object sender, Controller controller)
        {
            // Add new controller to each function group, within each module
            foreach (var module in Modules)
            {
                foreach (var group in module.ControllerFunctionGroups)
                {
                    group.AddControllerFunction(new ControllerFunction(controller));
                }
            }
        }
        
        private void LrDevelopControllerOnParameterChanged(IParameter parameter)
        {
            foreach (var moduleGroup in Modules)
            {
                foreach (var controllerFunctionGroup in moduleGroup.ControllerFunctionGroups)
                {
                    foreach (var controllerFunction in controllerFunctionGroup.ControllerFunctions)
                    {
                        controllerFunction.UpdateController(parameter);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _lrApi.LrDevelopController.ParameterChanged -= LrDevelopControllerOnParameterChanged;
            _disposed = true;
        }
    }
}