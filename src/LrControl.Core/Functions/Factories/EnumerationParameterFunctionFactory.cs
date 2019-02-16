using System;
using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class EnumerationParameterFunctionFactory<TValue> : FunctionFactory 
        where TValue : IComparable
    {
        private readonly IEnumerationParameter<TValue> _parameter;
        private readonly IEnumeration<TValue> _value;

        public EnumerationParameterFunctionFactory(ISettings settings, ILrApi api, 
            IEnumerationParameter<TValue> parameter, IEnumeration<TValue> value) : base(settings, api)
        {
            _parameter = parameter;
            _value = value;

            DisplayName = $"Set {parameter.DisplayName} to {value.Name}";
            Key = $"Set{parameter.Name}To{value.Value}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }
        
        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            return new EnumerationParameterFunction<TValue>(settings, api, DisplayName, Key, _parameter, _value);
        }
    }
}