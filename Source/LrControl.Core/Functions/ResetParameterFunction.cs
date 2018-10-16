using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class ResetParameterFunction : Function
    {
        private readonly IParameter _parameter;

        public ResetParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter parameter) : base(settings, api, displayName, key)
        {
            _parameter = parameter;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (!controllerRange.IsMaximum(controllerValue)) return;

            Api.LrDevelopController.StopTracking();
            Api.LrDevelopController.ResetToDefault(_parameter);
            ShowHud($"Reset {_parameter.DisplayName} to default");
        }
    }
}