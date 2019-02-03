using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal class TemperatureParameterFunction : ParameterFunction
    {
        private const double ParameterRangeSplit = 12000.0/48000.0;
        private const double ControllerRangeSplit = 85.0/100.0;
        private Range _controllerRangeHigh;
        private Range _controllerRangeLow;
        private Range _parameterRangeHigh;
        private Range _parameterRangeLow;
        private readonly IParameter<double> _parameter;

        public TemperatureParameterFunction(ISettings settings, ILrApi api, string displayName, string key,
            IParameter<double> parameter)
            : base(settings, api, displayName, key, parameter)
        {
            _parameter = parameter;
        }

        protected override bool UpdateRange(Range controllerRange)
        {
            if (!base.UpdateRange(controllerRange)) return false;

            _parameterRangeLow = new Range(ParameterRange.Minimum, ParameterRange.Maximum*ParameterRangeSplit);
            _parameterRangeHigh = new Range(ParameterRange.Maximum*ParameterRangeSplit, ParameterRange.Maximum);

            _controllerRangeLow = new Range(0, controllerRange.Maximum*ControllerRangeSplit);
            _controllerRangeHigh = new Range(controllerRange.Maximum*ControllerRangeSplit, controllerRange.Maximum);
            return true;
        }

        protected override int CalculateControllerValue(Range controllerRange)
        {
            if (!Api.LrDevelopController.GetValue(out var value, _parameter)) return 0;

            var controllerValue = value < ParameterRange.Maximum * ParameterRangeSplit
                ? _controllerRangeLow.FromRange(_parameterRangeLow, value)
                : _controllerRangeHigh.FromRange(_parameterRangeHigh, value);

            return (int)controllerValue;
        }

        protected override double CalculateParameterValue(int controllerValue, Range controllerRange)
        {
            return controllerValue < (controllerRange.Maximum - controllerRange.Minimum)*ControllerRangeSplit
                ? (int) _parameterRangeLow.FromRange(_controllerRangeLow, controllerValue)
                : (int) _parameterRangeHigh.FromRange(_controllerRangeHigh, controllerValue);
        }
    }
}