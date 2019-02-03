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
        private Dictionary<(Panel, ControllerId), IFunction> _panelFunctions =
            new Dictionary<(Panel, ControllerId), IFunction>();
        
        public override Module Module => Module.Develop;
        public Panel ActivePanel { get; set; }

        public DevelopModuleProfile() : base(Module.Develop)
        {
        }

        public void AssignFunction(Panel panel, ControllerId controllerId, IFunction function)
        {
            _panelFunctions[(panel, controllerId)] = function;
        }

        public void ClearFunction(Panel panel, ControllerId controllerId)
        {
            _panelFunctions.Remove((panel, controllerId));
        }

        public override void OnControllerInput(ControllerId controllerId, int value, Range range)
        {
            if (ActivePanel != null &&
                _panelFunctions.TryGetValue((ActivePanel, controllerId), out var function))
            {
                function.Apply(value, range);
            }
            else
            {
                base.OnControllerInput(controllerId, value, range);
            }
        }
    }
}