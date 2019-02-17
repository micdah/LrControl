using System;
using LrControl.Core.Devices;
using LrControl.Core.Devices.Enums;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Mapping
{
    public class ControllerFunction : IDisposable
    {
        private bool _enabled;

        internal ControllerFunction(Controller controller)
        {
            Controller = controller;
            Controller.ValueChanged += OnControllerValueChanged;
        }

        public Controller Controller { get; private set; }
        public IFunction Function { get; set; }
        public bool Assignable { get; internal set; }

        internal void Enable(bool resetIfUnmapped)
        {
            if(Function == null && resetIfUnmapped)
            {
                if (Controller.ControllerType == ControllerType.Encoder ||
                    Controller.ControllerType == ControllerType.Fader)
                {
                    Controller.Reset();
                }
            }

            _enabled = true;

            // Check if function has a value to initialize controller with
            UpdateController();
        }

        internal void Disable()
        {
            _enabled = false;
        }

        internal void UpdateController(IParameter parameter = null)
        {
            if (!_enabled) return;
            if (Function == null) return;
            if (!(Function is ParameterFunction parameterFunction)) return;
            if (parameter != null && !parameter.Equals(parameterFunction.Parameter)) return;

            if (parameterFunction.TryGetControllerValue(out var controllerValue, Controller.Range))
            {
                Controller.UpdateController(controllerValue);
            }
        }

        public void Dispose()
        {
            if (Controller != null)
            {
                Controller.ValueChanged -= OnControllerValueChanged;
                Controller = null;
            }

            Function = null;
        }

        private void OnControllerValueChanged(int controllerValue)
        {
            if (!_enabled) return;

            Function?.Apply(controllerValue, Controller.Range, null, null);
        }
    }
}