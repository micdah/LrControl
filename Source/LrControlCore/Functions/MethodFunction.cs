using System;
using LrControl.Api;

namespace LrControl.Core.Functions
{
    internal class MethodFunction : Function
    {
        private readonly Action<LrApi> _method;
        private readonly string _displayText;

        public MethodFunction(LrApi api, string displayName, Action<LrApi> method, string displayText, string key) : base(api, displayName, key)
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