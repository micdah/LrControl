using System;
using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class EnumerationParameterFunction<T> : Function 
        where T : IComparable
    {
        public IEnumerationParameter<T> Parameter { get; }
        public IEnumeration<T> Value { get; }

        public EnumerationParameterFunction(ISettings settings, ILrApi api, string displayName, string key, 
            IEnumerationParameter<T> parameter, IEnumeration<T> value) : base(settings, api, displayName, key)
        {
            Parameter = parameter;
            Value = value;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;
            
            Api.LrDevelopController.SetValue(Parameter, Value);
        }
    }
}