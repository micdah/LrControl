using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControlCore.Device;
using micdah.LrControlApi.Modules.LrApplicationView;
using Serilog;

namespace LrControlCore.Mapping
{
    public class ModuleGroup : INotifyPropertyChanged
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<ModuleGroup>();

        private readonly List<FunctionGroup> _lastEnabledFunctionGroups = new List<FunctionGroup>();
        private bool _enabled;
        private ObservableCollection<FunctionGroup> _functionGroups;
        private Module _module;

        public ModuleGroup(Module module, IEnumerable<FunctionGroup> functionGroups)
        {
            Module = module;
            FunctionGroups = new ObservableCollection<FunctionGroup>(functionGroups);
        }

        public Module Module
        {
            get { return _module; }
            private set
            {
                if (Equals(value, _module)) return;
                _module = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FunctionGroup> FunctionGroups
        {
            get { return _functionGroups; }
            private set
            {
                if (Equals(value, _functionGroups)) return;
                _functionGroups = value;
                OnPropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            private set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Enable()
        {
            // Enable last enabled function group(s)
            foreach (var enabledGroup in _lastEnabledFunctionGroups)
            {
                enabledGroup.Enable();
            }

            // Enable all global groups
            foreach (var globalGroup in FunctionGroups.Where(globalGroup => globalGroup.IsGlobal))
            {
                globalGroup.Enable();
            }

            Enabled = true;

            Log.Debug("Enabled ModuleGroup for {Name}", Module.Name);
        }


        public void Disable()
        {
            if (!Enabled) return;

            _lastEnabledFunctionGroups.Clear();
            foreach (var group in FunctionGroups)
            {
                if (group.Enabled && !group.IsGlobal)
                {
                    _lastEnabledFunctionGroups.Add(group);
                }

                group.Disable();
            }

            Enabled = false;

            Log.Debug("Disabled ModuleGroup for {Name}", Module.Name);
        }

        public bool CanAssignFunction(Controller controller, bool inGlobalGroup)
        {
            if (inGlobalGroup)
            {
                return FunctionGroups
                    .Where(g => !g.IsGlobal)
                    .All(functionGroup => !functionGroup.ControllerFunctions
                        .Any(x => x.Controller == controller && x.Function != null));
            }

            // Otherwise
            return FunctionGroups
                .Where(g => g.IsGlobal)
                .All(functionGroup => !functionGroup.ControllerFunctions
                    .Any(x => x.Controller == controller && x.Function != null));
        }

        public void RecalculateControllerFunctionState()
        {
            var globalFunctions = FunctionGroups.Where(g => g.IsGlobal)
                .SelectMany(g => g.ControllerFunctions)
                .Where(cf => cf.Function != null).ToList();

            var nonGlobalFunctions = FunctionGroups.Where(g => !g.IsGlobal)
                .SelectMany(g => g.ControllerFunctions)
                .Where(cf => cf.Function != null).ToList();

            foreach (var functionGroup in FunctionGroups)
            {
                foreach (var controllerFunction in functionGroup.ControllerFunctions)
                {
                    controllerFunction.Assignable = functionGroup.IsGlobal
                        ? nonGlobalFunctions.All(cf => cf.Controller != controllerFunction.Controller)
                        : globalFunctions.All(cf => cf.Controller != controllerFunction.Controller);
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