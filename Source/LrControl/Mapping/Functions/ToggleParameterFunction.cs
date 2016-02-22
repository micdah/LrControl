using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControl.Mapping.Functions
{
    public class ToggleParameterFunction : Function
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunction(LrControlApi.LrControlApi api, IParameter<bool> parameter) : base(api)
        {
            _parameter = parameter;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue == (int) Controller.Range.Maximum)
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