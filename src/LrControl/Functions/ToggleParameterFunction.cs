using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ToggleParameterFunction : ToggleFunction
    {
        public IParameter<bool> Parameter { get; }

        public ToggleParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter<bool> parameter) : base(settings, api, displayName, key)
        {
            Parameter = parameter;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!Api.LrDevelopController.GetValue(out var enabled, Parameter)) return;
            
            Api.LrDevelopController.SetValue(Parameter, !enabled);

            ShowHud($"{(!enabled ? "Enabled" : "Disabled")}: {Parameter.DisplayName}");
        }
    }
}