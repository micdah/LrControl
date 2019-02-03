using System;
using LrControl.Configurations;
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

        public EnumerationParameterFunction(ISettings settings, ILrApi api, string displayName, string key, 
            IEnumerationParameter<T> parameter, IEnumeration<T> value) : base(settings, api, displayName, key)
        {
            _parameter = parameter;
            _value = value;
        }

        public override void Apply(int value, Range range)
        {
            if (!range.IsMaximum(value)) return;
            
            Api.LrDevelopController.SetValue(_parameter, _value);
        }
    }
}