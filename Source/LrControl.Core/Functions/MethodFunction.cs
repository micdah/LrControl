using System;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.Core.Functions
{
    internal class MethodFunction : Function
    {
        private readonly Action<LrApi> _method;
        private readonly string _displayText;

        public MethodFunction(ISettings settings, LrApi api, string displayName, Action<LrApi> method,
            string displayText, string key) : base(settings, api, displayName, key)
        {
            _method = method;
            _displayText = displayText;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (!controllerRange.IsMaximum(controllerValue)) return;

            _method(Api);
            ShowHud(_displayText);
        }
    }
}