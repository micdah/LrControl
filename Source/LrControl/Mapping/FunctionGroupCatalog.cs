using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using micdah.LrControl.Annotations;
using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrApplicationView;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping
{
    public class FunctionGroupCatalog : INotifyPropertyChanged
    {
        private ObservableCollection<ModuleGroup> _modules;
        private readonly object _modulesLock = new object();

        private FunctionGroupCatalog()
        {
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

        public void InitControllers(ControllerManager manager)
        {
            foreach (var module in Modules)
            {
                foreach (var group in module.FunctionGroups)
                {
                    group.ClearControllerFunctions();

                    foreach (var controller in manager.Controllers)
                    {
                        group.ControllerFunctions.Add(new ControllerFunction
                        {
                            Controller = controller
                        });
                    }
                }
            }
        }

        public static FunctionGroupCatalog DefaultGroups(LrApi api)
        {
            return new FunctionGroupCatalog
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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}