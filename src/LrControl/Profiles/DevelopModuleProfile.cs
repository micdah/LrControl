using System.Collections.Concurrent;
using System.Collections.Generic;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Profiles
{
    public class DevelopModuleProfile : ModuleProfile
    {
        private readonly ConcurrentDictionary<Panel, Dictionary<ControllerId, IFunction>> _panelFunctions =
            new ConcurrentDictionary<Panel, Dictionary<ControllerId, IFunction>>();
        
        public override Module Module => Module.Develop;
        public Panel ActivePanel { get; set; }

        public DevelopModuleProfile() : base(Module.Develop)
        {
        }

        public void AssignFunction(Panel panel, in ControllerId controllerId, IFunction function)
        {
            var id = controllerId;

            _panelFunctions.AddOrUpdate(panel,
                _ => new Dictionary<ControllerId, IFunction> {{id, function}},
                (_, dict) =>
                {
                    dict[id] = function;
                    return dict;
                });
        }

        public void ClearFunction(Panel panel, in ControllerId controllerId)
        {
            if (_panelFunctions.TryGetValue(panel, out var dict))
            {
                dict.Remove(controllerId);
            }
        }

        public override void ApplyFunction(in ControllerId controllerId, int value, Range range, Module activeModule, Panel activePanel)
        {
            if (TryGetFunction(in controllerId, out var function))
            {
                function.Apply(value, range, activeModule, activePanel);
                
                // Reveal panel function?
                if (function is RevealOrTogglePanelFunction panelFunction)
                {
                    ActivePanel = panelFunction.Panel;
                }
            }
        }

        public override bool HasFunction(in ControllerId controllerId)
            => (ActivePanel != null &&
                _panelFunctions.TryGetValue(ActivePanel, out var dict) &&
                dict.ContainsKey(controllerId)) ||
               base.HasFunction(in controllerId);

        public override IEnumerable<(ControllerId, ParameterFunction)> GetParameterFunctions(IParameter parameter)
        {
            if (!_panelFunctions.TryGetValue(ActivePanel, out var activePanelFunction))
            {
                foreach (var entry in base.GetParameterFunctions(parameter))
                    yield return entry;
            }
            else
            {
                // Active panel functions
                foreach (var entry in activePanelFunction)
                {
                    if (entry.Value is ParameterFunction parameterFunction &&
                        ReferenceEquals(parameterFunction.Parameter, parameter))
                    {
                        yield return (entry.Key, parameterFunction);
                    }
                }

                // Module functions
                foreach (var entry in Functions)
                {
                    if (!activePanelFunction.ContainsKey(entry.Key) &&
                        entry.Value is ParameterFunction parameterFunction &&
                        ReferenceEquals(parameterFunction.Parameter, parameter))
                    {
                        yield return (entry.Key, parameterFunction);
                    }
                }
            }
        }

        protected override bool TryGetFunction(in ControllerId controllerId, out IFunction function)
        {
            if (ActivePanel != null && 
                _panelFunctions.TryGetValue(ActivePanel, out var dict) &&
                dict.TryGetValue(controllerId, out function))
                return true;
            
            return base.TryGetFunction(in controllerId, out function);
        }
    }
}