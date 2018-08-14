using System;
using LrControl.Core.Devices;
using LrControl.Core.Devices.Enums;
using LrControl.Core.Functions;

namespace LrControl.Core.Mapping
{
    public class ControllerFunction : IDisposable
    {
        private IFunction _function;

        internal ControllerFunction(Controller controller)
        {
            Controller = controller;
            Controller.ControllerValueChanged += OnControllerValueChanged;
        }

        public Controller Controller { get; private set; }

        public IFunction Function
        {
            get => _function;
            set
            {
                if (_function != null)
                {
                    _function.OnRequestUpdateControllerValue = null;
                }

                _function = value;

                if (_function != null)
                {
                    _function.OnRequestUpdateControllerValue = OnRequestUpdateControllerValue;
                }
            }
        }

        public bool Assignable { get; internal set; }

        private bool Enabled { get; set; }

        public bool HasFunction => Function != null;

        internal void Enable(bool resetIfUnmapped)
        {
            if(_function == null && resetIfUnmapped)
            {
                if (Controller.ControllerType == ControllerType.Encoder ||
                    Controller.ControllerType == ControllerType.Fader)
                {
                    Controller.Reset();
                }
            }

            Enabled = true;

            // Check if function has a value to initialize controller with
            OnRequestUpdateControllerValue();
        }

        internal void Disable()
        {
            Enabled = false;
        }

        public void Dispose()
        {
            if (Controller != null)
            {
                Controller.ControllerValueChanged -= OnControllerValueChanged;
                Controller = null;
            }

            _function = null;
        }

        private void OnRequestUpdateControllerValue()
        {
            if (!Enabled) return;
            if (_function == null) return;

            if (_function.UpdateControllerValue(out var controllerValue, Controller.Range))
            {
                Controller.SetControllerValue(controllerValue);
            }
        }

        private void OnControllerValueChanged(int controllerValue)
        {
            if (!Enabled) return;

            _function?.ControllerValueChanged(controllerValue, Controller.Range);
        }
    }
}