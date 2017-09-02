using System;
using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class ParameterFunction<T> : Function
    {
        protected readonly IParameter<T> Parameter;
        protected Range ParameterRange;

        public ParameterFunction(LrApi api, string displayName, IParameter<T> parameter, string key)
            : base(api, displayName, key)
        {
            Parameter = parameter;

            api.LrDevelopController.AddParameterChangedListener(parameter, ParameterChanged);
        }

        public override void Enable()
        {
            base.Enable();
            UpdateControllerValue();
        }

        protected override void Disposing()
        {
            Api.LrDevelopController.RemoveParameterChangedListener(Parameter, ParameterChanged);
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (!UpdateRange()) return;

            var parameterValue = CalculateParamterValue(controllerValue);
            switch (Parameter)
            {
                case IParameter<int> intParameter:
                    var intValue = Convert.ToInt32(parameterValue);
                    Api.LrDevelopController.SetValue(intParameter, intValue);

                    ShowHud($"{intParameter.DisplayName}: {intValue}");
                    break;

                case IParameter<double> doubleParameter:
                    Api.LrDevelopController.SetValue(doubleParameter, parameterValue);

                    ShowHud($"{doubleParameter.DisplayName}: {parameterValue:F2}");
                    break;

                case IParameter<bool> boolParameter:
                    var enabled = controllerValue == (int)Controller.Range.Maximum;
                    Api.LrDevelopController.SetValue(boolParameter, enabled);

                    ShowHud($"{boolParameter.DisplayName}: {(enabled ? "Enabled" : "Disabled")}");
                    break;
            }
        }

        protected virtual double CalculateParamterValue(int controllerValue)
        {
            return ParameterRange.FromRange(Controller.Range, controllerValue);
        }

        private void ParameterChanged()
        {
            if (!Enabled) return;

            UpdateControllerValue();
        }

        private void UpdateControllerValue()
        {
            if (!UpdateRange()) return;

            Controller.SetControllerValue(CalculateControllerValue());
        }

        protected virtual int CalculateControllerValue()
        {
            switch (Parameter)
            {
                case IParameter<int> intParameter:
                    if (Api.LrDevelopController.GetValue(out var intValue, intParameter))
                    {
                        return Convert.ToInt32(Controller.Range.FromRange(ParameterRange, intValue));
                    }
                    break;
                case IParameter<double> doubleParameter:
                    if (Api.LrDevelopController.GetValue(out var doubleValue, doubleParameter))
                    {
                        return Convert.ToInt32(Controller.Range.FromRange(ParameterRange, doubleValue));
                    }
                    break;
                case IParameter<bool> boolParameter:
                    if (Api.LrDevelopController.GetValue(out var boolValue, boolParameter))
                    {
                        var controllerValue = boolValue ? Controller.Range.Maximum : Controller.Range.Minimum;
                        return Convert.ToInt32(controllerValue);
                    }
                    break;
            }
            return 0;
        }

        protected virtual bool UpdateRange()
        {
            return ParameterRange != null || Api.LrDevelopController.GetRange(out ParameterRange, Parameter);
        }
    }
}