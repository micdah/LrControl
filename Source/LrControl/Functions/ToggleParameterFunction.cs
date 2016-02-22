using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;
using Midi.Enums;

namespace micdah.LrControl.Functions
{
    public class ToggleParameterFunction : Function
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunction(LrControlApi.LrControlApi api, ControllerType controllerType, Channel channel, int controlNumber, Range controllerRange, IParameter<bool> parameter) : base(api, controllerType, channel, controlNumber, controllerRange)
        {
            _parameter = parameter;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue == (int)ControllerRange.Maximum)
            {
                bool enabled;
                if (Api.LrDevelopController.GetValue(out enabled, _parameter))
                {
                    Api.LrDevelopController.SetValue(_parameter, !enabled);
                }
            }
        }
    }
}