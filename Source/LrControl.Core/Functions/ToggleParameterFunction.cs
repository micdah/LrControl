using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class ToggleParameterFunction : Function
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunction(ISettings settings, LrApi api, string displayName, string key,
            IParameter<bool> parameter) : base(settings, api, displayName, key)
        {
            _parameter = parameter;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (!controllerRange.IsMaximum(controllerValue)) return;

            if (Api.LrDevelopController.GetValue(out var enabled, _parameter))
            {
                Api.LrDevelopController.SetValue(_parameter, !enabled);

                ShowHud($"{(!enabled ? "Enabled" : "Disabled")}: {_parameter.DisplayName}");
            }
        }
    }
}