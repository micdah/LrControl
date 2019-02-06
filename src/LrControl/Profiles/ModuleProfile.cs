using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Profiles
{
    public class ModuleProfile : IModuleProfile
    {
        private readonly Dictionary<ControllerId, IFunction> _functions = new Dictionary<ControllerId, IFunction>();
    
        public virtual Module Module { get; }

        public ModuleProfile(Module module)
        {
            Module = module;
        }

        public void AssignFunction(in ControllerId controllerId, IFunction function)
        {
            _functions[controllerId] = function;
        }

        public void ClearFunction(in ControllerId controllerId)
        {
            _functions.Remove(controllerId);
        }

        public virtual void ApplyFunction(in ControllerId controllerId, int value, Range range, Module activeModule, Panel activePanel)
        {
            if (TryGetFunction(controllerId, out var function))
            {
                function.Apply(value, range, activeModule, activePanel);
            }
        }

        public virtual bool HasFunction(in ControllerId controllerId)
            => _functions.ContainsKey(controllerId);

        public virtual IDictionary<ControllerId, ParameterFunction> GetParameterFunctions(IParameter parameter)
        {
            var result = new Dictionary<ControllerId, ParameterFunction>();
            foreach (var entry in _functions)
            {
                if (entry.Value is ParameterFunction parameterFunction &&
                    ReferenceEquals(parameterFunction.Parameter, parameter))
                {
                    result[entry.Key] = parameterFunction;
                }
            }

            return result;
        }

        protected virtual bool TryGetFunction(in ControllerId controllerId, out IFunction function)
            => _functions.TryGetValue(controllerId, out function);
    }
}