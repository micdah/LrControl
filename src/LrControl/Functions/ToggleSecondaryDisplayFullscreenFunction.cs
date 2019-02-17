using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ToggleSecondaryDisplayFullscreenFunction : ToggleFunction
    {
        public ToggleSecondaryDisplayFullscreenFunction(ISettings settings, ILrApi api, string displayName, string key)
            : base(settings, api, displayName, key)
        {
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            Api.LrApplicationView.ToggleSecondaryDisplayFullscreen();
        }
    }
}