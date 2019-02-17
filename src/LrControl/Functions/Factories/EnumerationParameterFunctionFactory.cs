using System;
using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions.Factories
{
    public class EnumerationParameterFunctionFactory<TValue> : FunctionFactory 
        where TValue : IComparable
    {
        public IEnumerationParameter<TValue> Parameter { get; }
        public IEnumeration<TValue> Value { get; }

        public EnumerationParameterFunctionFactory(ISettings settings, ILrApi api, 
            IEnumerationParameter<TValue> parameter, IEnumeration<TValue> value) : base(settings, api)
        {
            Parameter = parameter;
            Value = value;

            DisplayName = $"Set {parameter.DisplayName} to {value.Name}";
            Key = $"Set{parameter.Name}To{value.Value}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }
        
        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            return new EnumerationParameterFunction<TValue>(settings, api, DisplayName, Key, Parameter, Value);
        }
    }
}