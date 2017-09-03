using System;
using LrControl.Api;
using LrControl.Core.Configurations;

namespace LrControl.Core.Functions.Factories
{
    internal class MethodFunctionFactory : FunctionFactory
    {
        private readonly Action<LrApi> _method;

        public MethodFunctionFactory(ISettings settings, LrApi api, string displayName, string key,
            Action<LrApi> method) : base(settings, api)
        {
            _method = method;
            DisplayName = displayName;
            Key = $"MethodFunction:{key}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, LrApi api)
        {
            return new MethodFunction(settings, api, DisplayName, _method, DisplayName, Key);
        }
    }
}