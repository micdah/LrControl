using System;
using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;

namespace LrControl.Core.Functions.Factories
{
    internal class MethodFunctionFactory : FunctionFactory
    {
        private readonly Action<ILrApi> _method;

        public MethodFunctionFactory(ISettings settings, ILrApi api, string displayName, string key,
            Action<ILrApi> method) : base(settings, api)
        {
            _method = method;
            DisplayName = displayName;
            Key = $"MethodFunction:{key}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new MethodFunction(settings, api, DisplayName, Key, _method, DisplayName);
    }
}