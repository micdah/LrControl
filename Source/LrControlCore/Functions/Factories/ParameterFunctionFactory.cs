using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;

namespace LrControlCore.Functions.Factories
{
    public class ParameterFunctionFactory : FunctionFactory
    {
        private readonly IParameter _parameter;

        public ParameterFunctionFactory(LrApi api, IParameter parameter) : base(api)
        {
            _parameter = parameter;
        }

        public override string DisplayName => $"Change {_parameter.DisplayName}";
        public override string Key => $"ParameterFunction:{_parameter.Name}";

        protected override Function CreateFunction(LrApi api)
        {
            if (ReferenceEquals(_parameter, Parameters.AdjustPanelParameters.Temperature))
            {
                return new TemperatureParameterFunction(api, DisplayName, _parameter, Key);
            }
            
            return new ParameterFunction(api, DisplayName, _parameter, Key);
        }
    }
}