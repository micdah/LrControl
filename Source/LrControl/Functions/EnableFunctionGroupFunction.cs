using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;
using Midi.Enums;

namespace micdah.LrControl.Functions
{
    public class EnableFunctionGroupFunction : Function
    {
        private readonly FunctionGroup _functionGroup;
        private readonly IParameter<bool> _enablePanelParamter; 

        public EnableFunctionGroupFunction(LrControlApi.LrControlApi api, ControllerType controllerType, Channel channel, int controlNumber, Range controllerRange, FunctionGroup functionGroup, IParameter<bool> enablePanelParameter) : base(api, controllerType, channel, controlNumber, controllerRange)
        {
            _functionGroup = functionGroup;
            _enablePanelParamter = enablePanelParameter;
        }

        protected override void ControllerChanged(int controllerValue)
        {
            if (controllerValue != ControllerRange.Maximum) return;

            if (!_functionGroup.Enabled)
            {
                _functionGroup.Enable();
            }
            else
            {
                if (_enablePanelParamter == null) return;

                bool enabled;
                if (Api.LrDevelopController.GetValue(out enabled, _enablePanelParamter))
                {
                    Api.LrDevelopController.SetValue(_enablePanelParamter, !enabled);
                }
            }
        }
    }
}