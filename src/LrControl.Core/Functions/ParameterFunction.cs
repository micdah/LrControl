using System;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class ParameterFunction : Function
    {
        protected Range ParameterRange;
        
        public ParameterFunction(ISettings settings, ILrApi api, string displayName, string key, IParameter parameter)
            : base(settings, api, displayName, key)
        {
            if (!(parameter is IParameter<int>) && !(parameter is IParameter<double>))
                throw new ArgumentException("Unsupported parameter type", nameof(parameter));
            
            Parameter = parameter;
        }
        
        public IParameter Parameter { get; }

        public override void ControllerValueChanged(int controllerValue, Range controllerRange)
        {
            if (!UpdateRange(controllerRange)) return;

            var parameterValue = CalculateParameterValue(controllerValue, controllerRange);
            switch (Parameter)
            {
                case IParameter<int> intParameter:
                    var intValue = (int)parameterValue;
                    Api.LrDevelopController.SetValue(intParameter, intValue);

                    ShowHud($"{intParameter.DisplayName}: {intValue}");
                    break;

                case IParameter<double> doubleParameter:
                    Api.LrDevelopController.SetValue(doubleParameter, parameterValue);

                    ShowHud($"{doubleParameter.DisplayName}: {parameterValue:F2}");
                    break;
            }
        }

        public bool TryGetControllerValue(out int controllerValue, Range controllerRange)
        {
            if (!UpdateRange(controllerRange))
            {
                controllerValue = default;
                return false;
            }

            controllerValue = CalculateControllerValue(controllerRange);
            return true;
        }

        protected virtual double CalculateParameterValue(int controllerValue, Range controllerRange)
        {
            return ParameterRange.FromRange(controllerRange, controllerValue);
        }

        protected virtual int CalculateControllerValue(Range controllerRange)
        {
            switch (Parameter)
            {
                case IParameter<int> intParameter:
                    if (Api.LrDevelopController.GetValue(out var intValue, intParameter))
                    {
                        return (int)controllerRange.FromRange(ParameterRange, intValue);
                    }
                    break;
                case IParameter<double> doubleParameter:
                    if (Api.LrDevelopController.GetValue(out var doubleValue, doubleParameter))
                    {
                        return (int)controllerRange.FromRange(ParameterRange, doubleValue);
                    }
                    break;
            }
            return 0;
        }

        protected virtual bool UpdateRange(Range controllerRange)
        {
            return ParameterRange != null || Api.LrDevelopController.GetRange(out ParameterRange, Parameter);
        }
    }
}