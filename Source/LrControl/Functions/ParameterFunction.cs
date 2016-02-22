using System;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;
using Midi.Enums;

namespace micdah.LrControl.Functions
{
    public class ParameterFunction : Function
    {
        private const double ExposureParameterRangeSplit = 12000.0/48000.0;
        private const double ExposureControllerRangeSplit = 85.0/100.0;
        private readonly IParameter<bool> _boolParameter;
        private readonly IParameter<double> _doubleParameter;
        private readonly IParameter<int> _intParameter;

        private Range _parameterRange;

        public ParameterFunction(LrControlApi.LrControlApi api, ControllerType controllerType, Channel channel,
            int controlNumber, Range controllerRange, IParameter parameter)
            : base(api, controllerType, channel, controlNumber, controllerRange)
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
                Api.LrDevelopController.SetValue(_intParameter, Convert.ToInt32(parameterValue));
            }
            else if (_doubleParameter != null)
            {
                Api.LrDevelopController.SetValue(_doubleParameter, parameterValue);
            }
            else if (_boolParameter != null)
            {
                Api.LrDevelopController.SetValue(_boolParameter, controllerValue == (int) ControllerRange.Maximum);
            }
        }

        private void LrDevelopControllerOnParameterChanged(IParameter parameter)
        {
            if (!Enabled) return;
            if (parameter != _intParameter && parameter != _doubleParameter && parameter != _boolParameter) return;
            if (OutputDevice == null) return;

            UpdateControllerValue();
        }

        private void UpdateControllerValue()
        {
            if (OutputDevice == null) return;
            if (!UpdateRange()) return;

            switch (ControllerType)
            {
                case ControllerType.ControlChange:
                    OutputDevice.SendControlChange(Channel, (Control) ControlNumber, CalculateControllerValue());
                    break;
                case ControllerType.Nrpn:
                    OutputDevice.SendNrpn(Channel, ControlNumber, CalculateControllerValue());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private double CalculateParamterValue(int controllerValue)
        {
            if (_doubleParameter == Parameters.AdjustPanelParameters.Temperature)
            {
                return CalculateExposureParameterValue(controllerValue);
            }

            return _parameterRange.FromRange(ControllerRange, controllerValue);
        }

        private int CalculateControllerValue()
        {
            if (_intParameter != null)
            {
                int value;
                if (Api.LrDevelopController.GetValue(out value, _intParameter))
                {
                    return Convert.ToInt32(ControllerRange.FromRange(_parameterRange, value));
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
                    return Convert.ToInt32(ControllerRange.FromRange(_parameterRange, value));
                }
            }
            else if (_boolParameter != null)
            {
                bool value;
                if (Api.LrDevelopController.GetValue(out value, _boolParameter))
                {
                    var controllerValue = value
                        ? ControllerRange.Maximum
                        : ControllerRange.Minimum;
                    return Convert.ToInt32(controllerValue);
                }
            }
            return 0;
        }

        private double CalculateExposureParameterValue(int controllerValue)
        {
            if (controllerValue < (ControllerRange.Maximum - ControllerRange.Minimum)*ExposureControllerRangeSplit)
            {
                return new Range(_parameterRange.Minimum, _parameterRange.Maximum*ExposureParameterRangeSplit)
                    .FromRange(new Range(0, ControllerRange.Maximum*ExposureControllerRangeSplit), controllerValue);
            }
            // Otherwise
            return new Range(_parameterRange.Maximum*ExposureParameterRangeSplit, _parameterRange.Maximum)
                .FromRange(new Range(ControllerRange.Maximum*ExposureControllerRangeSplit, ControllerRange.Maximum),
                    controllerValue);
        }

        private int CalculateExposureControllerValue(double value)
        {
            double controllerValue;
            if (value < _parameterRange.Maximum*ExposureParameterRangeSplit)
            {
                controllerValue = new Range(0, ControllerRange.Maximum*ExposureControllerRangeSplit)
                    .FromRange(new Range(_parameterRange.Minimum, _parameterRange.Maximum*ExposureParameterRangeSplit),
                        value);
            }
            else
            {
                controllerValue = new Range(ControllerRange.Maximum*ExposureControllerRangeSplit,
                    ControllerRange.Maximum)
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