using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class ToggleParameterFunctionFactory : FunctionFactory
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunctionFactory(ISettings settings, LrApi api, IParameter<bool> parameter) : base(settings, api)
        {
            _parameter = parameter;
        }

        public override string DisplayName => $"Toggle {_parameter.DisplayName}";
        public override string Key => $"ToggleParameterFunction:{_parameter.Name}";

        protected override IFunction CreateFunction(ISettings settings, LrApi api)
        {
            return new ToggleParameterFunction(settings, api, DisplayName, _parameter, Key);
        }
    }
}