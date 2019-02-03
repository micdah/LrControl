using System;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Profiles
{
    internal class DevelopModuleProfile : ModuleProfile
    {
        public override Module Module => Module.Develop;

        public DevelopModuleProfile() : base(Module.Develop)
        {
        }

        public void AssignFunction(Panel panel, ControllerId controllerId, IFunction function)
        {
            throw new NotImplementedException();
        }

        public void ClearFunction(Panel panel, ControllerId controllerId)
        {
            
        }

        public override void OnControllerInput(ControllerId controllerId, int value, Range range)
        {
            throw new NotImplementedException();
        }
    }
}