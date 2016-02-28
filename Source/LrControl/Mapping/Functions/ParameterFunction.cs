using System;
using micdah.LrControlApi;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;

namespace micdah.LrControl.Mapping.Functions
{
    public class ParameterFunction : Function
    {
        private const double ExposureParameterRangeSplit = 12000.0/48000.0;
        private const double ExposureControllerRangeSplit = 85.0/100.0;
        private readonly IParameter<bool> _boolParameter;
        private readonly IParameter<double> _doubleParameter;
        private readonly IParameter<int> _intParameter;
        private Range _parameterRange;

        public ParameterFunction(LrApi api, string displayName, IParameter parameter, string key)
            : base(api, displayName, key)
        {
            _intParameter = parameter as IParameter<int>;
            _doubleParameter = parameter as IParameter<double>;
            _boolParameter = parameter as IParameter<bool>;

            if (_intParameter == null && _doubleParameter == null && _boolParameter == null)
                throw new ArgumentException(@"Unsupported parameter type", nameof(parameter));

            api.LrDevelopController.ParameterChanged += LrDevelopControllerOnParameterChanged;
        }

        public override void Enable()
        {
            base.Enable();
            UpdateControllerValue();
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (!UpdateRange()) return;

            var parameterValue = CalculateParamterValue(controllerValue);
            if (_intParameter != null)
            {
                var intValue = Convert.ToInt32(parameterValue);
                Api.LrDevelopController.SetValue(_intParameter, intValue);

                ShowHud($"{_intParameter.DisplayName}: {intValue}");
            }
            else if (_doubleParameter != null)
            {
                Api.LrDevelopController.SetValue(_doubleParameter, parameterValue);

                ShowHud($"{_doubleParameter.DisplayName}: {parameterValue:F2}");
            }
            else if (_boolParameter != null)
            {
                var enabled = controllerValue == (int) Controller.Range.Maximum;
                Api.LrDevelopController.SetValue(_boolParameter, enabled);

                ShowHud($"{_boolParameter.DisplayName}: {(enabled ? "Enabled" : "Disabled")}");
            }
        }

        private double CalculateParamterValue(int controllerValue)
        {
            if (_doubleParameter == Parameters.AdjustPanelParameters.Temperature)
            {
                return CalculateExposureParameterValue(controllerValue);
            }

            return _parameterRange.FromRange(Controller.Range, controllerValue);
        }

        private void LrDevelopControllerOnParameterChanged(IParameter parameter)
        {
            if (!Enabled) return;
            if (parameter != _intParameter && parameter != _doubleParameter && parameter != _boolParameter) return;

            UpdateControllerValue();
        }

        private void UpdateControllerValue()
        {
            if (!UpdateRange()) return;

            Controller.SetControllerValue(CalculateControllerValue());
        }

        private int CalculateControllerValue()
        {
            if (_intParameter != null)
            {
                int value;
                if (Api.LrDevelopController.GetValue(out value, _intParameter))
                {
                    return Convert.ToInt32(Controller.Range.FromRange(_parameterRange, value));
                }
            }
            else if (_doubleParameter != null)
            {
                double value;
                if (Api.LrDevelopController.GetValue(out value, _doubleParameter))
                {
                    if (_doubleParameter == Parameters.AdjustPanelParameters.Temperature)
                    {
                        return CalculateExposureControllerValue(value);
                    }
                    return Convert.ToInt32(Controller.Range.FromRange(_parameterRange, value));
                }
            }
            else if (_boolParameter != null)
            {
                bool value;
                if (Api.LrDevelopController.GetValue(out value, _boolParameter))
                {
                    var controllerValue = value
                        ? Controller.Range.Maximum
                        : Controller.Range.Minimum;
                    return Convert.ToInt32(controllerValue);
                }
            }
            return 0;
        }

        private int CalculateExposureParameterValue(int controllerValue)
        {
            if (controllerValue < (Controller.Range.Maximum - Controller.Range.Minimum)*ExposureControllerRangeSplit)
            {
                return (int) new Range(_parameterRange.Minimum, _parameterRange.Maximum*ExposureParameterRangeSplit)
                    .FromRange(new Range(0, Controller.Range.Maximum*ExposureControllerRangeSplit), controllerValue);
            }
            // Otherwise
            return (int) new Range(_parameterRange.Maximum*ExposureParameterRangeSplit, _parameterRange.Maximum)
                .FromRange(new Range(Controller.Range.Maximum*ExposureControllerRangeSplit, Controller.Range.Maximum),
                    controllerValue);
        }

        private int CalculateExposureControllerValue(double value)
        {
            double controllerValue;
            if (value < _parameterRange.Maximum*ExposureParameterRangeSplit)
            {
                controllerValue = new Range(0, Controller.Range.Maximum*ExposureControllerRangeSplit)
                    .FromRange(new Range(_parameterRange.Minimum, _parameterRange.Maximum*ExposureParameterRangeSplit),
                        value);
            }
            else
            {
                controllerValue = new Range(Controller.Range.Maximum*ExposureControllerRangeSplit,
                    Controller.Range.Maximum)
                    .FromRange(new Range(_parameterRange.Maximum*ExposureParameterRangeSplit, _parameterRange.Maximum),
                        value);
            }
            return Convert.ToInt32(controllerValue);
        }

        private bool UpdateRange()
        {
            if (_parameterRange != null) return true;

            if (_intParameter != null)
            {
                return Api.LrDevelopController.GetRange(out _parameterRange, _intParameter);
            }
            if (_doubleParameter != null)
            {
                return Api.LrDevelopController.GetRange(out _parameterRange, _doubleParameter);
            }
            if (_boolParameter != null)
            {
                return Api.LrDevelopController.GetRange(out _parameterRange, _boolParameter);
            }
            return false;
        }
    }
}