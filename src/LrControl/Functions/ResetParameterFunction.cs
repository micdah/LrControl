using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ResetParameterFunction : ToggleFunction
    {
        public IParameter Parameter { get; }

        public ResetParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter parameter) : base(settings, api, displayName, key)
        {
            Parameter = parameter;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            Api.LrDevelopController.StopTracking();
            Api.LrDevelopController.ResetToDefault(Parameter);
            ShowHud($"Reset {Parameter.DisplayName} to default");
        }
    }
}