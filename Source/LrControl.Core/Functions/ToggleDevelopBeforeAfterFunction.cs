using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Core.Configurations;

namespace LrControl.Core.Functions
{
    internal class ToggleDevelopBeforeAfterFunction : Function
    {
        private bool _toggled;

        public ToggleDevelopBeforeAfterFunction(ISettings settings, LrApi api, string displayName, string key) : base(settings, api, displayName, key)
        {
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (controllerValue != (int) controllerRange.Maximum) return;

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