using LrControl.Api;
using LrControl.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class ToggleParameterFunctionFactory : FunctionFactory
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunctionFactory(LrApi api, IParameter<bool> parameter) : base(api)
        {
            _parameter = parameter;
        }

        public override string DisplayName => $"Toggle {_parameter.DisplayName}";
        public override string Key => $"ToggleParameterFunction:{_parameter.Name}";

        protected override IFunction CreateFunction(LrApi api)
        {
            return new ToggleParameterFunction(api, DisplayName, _parameter, Key);
        }
    }
}