using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping.Functions
{
    public class EnableFunctionGroupFunction : Function
    {
        private readonly IParameter<bool> _enablePanelParamter;
        private readonly FunctionGroup _functionGroup;

        public EnableFunctionGroupFunction(LrControlApi.LrControlApi api, FunctionGroup functionGroup,
            IParameter<bool> enablePanelParameter) : base(api)
        {
            _functionGroup = functionGroup;
            _enablePanelParamter = enablePanelParameter;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue != (int) Controller.Range.Maximum) return;

            if (!_functionGroup.Enabled)
            {
                _functionGroup.Enable();

                ShowHud($"Switched to panel: {_functionGroup.Panel.Name}");
            }
            else
            {
                if (_enablePanelParamter == null) return;

                bool enabled;
                if (Api.LrDevelopController.GetValue(out enabled, _enablePanelParamter))
                {
                    Api.LrDevelopController.SetValue(_enablePanelParamter, !enabled);

                    ShowHud($"{(!enabled ? "Enabled" : "Disabled")} panel: {_functionGroup.Panel.Name}");
                }
            }
        }
    }
}