using System;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;
using Midi.Enums;

namespace micdah.LrControl.Functions
{
    public class ParameterFunction : Function
    {
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
                Api.LrDevelopController.SetValue(_boolParameter, controllerValue == ControllerRange.Maximum);
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
            if (_intParameter == Parameters.AdjustPanelParameters.Temperature)
            {
                if (controllerValue < (ControllerRange.Maximum - ControllerRange.Minimum)/4*3)
                {
                    //var expoRange = new Range(0, Math.Pow((_parameterRange.Maximum - _parameterRange.Minimum)/4, 1.0/3));
                    //var expoValue = expoRange.FromRange(ControllerRange, controllerValue);
                    //return _parameterRange.Minimum + Math.Pow(expoValue, 3);
                    return new Range(_parameterRange.Minimum, _parameterRange.Maximum/4).FromRange(new Range(0, ControllerRange.Maximum/4*3), controllerValue);
                }
                else
                {
                    return
                        new Range(_parameterRange.Maximum/4, _parameterRange.Maximum).FromRange(
                            new Range(ControllerRange.Maximum/4*3, ControllerRange.Maximum), controllerValue);
                    //var expoRange = new Range(0, Math.Pow((_parameterRange.Maximum - _parameterRange.Minimum)/4*3, 1.0 / 3));
                    //var expoValue = expoRange.FromRange(ControllerRange, controllerValue);
                    //return _parameterRange.Minimum + (_parameterRange.Maximum - _parameterRange.Minimum)/4 + Math.Pow(expoValue, 3);
                }
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
                    //if (_intParameter == Parameters.AdjustPanelParameters.Temperature)
                    //{
                    //    if (value < _parameterRange.Maximum/4)
                    //    {
                    //        return Convert.ToInt32(new Range(0, ControllerRange.Maximum/4*3).FromRange(
                    //            new Range(_parameterRange.Minimum, _parameterRange.Maximum/4), value));
                    //    }
                    //}

                    return Convert.ToInt32(ControllerRange.FromRange(_parameterRange, value));
                }
            }
            else if (_doubleParameter != null)
            {
                double value;
                if (Api.LrDevelopController.GetValue(out value, _doubleParameter))
                {
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