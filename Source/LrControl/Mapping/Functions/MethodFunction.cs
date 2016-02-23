using System;
using micdah.LrControlApi;

namespace micdah.LrControl.Mapping.Functions
{
    public class MethodFunction : Function
    {
        private readonly Action<LrApi> _method;
        private readonly string _displayText;

        public MethodFunction(LrApi api, string displayName, Action<LrApi> method, string displayText) : base(api, displayName)
        {
            _method = method;
            _displayText = displayText;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue == (int) Controller.Range.Maximum)
            {
                _method(Api);
                ShowHud(_displayText);
            }
        }
    }
}