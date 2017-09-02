using System;
using LrControl.Api;
using LrControl.Api.Common;

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

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (controllerValue != (int) controllerRange.Maximum) return;

            _method(Api);
            ShowHud(_displayText);
        }
    }
}