using System;
using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class TemperatureParameterFunction : ParameterFunction<double>
    {
        private const double ParameterRangeSplit = 12000.0/48000.0;
        private const double ControllerRangeSplit = 85.0/100.0;
        private Range _controllerRangeHigh;
        private Range _controllerRangeLow;
        private Range _parameterRangeHigh;

        private Range _parameterRangeLow;

        public TemperatureParameterFunction(LrApi api, string displayName, IParameter<double> parameter, string key)
            : base(api, displayName, parameter, key)
        {
        }

        protected override bool UpdateRange()
        {
            if (!base.UpdateRange()) return false;

            _parameterRangeLow = new Range(ParameterRange.Minimum, ParameterRange.Maximum*ParameterRangeSplit);
            _parameterRangeHigh = new Range(ParameterRange.Maximum*ParameterRangeSplit, ParameterRange.Maximum);

            _controllerRangeLow = new Range(0, Controller.Range.Maximum*ControllerRangeSplit);
            _controllerRangeHigh = new Range(Controller.Range.Maximum*ControllerRangeSplit, Controller.Range.Maximum);
            return true;
        }

        protected override int CalculateControllerValue()
        {
            if (!Api.LrDevelopController.GetValue(out var value, Parameter)) return 0;

            var controllerValue = value < ParameterRange.Maximum * ParameterRangeSplit
                ? _controllerRangeLow.FromRange(_parameterRangeLow, value)
                : _controllerRangeHigh.FromRange(_parameterRangeHigh, value);

            return Convert.ToInt32(controllerValue);
        }

        protected override double CalculateParamterValue(int controllerValue)
        {
            return controllerValue < (Controller.Range.Maximum - Controller.Range.Minimum)*ControllerRangeSplit
                ? (int) _parameterRangeLow.FromRange(_controllerRangeLow, controllerValue)
                : (int) _parameterRangeHigh.FromRange(_controllerRangeHigh, controllerValue);
        }
    }
}