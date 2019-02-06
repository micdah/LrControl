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
        private readonly Dictionary<(Panel, ControllerId), IFunction> _panelFunctions =
            new Dictionary<(Panel, ControllerId), IFunction>();
        
        public override Module Module => Module.Develop;
        public Panel ActivePanel { get; set; }

        public DevelopModuleProfile() : base(Module.Develop)
        {
        }

        public void AssignFunction(Panel panel, in ControllerId controllerId, IFunction function)
        {
            _panelFunctions[(panel, controllerId)] = function;
        }

        public void ClearFunction(Panel panel, in ControllerId controllerId)
        {
            _panelFunctions.Remove((panel, controllerId));
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
            => (ActivePanel != null && _panelFunctions.ContainsKey((ActivePanel, controllerId))) ||
               base.HasFunction(in controllerId);

        protected override bool TryGetFunction(in ControllerId controllerId, out IFunction function)
        {
            if (ActivePanel != null && _panelFunctions.TryGetValue((ActivePanel, controllerId), out function))
                return true;
            return base.TryGetFunction(in controllerId, out function);
        }
    }
}