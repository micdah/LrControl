using System;
using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class MethodFunction : Function
    {
        private readonly Action<ILrApi> _method;
        private readonly string _displayText;

        public MethodFunction(ISettings settings, ILrApi api, string displayName, Action<ILrApi> method,
            string displayText, string key) : base(settings, api, displayName, key)
        {
            _method = method;
            _displayText = displayText;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;

            _method(Api);
            ShowHud(_displayText);
        }
    }
}