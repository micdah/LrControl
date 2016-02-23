using System;
using micdah.LrControlApi;

namespace micdah.LrControl.Mapping.Functions
{
    public class MethodFunctionFactory : FunctionFactory
    {
        private readonly Action<LrApi> _method;

        public MethodFunctionFactory(LrApi api, string displayName, string key,
            Action<LrApi> method) : base(api)
        {
            _method = method;
            DisplayName = displayName;
            Key = $"MethodFunction:{key}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override Function CreateFunction(LrApi api)
        {
            return new MethodFunction(api, _method, DisplayName);
        }
    }
}