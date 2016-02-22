using System;

namespace micdah.LrControl.Mapping.Functions
{
    public class MethodFunction : Function
    {
        private readonly Action<LrControlApi.LrControlApi> _method;

        public MethodFunction(LrControlApi.LrControlApi api, Action<LrControlApi.LrControlApi> method) : base(api)
        {
            _method = method;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue == (int) Controller.Range.Maximum)
            {
                _method(Api);
            }
        }
    }
}