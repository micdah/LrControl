using System.Collections.Generic;
using System.Linq;
using LrControl.Core.Devices;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using Serilog;

namespace LrControl.Core.Mapping
{
    public class ModuleGroup
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ModuleGroup>();
        private readonly List<ControllerFunctionGroup> _lastEnabledFunctionGroups = new List<ControllerFunctionGroup>();
        private readonly List<ControllerFunctionGroup> _controllerFunctionGroups;

        internal ModuleGroup(Module module, List<ControllerFunctionGroup> controllerFunctionGroups)
        {
            Module = module;
            _controllerFunctionGroups = controllerFunctionGroups;
        }

        public Module Module { get; }
        public IEnumerable<ControllerFunctionGroup> ControllerFunctionGroups => _controllerFunctionGroups;
        public bool Enabled { get; private set; }

        internal void AddControllerFunctionGroup(ControllerFunctionGroup controllerFunctionGroup)
        {
            _controllerFunctionGroups.Add(controllerFunctionGroup);
        }

        internal void Enable()
        {
            // Enable last enabled function group(s)
            foreach (var enabledGroup in _lastEnabledFunctionGroups)
            {
                enabledGroup.Enable();
            }

            // Enable all global groups
            foreach (var globalGroup in ControllerFunctionGroups.Where(globalGroup => globalGroup.IsGlobal))
            {
                globalGroup.Enable();
            }

            Enabled = true;

            Log.Debug("Enabled ModuleGroup for {Name}", Module.Name);
        }


        internal void Disable()
        {
            if (!Enabled) return;

            _lastEnabledFunctionGroups.Clear();
            foreach (var group in ControllerFunctionGroups)
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
                return ControllerFunctionGroups
                    .Where(g => !g.IsGlobal)
                    .All(functionGroup => !functionGroup.ControllerFunctions
                        .Any(x => x.Controller == controller && x.Function != null));
            }

            // Otherwise
            return ControllerFunctionGroups
                .Where(g => g.IsGlobal)
                .All(functionGroup => !functionGroup.ControllerFunctions
                    .Any(x => x.Controller == controller && x.Function != null));
        }

        public void RecalculateControllerFunctionState()
        {
            var globalFunctions = ControllerFunctionGroups.Where(g => g.IsGlobal)
                .SelectMany(g => g.ControllerFunctions)
                .Where(cf => cf.Function != null).ToList();

            var nonGlobalFunctions = ControllerFunctionGroups.Where(g => !g.IsGlobal)
                .SelectMany(g => g.ControllerFunctions)
                .Where(cf => cf.Function != null).ToList();

            foreach (var functionGroup in ControllerFunctionGroups)
            {
                foreach (var controllerFunction in functionGroup.ControllerFunctions)
                {
                    controllerFunction.Assignable = functionGroup.IsGlobal
                        ? nonGlobalFunctions.All(cf => cf.Controller != controllerFunction.Controller)
                        : globalFunctions.All(cf => cf.Controller != controllerFunction.Controller);
                }
            }
        }
    }
}