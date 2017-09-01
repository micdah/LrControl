using LrControl.Api;
using LrControl.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class ToggleParameterFunction : Function
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunction(LrApi api, string displayName, IParameter<bool> parameter, string key) : base(api, displayName, key)
        {
            _parameter = parameter;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue == (int) Controller.Range.Maximum)
            {
                bool enabled;
                if (Api.LrDevelopController.GetValue(out enabled, _parameter))
                {
                    Api.LrDevelopController.SetValue(_parameter, !enabled);

                    ShowHud($"{(!enabled ? "Enabled" : "Disabled")}: {_parameter.DisplayName}");
                }
            }
        }
    }
}