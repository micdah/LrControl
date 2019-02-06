using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using Serilog;

namespace LrControl.Functions
{
    public class RevealOrTogglePanelFunction : Function
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<RevealOrTogglePanelFunction>();
        private readonly Panel _panel;

        private static readonly Dictionary<Panel, IParameter<bool>> EnablePanelParameters =
            new Dictionary<Panel, IParameter<bool>>
            {
                {Panel.Basic, null},
                {Panel.Detail, EnablePanelParameter.Detail},
                {Panel.Effects, EnablePanelParameter.Effects},
                {Panel.ToneCurve, EnablePanelParameter.ToneCurve},
                {Panel.SplitToning, EnablePanelParameter.SplitToning},
                {Panel.ColorAdjustment, EnablePanelParameter.ColorAdjustments},
                {Panel.LensCorrections, EnablePanelParameter.LensCorrections},
                {Panel.CameraCalibration, EnablePanelParameter.Calibration}
            };

        public RevealOrTogglePanelFunction(ISettings settings, ILrApi api, string displayName, string key, Panel panel)
            : base(settings, api, displayName, key)
        {
            _panel = panel;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;

            if (activePanel != _panel || _panel == Panel.Basic)
            {
                // Reveal panel
                Api.LrDevelopController.RevealPanel(_panel);

                ShowHud($"Switched to panel {_panel.Name}");
            }
            else
            {
                // Toggle panel enabled/disabled
                var enablePanelParameter = EnablePanelParameters[_panel];
                if (Api.LrDevelopController.GetValue(out var enabled, enablePanelParameter))
                {
                    Api.LrDevelopController.SetValue(enablePanelParameter, !enabled);
                }
                else Log.Error("Unable to determine if Panel {Panel} is enabled or not", _panel.Name);
            }
        }
    }
}