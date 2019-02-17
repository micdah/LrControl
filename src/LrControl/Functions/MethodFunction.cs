using System;
using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class MethodFunction : ToggleFunction
    {
        public Action<ILrApi> Method { get; }
        private readonly string _displayText;

        public MethodFunction(ISettings settings, ILrApi api, string displayName, string key, Action<ILrApi> method,
            string displayText) : base(settings, api, displayName, key)
        {
            Method = method;
            _displayText = displayText;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            Method(Api);
            ShowHud(_displayText);
        }
    }
}