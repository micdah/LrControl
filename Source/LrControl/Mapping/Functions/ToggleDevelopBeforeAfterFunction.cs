using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrApplicationView;

namespace micdah.LrControl.Mapping.Functions
{
    public class ToggleDevelopBeforeAfterFunction : Function
    {
        private bool _toggled;

        public ToggleDevelopBeforeAfterFunction(LrApi api, string displayName, string key) : base(api, displayName, key)
        {
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue != (int) Controller.Range.Maximum) return;

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