using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControl.Mapping.Catalog;
using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrApplicationView;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping
{
    public class FunctionGroupManager : INotifyPropertyChanged
    {
        private readonly FunctionCatalog _functionCatalog;
        private readonly ControllerManager _controllerManager;
        private readonly object _modulesLock = new object();
        private ObservableCollection<ModuleGroup> _modules;

        private FunctionGroupManager(FunctionCatalog functionCatalog, ControllerManager controllerManager)
        {
            _functionCatalog = functionCatalog;
            _controllerManager = controllerManager;
        }

        public ObservableCollection<ModuleGroup> Modules
        {
            get { return _modules; }
            set
            {
                if (Equals(value, _modules)) return;
                _modules = value;
                BindingOperations.EnableCollectionSynchronization(_modules, _modulesLock);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static FunctionGroupManager DefaultGroups(LrApi api, FunctionCatalog functionCatalog, ControllerManager controllerManager)
        {
            return new FunctionGroupManager(functionCatalog, controllerManager)
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
                @group.FunctionGroups.Add(new FunctionGroup(api, panel)
                {
                    Key = $"{Module.Develop.Value}:{panel.Value}"
                });
            }

            return @group;
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
            }
        }

        public void Reset()
        {
            foreach (var module in Modules)
            {
                foreach (var @group in module.FunctionGroups)
                {
                    @group.ClearControllerFunctions();

                    foreach (var controller in _controllerManager.Controllers)
                    {
                        @group.ControllerFunctions.Add(new ControllerFunction
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
            foreach (var moduleGroup in Modules)
            {
                if (moduleGroup.Module == module)
                {
                    moduleGroup.Enable();
                }
                else
                {
                    moduleGroup.Disable();
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