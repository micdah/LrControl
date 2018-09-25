using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class ParameterFunction<T> : Function
    {
        protected readonly IParameter<T> Parameter;
        protected Range ParameterRange;
        
        public ParameterFunction(ISettings settings, LrApi api, string displayName, IParameter<T> parameter, string key)
            : base(settings, api, displayName, key)
        {
            Parameter = parameter;

            api.LrDevelopController.AddParameterChangedListener(parameter, RequestUpdateControllerValue);
        }

        protected override void Disposing()
        {
            Api.LrDevelopController.RemoveParameterChangedListener(Parameter, RequestUpdateControllerValue);
        }

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

                case IParameter<bool> boolParameter:
                    var enabled = controllerValue == (int)controllerRange.Maximum;
                    Api.LrDevelopController.SetValue(boolParameter, enabled);

                    ShowHud($"{boolParameter.DisplayName}: {(enabled ? "Enabled" : "Disabled")}");
                    break;
            }
        }

        protected virtual double CalculateParameterValue(int controllerValue, Range controllerRange)
        {
            return ParameterRange.FromRange(controllerRange, controllerValue);
        }

        public override bool UpdateControllerValue(out int controllerValue, Range controllerRange)
        {
            if (!UpdateRange(controllerRange))
            {
                controllerValue = default(int);
                return false;
            }

            controllerValue = CalculateControllerValue(controllerRange);
            return true;
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
                case IParameter<bool> boolParameter:
                    if (Api.LrDevelopController.GetValue(out var boolValue, boolParameter))
                    {
                        return (int)(boolValue ? controllerRange.Maximum : controllerRange.Minimum);
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