using System;
using LrControl.Core.Configurations;
using LrControl.Core.Util;
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

        public EnumerationParameterFunctionFactory(ISettings settings, LrApi api, 
            IEnumerationParameter<TValue> parameter, IEnumeration<TValue> value) : base(settings, api)
        {
            if (!parameter.GetType().IsTypeOf(typeof(IEnumerationParameter<>)))
                throw new ArgumentException($"Unsupported parameter type {parameter.GetType()}");
            
            _parameter = parameter;
            _value = value;

            DisplayName = $"Set {parameter.DisplayName} to {value.Name}";
            Key = $"Set{parameter.Name}To{value.Value}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }
        
        protected override IFunction CreateFunction(ISettings settings, LrApi api)
        {
            return new EnumerationParameterFunction<TValue>(settings, api, DisplayName, Key, _parameter, _value);
        }
    }
}