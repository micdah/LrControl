using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Core.Configurations;
using LrControl.Core.Device;
using LrControl.Core.Functions.Catalog;

namespace LrControl.Core.Mapping
{
    public class FunctionGroupManager : INotifyPropertyChanged
    {
        private readonly IFunctionCatalog _functionCatalog;
        private readonly MidiDevice _midiDevice;
        private ObservableCollection<ModuleGroup> _modules;

        private FunctionGroupManager(IFunctionCatalog functionCatalog, MidiDevice midiDevice)
        {
            _functionCatalog = functionCatalog;
            _midiDevice = midiDevice;
        }

        public ObservableCollection<ModuleGroup> Modules
        {
            get => _modules;
            set
            {
                if (Equals(value, _modules)) return;
                _modules = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static FunctionGroupManager DefaultGroups(LrApi api, IFunctionCatalog functionCatalog, MidiDevice midiDevice)
        {
            return new FunctionGroupManager(functionCatalog, midiDevice)
            {
                Modules = new ObservableCollection<ModuleGroup>
                {
                    CreateModuleWithGlobal(api, Module.Library),
                    CreateDevelopModule(api),
                    CreateModuleWithGlobal(api, Module.Map),
                    CreateModuleWithGlobal(api, Module.Book),
                    CreateModuleWithGlobal(api, Module.Slideshow),
                    CreateModuleWithGlobal(api, Module.Print),
                    CreateModuleWithGlobal(api, Module.Web)
                }
            };
        }

        private static ModuleGroup CreateModuleWithGlobal(LrApi api, Module module)
        {
            return new ModuleGroup(module, new[]
            {
                new FunctionGroup(api)
                {
                    Key = $"{module.Value}:Global"
                }
            });
        }

        private static ModuleGroup CreateDevelopModule(LrApi api)
        {
            var group = new ModuleGroup(Module.Develop, new[]
            {
                new FunctionGroup(api)
                {
                    Key = $"{Module.Develop.Value}:Global"
                }
            });

            foreach (var panel in Panel.AllEnums)
            {
                group.FunctionGroups.Add(new FunctionGroup(api, panel)
                {
                    Key = $"{Module.Develop.Value}:{panel.Value}"
                });
            }

            return group;
        }

        public void Load(List<ModuleConfiguration> modules)
        {
            Reset();

            foreach (var confModule in modules)
            {
                // Find matching module
                var module = Modules.SingleOrDefault(m => m.Module.Value == confModule.ModuleName);
                if (module == null) continue;

                foreach (var confFunctionGroup in confModule.FunctionGroups)
                {
                    // Find matching function group
                    var functionGroup = module.FunctionGroups.SingleOrDefault(g => g.Key == confFunctionGroup.Key);
                    if (functionGroup == null) continue;

                    foreach (var confControllerFunction in confFunctionGroup.ControllerFunctions)
                    {
                        // Find controller function, for controller key
                        var controllerFunction =
                            functionGroup.ControllerFunctions.SingleOrDefault(
                                c => c.Controller.IsController(confControllerFunction.ControllerKey));
                        if (controllerFunction == null) continue;

                        // Find function factory, for function key
                        var functionFactory = _functionCatalog.GetFunctionFactory(confControllerFunction.FunctionKey);
                        if (functionFactory == null) continue;

                        controllerFunction.Function = functionFactory.CreateFunction();
                    }
                }

                module.RecalculateControllerFunctionState();
            }
        }

        public void Reset()
        {
            foreach (var module in Modules)
            {
                foreach (var group in module.FunctionGroups)
                {
                    group.ClearControllerFunctions();

                    foreach (var controller in _midiDevice.Controllers)
                    {
                        group.ControllerFunctions.Add(new ControllerFunction
                        {
                            Controller = controller
                        });
                    }
                }
            }
        }

        public List<ModuleConfiguration> GetConfiguration()
        {
            return Modules.Select(module => new ModuleConfiguration
            {
                ModuleName = module.Module.Value,
                FunctionGroups = module.FunctionGroups
                    .Select(functionGroup => new FunctionGroupConfiguration
                    {
                        Key = functionGroup.Key,
                        ControllerFunctions = functionGroup.ControllerFunctions
                            .Select(x => new ControllerFunctionConfiguration
                            {
                                ControllerKey = x.Controller.GetConfigurationKey(),
                                FunctionKey = x.Function?.Key
                            }).ToList()
                    }).ToList()
            }).ToList();
        }

        public void EnableModule(Module module)
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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}