using System;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class EnumerationParameterFunction<T> : Function 
        where T : IComparable
    {
        private readonly IEnumerationParameter<T> _parameter;
        private readonly IEnumeration<T> _value;

        public EnumerationParameterFunction(ISettings settings, LrApi api, string displayName, string key, 
            IEnumerationParameter<T> parameter, IEnumeration<T> value) : base(settings, api, displayName, key)
        {
            _parameter = parameter;
            _value = value;
        }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (controllerValue != (int) controllerRange.Maximum) return;

            Api.LrDevelopController.SetValue<T>(_parameter, _value);
        }
    }
}