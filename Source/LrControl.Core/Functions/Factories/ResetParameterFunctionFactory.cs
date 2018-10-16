using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class ResetParameterFunctionFactory : FunctionFactory
    {
        private readonly IParameter _parameter;

        public ResetParameterFunctionFactory(ISettings settings, ILrApi api, IParameter parameter) : base(settings, api)
        {
            _parameter = parameter;
            DisplayName = $"Reset {parameter.DisplayName} to default";
            Key = $"ResetToDefault{parameter.Name}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ResetParameterFunction(settings, api, DisplayName, Key, _parameter);
    }
}