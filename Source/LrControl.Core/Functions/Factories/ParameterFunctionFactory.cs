using System;
using LrControl.Api;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Api.Modules.LrDevelopController.Parameters;
using Serilog;

namespace LrControl.Core.Functions.Factories
{
    internal class ParameterFunctionFactory : FunctionFactory
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ParameterFunctionFactory>();
        private readonly IParameter _parameter;

        public ParameterFunctionFactory(LrApi api, IParameter parameter) : base(api)
        {
            _parameter = parameter;
        }

        public override string DisplayName => $"Change {_parameter.DisplayName}";
        public override string Key => $"ParameterFunction:{_parameter.Name}";

        protected override IFunction CreateFunction(LrApi api)
        {
            switch (_parameter)
            {
                case IParameter<double> temperatureParameter when ReferenceEquals(temperatureParameter, Parameters.AdjustPanelParameters.Temperature):
                    return new TemperatureParameterFunction(api, DisplayName, temperatureParameter, Key);
                case IParameter<int> intParameter:
                    return new ParameterFunction<int>(api, DisplayName, intParameter, Key);
                case IParameter<double> doubleParameter:
                    return new ParameterFunction<double>(api, DisplayName, doubleParameter, Key);
                case IParameter<bool> boolParameter:
                    return new ParameterFunction<bool>(api, DisplayName, boolParameter, Key);
                default:
                {
                    Log.Error("Unsupported Parameter {Parameter}", _parameter);
                    throw new ArgumentException("Unsupported parameter type");
                }
            }
        }
    }
}