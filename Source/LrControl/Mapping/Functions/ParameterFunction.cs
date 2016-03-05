using System;
using micdah.LrControlApi;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping.Functions
{
    public class ParameterFunction : Function
    {
        protected readonly IParameter<bool> BoolParameter;
        protected readonly IParameter<double> DoubleParameter;
        protected readonly IParameter<int> IntParameter;
        protected Range ParameterRange;

        public ParameterFunction(LrApi api, string displayName, IParameter parameter, string key)
            : base(api, displayName, key)
        {
            IntParameter = parameter as IParameter<int>;
            DoubleParameter = parameter as IParameter<double>;
            BoolParameter = parameter as IParameter<bool>;

            if (IntParameter == null && DoubleParameter == null && BoolParameter == null)
                throw new ArgumentException(@"Unsupported parameter type", nameof(parameter));

            api.LrDevelopController.AddParameterChangedListener(parameter, ParameterChanged);
        }

        private IParameter Parameter
        {
            get
            {
                if (IntParameter != null)
                    return IntParameter;
                if (BoolParameter != null)
                    return BoolParameter;
                if (DoubleParameter != null)
                    return DoubleParameter;
                throw new InvalidOperationException();
            }
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
            if (IntParameter != null)
            {
                var intValue = Convert.ToInt32(parameterValue);
                Api.LrDevelopController.SetValue(IntParameter, intValue);

                ShowHud($"{IntParameter.DisplayName}: {intValue}");
            }
            else if (DoubleParameter != null)
            {
                Api.LrDevelopController.SetValue(DoubleParameter, parameterValue);

                ShowHud($"{DoubleParameter.DisplayName}: {parameterValue:F2}");
            }
            else if (BoolParameter != null)
            {
                var enabled = controllerValue == (int) Controller.Range.Maximum;
                Api.LrDevelopController.SetValue(BoolParameter, enabled);

                ShowHud($"{BoolParameter.DisplayName}: {(enabled ? "Enabled" : "Disabled")}");
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
            if (IntParameter != null)
            {
                int value;
                if (Api.LrDevelopController.GetValue(out value, IntParameter))
                {
                    return Convert.ToInt32(Controller.Range.FromRange(ParameterRange, value));
                }
            }
            else if (DoubleParameter != null)
            {
                double value;
                if (Api.LrDevelopController.GetValue(out value, DoubleParameter))
                {
                    return Convert.ToInt32(Controller.Range.FromRange(ParameterRange, value));
                }
            }
            else if (BoolParameter != null)
            {
                bool value;
                if (Api.LrDevelopController.GetValue(out value, BoolParameter))
                {
                    var controllerValue = value
                        ? Controller.Range.Maximum
                        : Controller.Range.Minimum;
                    return Convert.ToInt32(controllerValue);
                }
            }
            return 0;
        }

        protected virtual bool UpdateRange()
        {
            return ParameterRange != null || Api.LrDevelopController.GetRange(out ParameterRange, Parameter);
        }
    }
}