using System;
using LrControl.Core.Configurations;
using LrControl.Core.Util;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using Serilog;

namespace LrControl.Core.Functions.Factories
{
    internal class ParameterFunctionFactory : FunctionFactory
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ParameterFunctionFactory>();
        public IParameter Parameter { get; }

        public ParameterFunctionFactory(ISettings settings, ILrApi api, IParameter parameter) : base(settings, api)
        {
            if (!parameter.GetType().IsTypeOf(typeof(IParameter<>)))
                throw new ArgumentException($"Unsupported parameter type {parameter.GetType()}");
            
            Parameter = parameter;
            DisplayName = $"Change {parameter.DisplayName}";
            Key = $"ParameterFunction:{parameter.Name}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            switch (Parameter)
            {
                case IParameter<double> temperatureParameter when ReferenceEquals(temperatureParameter, AdjustPanelParameter.Temperature):
                    return new TemperatureParameterFunction(settings, api, DisplayName, Key, temperatureParameter);
                case IParameter<int> intParameter:
                    return new ParameterFunction(settings, api, DisplayName, Key, intParameter);
                case IParameter<double> doubleParameter:
                    return new ParameterFunction(settings, api, DisplayName, Key, doubleParameter);
                case IParameter<bool> boolParameter:
                    return new ToggleParameterFunction(settings, api, DisplayName, Key, boolParameter);
                default:
                {
                    Log.Error("Unsupported Parameter {Parameter}", Parameter);
                    throw new ArgumentException("Unsupported parameter type");
                }
            }
        }
    }
}