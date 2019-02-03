using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Profiles
{
    internal interface IModuleProfile
    {
        Module Module { get; }

        void AssignFunction(ControllerId controllerId, IFunction function);
        void ClearFunction(ControllerId controllerId);
        void OnControllerInput(ControllerId controllerId, int value, Range range);
    }
}