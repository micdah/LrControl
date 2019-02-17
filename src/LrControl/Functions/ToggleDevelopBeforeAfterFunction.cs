using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ToggleDevelopBeforeAfterFunction : ToggleFunction
    {
        private bool _toggled;

        public ToggleDevelopBeforeAfterFunction(ISettings settings, ILrApi api, string displayName, string key) : base(
            settings, api, displayName, key)
        {
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (_toggled)
            {
                Api.LrApplicationView.ShowView(PrimaryView.DevelopLoupe);
                ShowHud("After");
                _toggled = false;
            }
            else
            {
                Api.LrApplicationView.ShowView(PrimaryView.DevelopBefore);
                ShowHud("Before");
                _toggled = true;
            }
        }
    }
}