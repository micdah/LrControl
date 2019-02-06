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
            if (TryGetFunction(in controllerId, activeModule, activePanel, out var function))
            {
                function.Apply(value, range, activeModule, activePanel);
                
                // Reveal panel function?
                if (function is RevealOrTogglePanelFunction panelFunction)
                {
                    ActivePanel = panelFunction.Panel;
                }
            }
        }

        protected override bool TryGetFunction(in ControllerId controllerId, Module activeModule, Panel activePanel, out IFunction function)
        {
            if (activePanel != null && _panelFunctions.TryGetValue((activePanel, controllerId), out function))
                return true;
            return base.TryGetFunction(in controllerId, activeModule, activePanel, out function);
        }
    }
}