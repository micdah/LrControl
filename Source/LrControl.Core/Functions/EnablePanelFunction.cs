using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Core.Mapping;

namespace LrControl.Core.Functions
{
    internal class EnablePanelFunction : Function
    {
        private readonly IParameter<bool> _enablePanelParamter;
        private readonly Panel _panel;

        public EnablePanelFunction(LrApi api, string displayName, Panel panel, IParameter<bool> enablePanelParameter, string key) : base(api, displayName, key)
        {
            _panel = panel;
            _enablePanelParamter = enablePanelParameter;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (controllerValue != (int) controllerRange.Maximum) return;

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

                    if (Api.LrDevelopController.GetValue(out var enabled, _enablePanelParamter))
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