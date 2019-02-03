using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Profiles
{
    internal class ModuleProfile : IModuleProfile
    {
        private readonly Dictionary<ControllerId, IFunction> _functions = new Dictionary<ControllerId, IFunction>();
    
        public virtual Module Module { get; }

        public ModuleProfile(Module module)
        {
            Module = module;
        }

        public void AssignFunction(ControllerId controllerId, IFunction function)
        {
            _functions[controllerId] = function;
        }

        public void ClearFunction(ControllerId controllerId)
        {
            _functions.Remove(controllerId);
        }

        public virtual void OnControllerInput(ControllerId controllerId, int value, Range range)
        {
            if (_functions.TryGetValue(controllerId, out var function))
            {
                function.Apply(value, range);
            }
        }
    }
}