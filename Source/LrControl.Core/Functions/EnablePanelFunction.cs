using LrControl.Core.Configurations;
using LrControl.Core.Mapping;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class EnablePanelFunction : Function
    {
        private readonly IParameter<bool> _enablePanelParamter;
        private readonly Panel _panel;

        public EnablePanelFunction(ISettings settings, LrApi api, string displayName, Panel panel, IParameter<bool> enablePanelParameter, string key) : base(settings, api, displayName, key)
        {
            _panel = panel;
            _enablePanelParamter = enablePanelParameter;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (!controllerRange.IsMaximum(controllerValue)) return;

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