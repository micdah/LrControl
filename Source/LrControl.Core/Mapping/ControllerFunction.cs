using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Core.Devices;
using LrControl.Core.Devices.Enums;
using LrControl.Core.Functions;

namespace LrControl.Core.Mapping
{
    public class ControllerFunction : INotifyPropertyChanged, IDisposable
    {
        private IFunction _function;
        private bool _assignable;

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

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFunction));
            }
        }

        public bool Assignable
        {
            get => _assignable;
            internal set
            {
                if (value == _assignable) return;
                _assignable = value;
                OnPropertyChanged();
            }
        }

        private bool Enabled { get; set; }

        public bool HasFunction => Function != null;

        public event PropertyChangedEventHandler PropertyChanged;

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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}