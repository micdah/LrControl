using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Core.Configurations;

namespace LrControl.Core.Functions
{
    internal class ToggleParameterFunction : Function
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunction(ISettings settings, LrApi api, string displayName, IParameter<bool> parameter, string key) : base(settings, api, displayName, key)
        {
            _parameter = parameter;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (controllerValue != (int) controllerRange.Maximum) return;

            if (Api.LrDevelopController.GetValue(out var enabled, _parameter))
            {
                Api.LrDevelopController.SetValue(_parameter, !enabled);

                ShowHud($"{(!enabled ? "Enabled" : "Disabled")}: {_parameter.DisplayName}");
            }
        }
    }
}