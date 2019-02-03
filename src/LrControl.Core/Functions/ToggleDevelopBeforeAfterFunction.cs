using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Core.Functions
{
    internal class ToggleDevelopBeforeAfterFunction : Function
    {
        private bool _toggled;

        public ToggleDevelopBeforeAfterFunction(ISettings settings, ILrApi api, string displayName, string key) : base(
            settings, api, displayName, key)
        {
        }

        public override void Apply(int value, Range range)
        {
            if (!range.IsMaximum(value)) return;

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