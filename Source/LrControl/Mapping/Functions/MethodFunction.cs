using System;
using micdah.LrControlApi.Common;
using Midi.Enums;

namespace micdah.LrControl.Mapping.Functions
{
    public class MethodFunction : Function
    {
        private readonly Action<LrControlApi.LrControlApi> _method;

        public MethodFunction(LrControlApi.LrControlApi api, ControllerType controllerType, Channel channel, int controlNumber, Range controllerRange, Action<LrControlApi.LrControlApi> method) : base(api, controllerType, channel, controlNumber, controllerRange)
        {
            _method = method;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue == (int)ControllerRange.Maximum)
            {
                _method(Api);
            }
        }
    }
}