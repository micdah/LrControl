using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ToggleParameterFunction : Function
    {
        public IParameter<bool> Parameter { get; }

        public ToggleParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter<bool> parameter) : base(settings, api, displayName, key)
        {
            Parameter = parameter;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;

            if (Api.LrDevelopController.GetValue(out var enabled, Parameter))
            {
                Api.LrDevelopController.SetValue(Parameter, !enabled);

                ShowHud($"{(!enabled ? "Enabled" : "Disabled")}: {Parameter.DisplayName}");
            }
        }
    }
}