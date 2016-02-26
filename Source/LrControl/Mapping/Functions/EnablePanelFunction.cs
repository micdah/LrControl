using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping.Functions
{
    public class EnablePanelFunction : Function
    {
        private readonly IParameter<bool> _enablePanelParamter;
        private readonly Panel _panel;

        public EnablePanelFunction(LrApi api, string displayName, Panel panel, IParameter<bool> enablePanelParameter, string key) : base(api, displayName, key)
        {
            _panel = panel;
            _enablePanelParamter = enablePanelParameter;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue != (int) Controller.Range.Maximum) return;

            var functionGroup = FunctionGroup.GetFunctionGroupFor(_panel);
            if (functionGroup != null)
            {
                if (!functionGroup.Enabled)
                {
                    functionGroup.Enable();

                    ShowHud($"Switched to panel: {functionGroup.Panel.Name}");
                }
                else
                {
                    if (_enablePanelParamter == null) return;

                    bool enabled;
                    if (Api.LrDevelopController.GetValue(out enabled, _enablePanelParamter))
                    {
                        Api.LrDevelopController.SetValue(_enablePanelParamter, !enabled);

                        ShowHud($"{(!enabled ? "Enabled" : "Disabled")} panel: {functionGroup.Panel.Name}");
                    }
                }
            }
            else
            {
                Api.LrDevelopController.RevealPanel(_panel);
                ShowHud($"Switched to panel: {_panel.Name}");
            }
        }
    }
}