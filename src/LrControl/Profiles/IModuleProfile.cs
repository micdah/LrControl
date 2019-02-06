using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Profiles
{
    public interface IModuleProfile
    {
        Module Module { get; }

        void AssignFunction(in ControllerId controllerId, IFunction function);
        void ClearFunction(in ControllerId controllerId);
        void ApplyFunction(in ControllerId controllerId, int value, Range range, Module activeModule, Panel activePanel);
        bool HasFunction(in ControllerId controllerId);
    }
}