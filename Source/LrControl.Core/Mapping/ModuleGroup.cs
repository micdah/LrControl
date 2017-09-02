using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Core.Devices;
using Serilog;

namespace LrControl.Core.Mapping
{
    public class ModuleGroup : INotifyPropertyChanged
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ModuleGroup>();
        private readonly List<FunctionGroup> _lastEnabledFunctionGroups = new List<FunctionGroup>();
        private readonly List<FunctionGroup> _functionGroups;
        private bool _enabled;

        public ModuleGroup(Module module, List<FunctionGroup> functionGroups)
        {
            Module = module;
            OnPropertyChanged(nameof(Module));

            _functionGroups = functionGroups;
            OnPropertyChanged(nameof(FunctionGroups));
        }

        public Module Module { get; }
        public IEnumerable<FunctionGroup> FunctionGroups => _functionGroups;

        public bool Enabled
        {
            get => _enabled;
            private set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddFunctionGroup(FunctionGroup functionGroup)
        {
            _functionGroups.Add(functionGroup);
            OnPropertyChanged(nameof(FunctionGroups));
        }

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